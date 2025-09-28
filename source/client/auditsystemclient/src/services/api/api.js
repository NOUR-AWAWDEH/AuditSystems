// api.js
import axios from 'axios';
import authService from '@/services/auth/authRepos/authService';
import router from '@/router/router';

const apiUrl = __VITE_API_URL__;

const api = axios.create({
  baseURL: apiUrl,
  headers: { 'Content-Type': 'application/json' },
});

api.interceptors.request.use((config) => {
  const token = authService.getLocalToken();
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
      const ok = await authService.refreshToken();
      if (ok) {
        originalRequest.headers.Authorization = `Bearer ${authService.getLocalToken()}`;
        return api(originalRequest);
      }
      authService.removeLocalTokens();
      router.push('/login');
    }
    return Promise.reject(error);
  }
);

export default api;