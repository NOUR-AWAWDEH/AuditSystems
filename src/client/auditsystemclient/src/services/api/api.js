// src/api.js
import axios from 'axios';
import authService from '@/services/auth/authRepos/authService';

const apiUrl =  __VITE_API_URL__;

const api = axios.create({
  baseURL: apiUrl,
  headers: { 'Content-Type': 'application/json' },
});

// Request interceptor to add Authorization header
api.interceptors.request.use(
  (config) => {
    const token = authService.getLocalToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle 401 Unauthorized errors
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    // Check if the error is a 401 and if it's not a retry attempt
    if (error.response && error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true; // Mark this request as a retry

      const refreshSuccessful = await authService.refreshToken();

      if (refreshSuccessful) {
        // Retry the original request with the new token
        originalRequest.headers.Authorization = `Bearer ${authService.getLocalToken()}`;
        return api(originalRequest);
      }
    }

    return Promise.reject(error);
  }
);

export default api;
