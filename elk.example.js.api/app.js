const express = require('express');
const winston = require('winston');
const ecsFormat = require('@elastic/ecs-winston-format')
const LogstashTransport = require('./logstashTransport');

const app = express();
const port = 4000;
const secretValue = Math.floor(Math.random() * 10000);

const logger = winston.createLogger({
    level: 'info',
    format: ecsFormat(),
    transports: [
        new LogstashTransport({
            logstashUrl: 'http://localhost:8080',  // Replace with your Logstash host and port
        }),
    ],
});

app.get('/:value', (req, res) => {
    const value = parseInt(req.params.value, 10);
    if (value === secretValue) {
        logger.info('You have found the secret value', { id: value, request_id: req.id, request_path: req.path });
        res.status(200).send('You have found the secret value');
    } else {
        logger.warn('You have not found the secret value', { id: value, request_id: req.id, request_path: req.path });
        res.status(400).send('You have not found the secret value');
    }
});

app.listen(port, () => {
    console.log(`Server running at http://localhost:${port}/`);
});