const Transport = require('winston-transport');
const axios = require('axios');

class LogstashTransport extends Transport {
    constructor(opts) {
        super(opts);
        this.logstashUrl = opts.logstashUrl;
    }

    log(info, callback) {
        setImmediate(() => {
            this.emit('logged', info);
        });
    
        const logData = {
            ...info,
            metadata: { api_name: 'js-api' },
        };
    
        axios.post(this.logstashUrl, logData)
            .then(() => callback())
            .catch(err => {
                console.error('Error sending log to Logstash:', err);
                callback(err);
            });
    }
}

module.exports = LogstashTransport;