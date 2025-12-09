import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
  stages: [
    { duration: '15s', target: 50 },   // 50 users
    { duration: '15s', target: 150 },  // 150 users
    { duration: '15s', target: 300 },  // 300 users
    { duration: '15s', target: 600 },  // 600 users
    { duration: '15s', target: 1000 }, // 1000 users (stress)
  ],
};

export default function () {
  const url = 'https://localhost:7218/api/Auth/login';
  
  const payload = JSON.stringify({
    username: "nour",
    password: "Havana123%100"
  });

  const headers = {
    'Content-Type': 'application/json',
  };

  const res = http.post(url, payload, { headers });

  check(res, {
    'status is 200': (r) => r.status === 200,
  });

  sleep(1);
}
