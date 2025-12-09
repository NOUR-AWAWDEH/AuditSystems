import http from "k6/http";
import { check, sleep } from "k6";

export const options = {
  stages: [
    { duration: "30s", target: 50 }, // ramp-up
    { duration: "1m", target: 200 }, // peak load
    { duration: "30s", target: 0 }, // ramp-down
  ],
};

export default function () {
  const res = http.get("https://localhost:7218/api/Test/db-load-test", {
    tlsAuth: [{ cert: "", key: "" }], // optional if HTTPS
  });

  check(res, {
    "status 200": (r) => r.status === 200,
  });

  sleep(0.1);
}
