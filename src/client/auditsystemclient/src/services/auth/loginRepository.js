// src/services/auth/loginRepository.js
import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const AUTH_ENDPOINT = 'api/auth/login';
const AUTHORIZATION_HEADER = 'Authorization';
const BEARER_PREFIX = 'Bearer ';

export const loginRepository = {
  async loginRequest(username, password) {
    try {
      const response = await api.post(AUTH_ENDPOINT, { username, password });

      if (!response?.data?.success) {
        const errors = Array.isArray(response?.data?.errors)
          ? response.data.errors
          : [{ message: response?.data?.message || 'Authentication failed' }];
        return ApiResponseT.badRequest(errors, response?.data?.message);
      }

      const result = {
        jwtToken: response.data.result.jwtToken,
        refreshToken: response.data.result.refreshToken,
        isEmailVerified: response.data.result.isEmailVerified,
      };
      this.setAuthHeader(result.jwtToken);
      return ApiResponseT.ok(result, response.data.message);

    } catch (error) {
      console.error('Login error:', error);
      let errorMessage = 'Authentication failed';
      let errors = [{ message: errorMessage }];

      if (error?.response?.data) {
        errorMessage = error.response.data.message || errorMessage;
        errors = Array.isArray(error.response.data.errors)
          ? error.response.data.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      return ApiResponseT.badRequest(errors, errorMessage);
    }
  },

  setAuthHeader(token) {
    if (token) {
      api.defaults.headers.common[AUTHORIZATION_HEADER] = `${BEARER_PREFIX}${token}`;
    } else {
      delete api.defaults.headers.common[AUTHORIZATION_HEADER];
    }
  },

  clearAuthHeader() {
    delete api.defaults.headers.common[AUTHORIZATION_HEADER];
  },
};
