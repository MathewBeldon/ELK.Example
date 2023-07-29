import http from 'k6/http';
import exec from 'k6/execution';

export const options = {    
    vus: 1,
    duration: '600s',
};

var attempt = 0;
export default function () {
    const res = http.get('http://localhost:4000/' + attempt++);
    if (res.status == 200){
        exec.test.abort('value found');
    }
};