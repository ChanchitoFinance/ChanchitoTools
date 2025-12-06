# EnvValidation - Portable Environment Variable Validation Library

A lightweight and portable Lua library for environment variable validation using JOI-like schemas. Schemas are defined in JSON, making them completely portable to any programming language.

![EnvValidation Example](./Documentation/image.png)

![EnvValidation Example 2](./Documentation/image%20copy.png)

## Features

- **Portable**: Schemas are defined in JSON, can be used in any language
- **JOI-like**: Familiar syntax similar to JOI for validation
- **Multiple types**: string, number, integer, boolean, url, email, json
- **Advanced validations**: enum, min/max (independent), min_length/max_length, pattern (regex)
- **Default values**: Support for default values
- **Optional fields**: Non-required variables with conditional validation
- **Clear error messages**: Descriptive errors for easy debugging

## Installation

### Lua (Core Library)

Copy the files from the `lua/` directory (`env_validation.lua`, `validators.lua` and `schema_loader.lua`) to your project.

### Optional Dependencies

To parse JSON files, use one of these libraries:

- **dkjson** (recommended): `luarocks install dkjson`
- **cjson**: `luarocks install lua-cjson`
- **json.lua**: Pure Lua implementation without C dependencies

If no JSON library is installed, the library includes a basic parser for simple schemas. For complex schemas, use an external library.

## Quick Usage

### Example 1: Validation from JSON file

```lua
local env = require("env_validation")
local schema = env.load_schema("schema.json")
local result = env.validate(schema)

if result.valid then
    for key, value in pairs(result.values) do
        print(key .. " = " .. tostring(value))
    end
else
    for _, err in ipairs(result.errors) do
        print(err.variable .. ": " .. err.error)
    end
end
```

### Example 2: Validation from Lua schema

```lua
local env = require("env_validation")

local schema = {
    DATABASE_URL = { type = "url", required = true },
    PORT = { type = "integer", default = 3000, min = 1, max = 65535 },
    DEBUG = { type = "boolean", default = false }
}

local result = env.validate(schema, {
    DATABASE_URL = "https://db.example.com",
    PORT = "8080",
    DEBUG = "true"
})
```

## Supported Validation Types

- **string**: Validates string values with optional min_length, max_length, and pattern
- **number**: Validates numeric values (decimals allowed) with optional min/max
- **integer**: Validates integer values with optional min/max
- **boolean**: Accepts true/false, 1/0, yes/no, on/off
- **url**: Validates URL format
- **email**: Validates email format
- **json**: Validates JSON string format

## Validation Options

- **required**: Whether variable is required (default: true)
- **default**: Default value if variable is not present
- **enum**: List of allowed values
- **min / max**: Minimum and maximum values for numbers (independent - can use only min, only max, or both)
- **min_length / max_length**: Minimum and maximum length for strings
- **pattern**: Regular expression for string validation
- **description**: Optional documentation (does not affect validation)

## Language Adapters

This library includes adapters for Python and JavaScript that use the core Lua validation logic, allowing you to use the same validation schemas across different languages.

### Python Adapter

The Python adapter uses `lupa` to execute the Lua validation code. See `python/README.md` for detailed documentation.

**Installation:**
```bash
pip install lupa
```

**Quick Example:**
```python
from python.adapters.python_adapter import EnvValidator

validator = EnvValidator()

schema = {
    "API_KEY": {"type": "string", "required": True},
    "PORT": {"type": "number", "default": 3000}
}

result = validator.validate(schema)
if result["valid"]:
    print("Validated values:", result["values"])
```

See `python/examples/example_python.py` for complete examples.

### JavaScript Adapter

The JavaScript adapter uses `fengari` to execute the Lua validation code. See `javascript/README.md` for detailed documentation.

**Installation:**
```bash
cd javascript
npm install
```

**Quick Example:**
```javascript
const { EnvValidator } = require('./adapters/javascript_adapter.js');

const validator = new EnvValidator();

const schema = {
    API_KEY: { type: 'string', required: true },
    PORT: { type: 'number', default: 3000 }
};

const result = validator.validate(schema);
if (result.valid) {
    console.log('Validated values:', result.values);
}
```

See `javascript/examples/example_javascript.js` for complete examples.

## Project Structure

```
ChanchitoTools.EnvValidation/
├── lua/                           # Core Lua validation library
│   ├── env_validation.lua         # Main module
│   ├── validators.lua             # Validators by type
│   └── schema_loader.lua          # JSON schema loader
├── python/                        # Python adapter
│   ├── adapters/
│   │   └── python_adapter.py      # Python adapter using lupa
│   ├── examples/
│   │   └── example_python.py      # Python usage examples
│   └── README.md                  # Python adapter documentation
├── javascript/                    # JavaScript adapter
│   ├── adapters/
│   │   └── javascript_adapter.js  # JavaScript adapter using fengari
│   ├── examples/
│   │   └── example_javascript.js  # JavaScript usage examples
│   ├── package.json               # Node.js dependencies
│   └── README.md                  # JavaScript adapter documentation
├── Documentation/                 # Documentation images
│   ├── image.png
│   └── image copy.png
├── schema.example.json            # Portable schema example
├── env.example                     # Example environment file
├── README.md                      # This documentation
└── LICENSE                        # MPL-2.0 License
```

## Portability

JSON schemas can be used across different languages. This library provides:

- **Lua**: Core validation library in `lua/` directory
- **Python**: Adapter using `lupa` to execute Lua code (see `python/` directory)
- **JavaScript**: Adapter using `fengari` to execute Lua code (see `javascript/` directory)

The same `schema.example.json` file works with all adapters, ensuring consistent validation across your stack.

## API Reference

### `env.validate(schema, env_vars?)`

Validates environment variables according to a schema.

**Parameters:**
- `schema` (table|string): Validation schema (Lua table or path to JSON file)
- `env_vars` (table, optional): Table of environment variables. If not provided, reads from `os.getenv()`

**Returns:**
```lua
{
    valid = true/false,
    values = { VAR_NAME = validated_value, ... },  -- Only if valid == true
    errors = { {variable = "VAR_NAME", error = "error message"}, ... }  -- Only if valid == false
}
```

### `env.load_schema(file_path)`

Loads a schema from a JSON file.

**Parameters:**
- `file_path` (string): Path to the JSON schema file

**Returns:** Lua table with the loaded schema

### `env.create_schema(schema_table)`

Validates and returns a schema from a Lua table.

**Parameters:**
- `schema_table` (table): Lua table with schema definition

**Returns:** Validated schema

### `env.validate_or_error(schema, env_vars?)`

Validates and returns only valid values, throwing error if validation fails.

**Parameters:**
- `schema` (table|string): Validation schema
- `env_vars` (table, optional): Table of environment variables

**Returns:** Table with validated values

**Throws:** Error if validation fails

## Complete Examples

See the example files for detailed usage:

- **Lua**: Use the core library directly from the `lua/` directory
- **Python**: See `python/examples/example_python.py` for Python adapter examples
- **JavaScript**: See `javascript/examples/example_javascript.js` for JavaScript adapter examples

All examples use the same `schema.example.json` file, demonstrating true portability across languages.

## Contributing

Contributions are welcome. Please:

1. Maintain JSON schema portability
2. Add tests for new features
3. Document changes in README
4. Follow existing code style

## License

This project is licensed under the Mozilla Public License 2.0 (MPL-2.0). See the LICENSE file for more details.

## Notes

- Environment variable values are always strings. The library converts them automatically according to the specified type.
- For complex JSON schemas, use an external JSON library (dkjson, cjson, etc.).
- Regex patterns use Lua syntax (not PCRE).
