# JavaScript Adapter

JavaScript adapter for ChanchitoTools.EnvValidation using fengari Lua runtime.

## Installation

Install required dependencies in the `javascript` directory:

```bash
cd javascript
npm install
```

Or install fengari directly:

```bash
cd javascript
npm install fengari
```

## Usage

### Basic Example

```javascript
const { EnvValidator } = require('./adapters/javascript_adapter.js');

const validator = new EnvValidator();

const schema = {
    API_KEY: { type: 'string', required: true },
    PORT: { type: 'number', default: 3000 }
};

const envVars = {
    API_KEY: 'secret123',
    PORT: '8080'
};

const result = validator.validate(schema, envVars);

if (result.valid) {
    console.log('Validated values:', result.values);
} else {
    console.error('Validation errors:', result.errors);
}
```

### Using Schema File

```javascript
const path = require('path');
const { EnvValidator } = require('./adapters/javascript_adapter.js');

const validator = new EnvValidator();
const schemaPath = path.join(__dirname, '..', 'schema.example.json');

const result = validator.validate(schemaPath);

if (result.valid) {
    console.log('All environment variables are valid');
} else {
    result.errors.forEach(err => {
        console.error(`${err.variable}: ${err.error}`);
    });
}
```

### Validate or Throw

```javascript
try {
    const values = validator.validateOrError(schema, envVars);
    console.log('Validated values:', values);
} catch (error) {
    console.error('Validation failed:', error.message);
}
```

### Custom Logger

```javascript
const logger = {
    info: (msg) => console.log(`[INFO] ${msg}`),
    warning: (msg) => console.warn(`[WARN] ${msg}`),
    error: (msg) => console.error(`[ERROR] ${msg}`)
};

const validator = new EnvValidator(null, logger);
```

### Custom Lua Path

```javascript
const validator = new EnvValidator('/path/to/lua/directory');
```

## API

### Constructor

`new EnvValidator(luaPath?, logger?)`

- `luaPath` (optional): Path to directory containing Lua modules. Defaults to auto-detection.
- `logger` (optional): Custom logger object with `info`, `warning`, and `error` methods.

### Methods

#### `validate(schema, envVars?)`

Validates environment variables against a schema.

- `schema`: Schema object or path to JSON schema file.
- `envVars` (optional): Object with environment variables. Defaults to `process.env`.

Returns: `{ valid: boolean, values: object, errors: array }`

#### `validateOrError(schema, envVars?)`

Validates environment variables and throws if validation fails.

- `schema`: Schema object or path to JSON schema file.
- `envVars` (optional): Object with environment variables. Defaults to `process.env`.

Returns: Object with validated values.

Throws: Error if validation fails.

#### `loadSchema(filePath)`

Loads a schema from a JSON file.

- `filePath`: Path to JSON schema file.

Returns: Schema object.

## Schema Format

See the main README.md for schema format documentation.

