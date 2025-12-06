import os
import json
import logging
from pathlib import Path
from typing import Dict, Any, Optional, Union

try:
    from lupa import LuaRuntime
except ImportError:
    raise ImportError(
        "lupa is required. Install it with: pip install lupa\n"
        "Alternatively, you can use the subprocess adapter (python_subprocess_adapter.py)"
    )


class EnvValidator:
    
    def __init__(self, lua_path: Optional[str] = None, logger: Optional[logging.Logger] = None):
        self.lua_path = lua_path or self._find_lua_path()
        self.logger = logger or self._create_default_logger()
        self.lua = LuaRuntime()
        self._load_lua_modules()
    
    def _create_default_logger(self) -> logging.Logger:
        logger = logging.getLogger('EnvValidator')
        if not logger.handlers:
            handler = logging.StreamHandler()
            formatter = logging.Formatter('[EnvValidator] %(message)s')
            handler.setFormatter(formatter)
            logger.addHandler(handler)
            logger.setLevel(logging.INFO)
        return logger
    
    def _find_lua_path(self) -> str:
        adapter_dir = Path(__file__).parent.resolve()
        root_dir = adapter_dir.parent.parent.resolve()
        lua_dir = root_dir / "lua"
        
        if (lua_dir / "env_validation.lua").exists():
            return str(lua_dir)
        
        parent_root = root_dir.parent
        lua_dir = parent_root / "lua"
        if (lua_dir / "env_validation.lua").exists():
            return str(lua_dir)
        
        return str(root_dir / "lua")
    
    def _load_lua_modules(self):
        lua_path = Path(self.lua_path).resolve()
        if not (lua_path / "env_validation.lua").exists():
            raise FileNotFoundError(f"Lua module directory not found: {lua_path}")
        
        lua_path_str = str(lua_path).replace("\\", "/")
        
        path_code = f'package.path = package.path .. ";{lua_path_str}/?.lua"'
        self.lua.execute(path_code)
        
        load_code = '''
            local env = require("env_validation")
            return env
        '''
        self.env_module = self.lua.execute(load_code)
    
    def validate(
        self, 
        schema: Union[str, Dict[str, Any]], 
        env_vars: Optional[Dict[str, str]] = None
    ) -> Dict[str, Any]:
        if env_vars is None:
            env_vars = dict(os.environ)
        
        schema_keys = []
        if isinstance(schema, dict):
            schema_keys = list(schema.keys())
            schema_lua = self._dict_to_lua_table(schema)
            result = self.env_module.validate(schema_lua, self._dict_to_lua_table(env_vars))
        else:
            try:
                with open(schema, 'r', encoding='utf-8') as f:
                    schema_dict = json.load(f)
                schema_keys = list(schema_dict.keys())
                schema_lua = self._dict_to_lua_table(schema_dict)
                result = self.env_module.validate(schema_lua, self._dict_to_lua_table(env_vars))
            except (FileNotFoundError, json.JSONDecodeError) as e:
                schema_path_normalized = schema.replace("\\", "/")
                result = self.env_module.validate(schema_path_normalized, self._dict_to_lua_table(env_vars))
                if isinstance(result, dict) and 'values' in result:
                    schema_keys = list(result.get('values', {}).keys())
        
        result_dict = self._lua_table_to_dict(result)
        
        read_vars = [k for k in schema_keys if k in env_vars]
        validated_vars = list(result_dict.get('values', {}).keys()) if result_dict.get('valid') else []
        missing_vars = [k for k in schema_keys if k not in env_vars]
        errors = result_dict.get('errors', [])
        
        if read_vars:
            self.logger.info(f"Read {len(read_vars)} variables: {', '.join(read_vars)}")
        if validated_vars:
            self.logger.info(f"Validated {len(validated_vars)} variables: {', '.join(validated_vars)}")
        if missing_vars:
            self.logger.warning(f"Missing {len(missing_vars)} variables from schema: {', '.join(missing_vars)}")
        if errors:
            self.logger.error(f"Validation failed with {len(errors)} errors")
            for err in errors:
                if isinstance(err, dict):
                    self.logger.error(f"  {err.get('variable', 'unknown')}: {err.get('error', 'unknown error')}")
        
        return result_dict
    
    def load_schema(self, file_path: str) -> Dict[str, Any]:
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                return json.load(f)
        except (FileNotFoundError, json.JSONDecodeError):
            schema = self.env_module.load_schema(file_path)
            return self._lua_table_to_dict(schema)
    
    def validate_or_error(
        self, 
        schema: Union[str, Dict[str, Any]], 
        env_vars: Optional[Dict[str, str]] = None
    ) -> Dict[str, Any]:
        result = self.validate(schema, env_vars)
        
        if not result['valid']:
            error_messages = []
            for err in result['errors']:
                if isinstance(err, dict):
                    error_messages.append(f"{err.get('variable', 'unknown')}: {err.get('error', 'unknown error')}")
                else:
                    error_messages.append(str(err))
            
            raise ValueError(
                "Environment variable validation failed:\n" + 
                "\n".join(error_messages)
            )
        
        values = result.get('values', {})
        if not isinstance(values, dict):
            values = self._lua_value_to_python(values)
        
        return values
    
    def _dict_to_lua_table(self, d: Dict) -> Any:
        if d is None:
            return None
        
        return self._convert_to_lua_table(d)
    
    def _convert_to_lua_table(self, obj: Any) -> Any:
        if obj is None:
            return None
        elif isinstance(obj, (str, int, float, bool)):
            return obj
        elif isinstance(obj, list):
            lua_list = self.lua.table_from([])
            for i, item in enumerate(obj, 1):
                lua_list[i] = self._convert_to_lua_table(item)
            return lua_list
        elif isinstance(obj, dict):
            lua_table = self.lua.table_from({})
            for key, value in obj.items():
                converted_value = self._convert_to_lua_table(value)
                lua_table[key] = converted_value
            return lua_table
        else:
            return str(obj)
    
    def _lua_table_to_dict(self, lua_table: Any) -> Dict:
        if lua_table is None:
            return {}
        
        return self._lua_value_to_python(lua_table)
    
    def _lua_value_to_python(self, value: Any) -> Any:
        if value is None:
            return None
        
        if isinstance(value, (str, int, float, bool)):
            return value
        
        if hasattr(value, '__iter__'):
            try:
                items = list(value.items())
                if not items:
                    try:
                        length = len(value)
                        return {}
                    except:
                        return {}
                
                keys = [k for k, v in items]
                numeric_keys = [k for k in keys if isinstance(k, int)]
                
                if numeric_keys and len(numeric_keys) == len(keys):
                    sorted_keys = sorted(numeric_keys)
                    min_key = min(sorted_keys)
                    max_key = max(sorted_keys)
                    
                    if min_key == 1 and sorted_keys == list(range(min_key, max_key + 1)):
                        return [self._lua_value_to_python(v) for k, v in sorted(items, key=lambda x: x[0])]
                
                return {str(k): self._lua_value_to_python(v) for k, v in items}
            except (AttributeError, TypeError, ValueError):
                try:
                    items_list = list(value)
                    return [self._lua_value_to_python(v) for v in items_list]
                except:
                    pass
        
        return str(value)


def validate_env(schema: Union[str, Dict[str, Any]], env_vars: Optional[Dict[str, str]] = None) -> Dict[str, Any]:
    validator = EnvValidator()
    return validator.validate(schema, env_vars)
