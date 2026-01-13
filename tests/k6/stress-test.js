import http from 'k6/http';
import { sleep, check } from 'k6';

export let options = {
  stages: [
    { duration: '20s', target: 25 },
    { duration: '20s', target: 50 },
    { duration: '20s', target: 100 },
    { duration: '30s', target: 100 },
    { duration: '20s', target: 0 },
  ],
  thresholds: {
    http_req_failed: ['rate<0.05'], 
    http_req_duration: ['p(95)<800'],
  },
  setupTimeout: '5m',
  summaryTrendStats: ['avg','p(90)','p(95)','max','min'],
};

export default function () {
  const baseUrl = __ENV.API_URL;

  const res = http.get(`${baseUrl}/api/Names`, {
    headers: {
      "Accept": "text/plain"
    }
  });

  check(res, {
    'status is 200': (r) => r.status === 200,
  });

  sleep(1);
}


