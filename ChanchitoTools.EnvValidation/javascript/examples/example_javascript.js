const path = require('path');
const fs = require('fs');

let EnvValidator;
const adapter = require('../adapters/javascript_adapter');
EnvValidator = adapter.EnvValidator;

async function main() {
    const validator = new EnvValidator();
    
    const schemaPath = path.join(__dirname, '..', '..', 'schema.example.json');
    
    if (fs.existsSync(schemaPath)) {
        const envVars = {
            DATABASE_URL: 'https://db.example.com',
            API_PORT: '8080',
            NODE_ENV: 'production',
            LOG_LEVEL: 'info',
            ENABLE_CACHE: 'true',
            MAX_CONNECTIONS: '50',
            ADMIN_EMAIL: 'admin@example.com',
            API_KEY: 'ABCD1234EFGH5678IJKL9012MNOP3456'
        };
        
        let result;
        if (validator.validate.constructor.name === 'AsyncFunction') {
            result = await validator.validate(schemaPath, envVars);
        } else {
            result = validator.validate(schemaPath, envVars);
        }
    }
    
    const schemaDict = {
        API_KEY: {
            type: 'string',
            required: true,
            min_length: 32,
            pattern: '^[A-Za-z0-9]+$',
            description: 'Secret API key'
        },
        DEBUG: {
            type: 'boolean',
            required: false,
            default: false,
            description: 'Enable debug mode'
        },
        PORT: {
            type: 'integer',
            required: false,
            default: 3000,
            min: 1,
            max: 65535,
            description: 'Server port'
        },
        ENVIRONMENT: {
            type: 'string',
            required: true,
            enum: ['development', 'staging', 'production'],
            description: 'Execution environment'
        }
    };
    
    const envVars2 = {
        API_KEY: 'ABCD1234EFGH5678IJKL9012MNOP3456',
        DEBUG: 'true',
        PORT: '8080',
        ENVIRONMENT: 'production'
    };
    
    let result2;
    if (validator.validate.constructor.name === 'AsyncFunction') {
        result2 = await validator.validate(schemaDict, envVars2);
    } else {
        result2 = validator.validate(schemaDict, envVars2);
    }
    
    try {
        let values;
        if (validator.validateOrError.constructor.name === 'AsyncFunction') {
            values = await validator.validateOrError(schemaDict, envVars2);
        } else {
            values = validator.validateOrError(schemaDict, envVars2);
        }
    } catch (error) {
    }
    
    const invalidEnvVars = {
        API_KEY: 'short',
        PORT: '99999',
        ENVIRONMENT: 'invalid'
    };
    
    let result3;
    if (validator.validate.constructor.name === 'AsyncFunction') {
        result3 = await validator.validate(schemaDict, invalidEnvVars);
    } else {
        result3 = validator.validate(schemaDict, invalidEnvVars);
    }
}

main().catch(error => {
    console.error('Error:', error.message);
    console.error(error.stack);
    process.exit(1);
});
