// src/services/auth/authRepos/authStatuseRepository.js

import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const AUTH_STATUS_ENDPOINT = 'api/auth/auth-status';

export const authStatuseRepository = {
  async getAuthStatus(token) {
    try {
      const response = await api.post(AUTH_STATUS_ENDPOINT, { token });

      if (response?.data?.isAuthenticated) {
        // The backend returns the AuthStatusResponseDto directly
        return ApiResponseT.ok(response.data, 'Authenticated');
      } else {
        return ApiResponseT.badRequest(
          [{ message: response?.data?.message || 'Not authenticated' }],
          response?.data?.message || 'Not authenticated'
        );
      }
    } catch (error) {
      console.error('Auth status error:', error);
      let errorMessage = 'Failed to check authentication status';
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
  }
};
