import http from 'k6/http';
import { sleep, check } from 'k6';

export let options = {
  stages: [
    { duration: '10s', target: 10 }, // building to 10 users
    { duration: '20s', target: 20 }, // then to 20 users
    { duration: '10s', target: 0 },  // phase out to 0 users
  ],
  thresholds: {
    http_req_failed: ['rate<0.05'],   // less than 5% errors
    http_req_duration: ['p(95)<800'], // 95% of the requests < 800ms
  },
};

export default function () {
  const baseUrl = __ENV.API_URL;

  const res = http.get(`${baseUrl}/api/Names`);

  check(res, {
    'status is 200': (r) => r.status === 200,
  });

  sleep(1);
}
