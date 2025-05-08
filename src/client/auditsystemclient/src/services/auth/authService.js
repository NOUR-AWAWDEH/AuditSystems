// src/services/auth/authService.js
import { ApiResponseT } from '@/utils/ApiResponse';
import api from '@/services/api/api';

export const authService = {
  async login(username, password) {
    try {
      const response = await api.post('api/auth/login', {
        username,
        password,
      });

      if (!response.data.success) {
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

      // Store token in localStorage
      localStorage.setItem('token', result.jwtToken);

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

  setAuthHeader(token) {
    if (token) {
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    }
  },

  clearAuthHeader() {
    delete api.defaults.headers.common['Authorization'];
  },

  async logout() {
    try {
      await api.post('api/Auth/logout');
      localStorage.removeItem('token');
      return ApiResponseT.ok(null, 'Logout successful');
    } catch (error) {
      console.error('Logout error:', error);
      return ApiResponseT.badRequest([{ message: 'Logout failed' }]);
    }
  },
};
