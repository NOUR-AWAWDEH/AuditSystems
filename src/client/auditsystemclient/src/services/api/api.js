import axios from 'axios';

const apiUrl = __VITE_API_URL__;

// Create an axios instance with configuration
const api = axios.create({
  baseURL: apiUrl,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;
