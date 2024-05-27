const express = require('express');
const nodemailer = require('nodemailer');
const swaggerUi = require('swagger-ui-express');
const swaggerJsdoc = require('swagger-jsdoc');
const swaggerDocument = require('./swagger.json');
require('dotenv').config();

const app = express();
const port = 7777;

const startTime = Date.now();

app.use(express.json());

// Configuración de nodemailer
const transporter = nodemailer.createTransport({
    service: 'gmail',
    auth: {
        user: 'juanc.ramosr@uqvirtual.edu.co',
        pass: 'dtxyfeggeewyietl'
    }
});

// Ruta raiz para la redirección a swagger
app.get('/', (req, res) => {
    res.redirect('/api-docs');
});

// Endpoint para enviar correo electrónico
app.post('/send-email', (req, res) => {
    const { to, subject, text } = req.body;

    const mailOptions = {
        from: 'juanc.ramosr@uqvirtual.edu.co',
        to,
        subject,
        text
    };

    transporter.sendMail(mailOptions, (error, info) => {
        if (error) {
            return res.status(500).send(error.toString());
        }
        res.status(200).send('Email enviado: ' + info.response);
    });
});

function istReady() {
    return new Promise((resolve, reject) => {
        transporter.verify((error, success) => {
            if (error) {
                console.error('Error conectando a Gmail:', error);
                resolve('NO-READY');
            } else {
                console.log('Conectado a Gmail:', success);
                resolve('READY');
            }
        });
    });
}

// Endpoint para verificar la salud del servicio
app.get('/health', async (req, res) => {

    const currentTime = Date.now();
    const uptime = currentTime - startTime;
    const uptimeInSeconds = Math.floor(uptime / 1000);

    const body = {
        status: "UP",
        checks: [
            {
                status: "LIVE",
                version: "1.0",
                uptime: `${uptimeInSeconds}`
            },
            {
                status: await istReady(),
                version: "1.0",
                uptime: `${uptimeInSeconds}`
            }
        ]
    };


    res.status(200).json(body);
});

// Endpoint para servir la documentación de Swagger
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerDocument));

app.listen(port, () => {
    console.log(`Servidor corriendo en http://localhost:${port}`);
});