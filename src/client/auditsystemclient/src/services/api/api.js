import axios from 'axios';

// Use the globally defined environment variable from Vite config
const apiUrl = __VITE_API_URL__;

// Create an axios instance with configuration
const api = axios.create({
  baseURL: apiUrl,
  headers: {
    'Content-Type': 'application/json',
  },
});
console.log("API URL:", apiUrl);
export default api;
