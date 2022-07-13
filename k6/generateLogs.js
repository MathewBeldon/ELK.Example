import http from 'k6/http';
import { check } from 'k6';
import { randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.1.0/index.js';

export const options = {    
    vus: 10,
    duration: '600s',
};

export default function () {
    http.get('http://localhost:3000/' + randomIntBetween(1, 1000000));
};