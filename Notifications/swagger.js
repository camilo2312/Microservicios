const swaggerJsdoc = require('swagger-jsdoc');

const options = {
    definition: {
        openapi: '3.0.0',
        info: {
            title: 'Email API',
            version: '1.0.0',
            description: 'API para enviar correos electrónicos',
        },
        servers: [
            {
                url: 'http://localhost:3000',
            },
        ],
    },
    apis: ['./index.js'], // Archivos con anotaciones Swagger
};

const specs = swaggerJsdoc(options);

module.exports = specs;