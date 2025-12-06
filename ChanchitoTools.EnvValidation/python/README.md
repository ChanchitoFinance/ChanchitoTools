# Python Adapter

Python adapter for ChanchitoTools.EnvValidation using lupa Lua runtime.

## Installation

Install required dependencies:

```bash
pip install lupa
```

## Usage

### Basic Example

```python
from adapters.python_adapter import EnvValidator

validator = EnvValidator()

schema = {
    "API_KEY": {"type": "string", "required": True},
    "PORT": {"type": "number", "default": 3000}
}

env_vars = {
    "API_KEY": "secret123",
    "PORT": "8080"
}

result = validator.validate(schema, env_vars)

if result["valid"]:
    print("Validated values:", result["values"])
else:
    print("Validation errors:", result["errors"])
```

### Using Schema File

```python
import os
from adapters.python_adapter import EnvValidator

validator = EnvValidator()
schema_path = os.path.join(os.path.dirname(__file__), "..", "schema.example.json")

result = validator.validate(schema_path)

if result["valid"]:
    print("All environment variables are valid")
else:
    for err in result["errors"]:
        print(f"{err['variable']}: {err['error']}")
```

### Validate or Throw

```python
try:
    values = validator.validate_or_error(schema, env_vars)
    print("Validated values:", values)
except ValueError as error:
    print("Validation failed:", str(error))
```

### Custom Logger

```python
import logging
from adapters.python_adapter import EnvValidator

logger = logging.getLogger("MyLogger")
handler = logging.StreamHandler()
logger.addHandler(handler)
logger.setLevel(logging.INFO)

validator = EnvValidator(logger=logger)
```

### Custom Lua Path

```python
validator = EnvValidator(lua_path="/path/to/lua/directory")
```

## API

### Constructor

`EnvValidator(lua_path=None, logger=None)`

- `lua_path` (optional): Path to directory containing Lua modules. Defaults to auto-detection.
- `logger` (optional): Python logging.Logger instance. Defaults to console logger.

### Methods

#### `validate(schema, env_vars=None)`

Validates environment variables against a schema.

- `schema`: Schema dictionary or path to JSON schema file.
- `env_vars` (optional): Dictionary with environment variables. Defaults to `os.environ`.

Returns: `{"valid": bool, "values": dict, "errors": list}`

#### `validate_or_error(schema, env_vars=None)`

Validates environment variables and raises exception if validation fails.

- `schema`: Schema dictionary or path to JSON schema file.
- `env_vars` (optional): Dictionary with environment variables. Defaults to `os.environ`.

Returns: Dictionary with validated values.

Raises: `ValueError` if validation fails.

#### `load_schema(file_path)`

Loads a schema from a JSON file.

- `file_path`: Path to JSON schema file.

Returns: Schema dictionary.

## Schema Format

See the main README.md for schema format documentation.

