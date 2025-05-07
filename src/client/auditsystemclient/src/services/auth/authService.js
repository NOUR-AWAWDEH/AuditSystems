// src/services/auth/authService.js
import { ApiResponseT } from '@/utils/ApiResponse';
import axios from 'axios';

const api = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const authService = {
  setAuthHeader(token) {
    api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  },

  clearAuthHeader() {
    delete api.defaults.headers.common['Authorization'];
  },

  async login(username, password) {
    try {
      const response = await api.post('/Auth/login', {
        username,
        password,
      });
      console.log("API URL:", api.defaults.baseURL + '/Auth/login');      if (!response.data.success) {
        // Ensure errors is an array, even if backend sends something else
        const errors = Array.isArray(response.data.errors)
          ? response.data.errors
          : [{ message: response.data.message || 'Authentication failed' }];
        return ApiResponseT.badRequest(errors);
      }

      const result = {
        jwtToken: response.data.result.jwtToken,
        refreshToken: response.data.result.refreshToken,
        isEmailVerified: response.data.result.isEmailVerified,
      };

      this.setAuthHeader(result.jwtToken);

      return ApiResponseT.ok(result, response.data.successMessage);
    } catch (error) {
      console.error('Login error:', error);

      let errors = [{ message: 'Authentication failed' }];

      if (error.response?.data?.errors?.length) {
        errors = error.response.data.errors;
      } else if (error.response?.data?.message) {
        errors = [{ message: error.response.data.message }];
      } else if (error.message) {
        errors = [{ message: error.message }];
      }

      return ApiResponseT.badRequest(errors);
    }
  },

  async logout(token) {
    try {
      await api.post('/Auth/logout', null, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return ApiResponseT.ok(null, 'Logout successful');
    } catch (error) {
      console.error('Logout error:', error);
      return ApiResponseT.badRequest([{ message: 'Logout failed' }]);
    }
  },
};
