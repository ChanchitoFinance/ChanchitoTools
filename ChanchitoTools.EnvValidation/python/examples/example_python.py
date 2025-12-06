import os
import sys

sys.path.insert(0, os.path.join(os.path.dirname(__file__), '..', 'adapters'))

try:
    from python_adapter import EnvValidator
except ImportError:
    from python_subprocess_adapter import EnvValidator

def main():
    validator = EnvValidator()
    
    schema_path = os.path.join(os.path.dirname(__file__), "..", "..", "schema.example.json")
    
    if os.path.exists(schema_path):
        env_vars = {
            "DATABASE_URL": "https://db.example.com",
            "API_PORT": "8080",
            "NODE_ENV": "production",
            "LOG_LEVEL": "info",
            "ENABLE_CACHE": "true",
            "MAX_CONNECTIONS": "50",
            "ADMIN_EMAIL": "admin@example.com",
            "API_KEY": "ABCD1234EFGH5678IJKL9012MNOP3456"
        }
        
        result = validator.validate(schema_path, env_vars)
    
    schema_dict = {
        "API_KEY": {
            "type": "string",
            "required": True,
            "min_length": 32,
            "pattern": "^[A-Za-z0-9]+$",
            "description": "Secret API key"
        },
        "DEBUG": {
            "type": "boolean",
            "required": False,
            "default": False,
            "description": "Enable debug mode"
        },
        "PORT": {
            "type": "integer",
            "required": False,
            "default": 3000,
            "min": 1,
            "max": 65535,
            "description": "Server port"
        },
        "ENVIRONMENT": {
            "type": "string",
            "required": True,
            "enum": ["development", "staging", "production"],
            "description": "Execution environment"
        }
    }
    
    env_vars2 = {
        "API_KEY": "ABCD1234EFGH5678IJKL9012MNOP3456",
        "DEBUG": "true",
        "PORT": "8080",
        "ENVIRONMENT": "production"
    }
    
    result2 = validator.validate(schema_dict, env_vars2)
    
    try:
        values = validator.validate_or_error(schema_dict, env_vars2)
    except ValueError as e:
        pass
    
    invalid_env_vars = {
        "API_KEY": "short",
        "PORT": "99999",
        "ENVIRONMENT": "invalid"
    }
    
    result3 = validator.validate(schema_dict, invalid_env_vars)

if __name__ == "__main__":
    main()
