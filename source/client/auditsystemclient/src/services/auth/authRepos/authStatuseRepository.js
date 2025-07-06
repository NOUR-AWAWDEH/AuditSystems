// src/services/auth/authRepos/authStatuseRepository.js

import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const AUTH_STATUS_ENDPOINT = 'api/Auth/auth-status';

export const authStatuseRepository = {
  async getAuthStatus(token) {
    try {
      const response = await api.post(AUTH_STATUS_ENDPOINT, { token });
      if (response?.data?.isAuthenticated) {
        return ApiResponseT.ok({
          isAuthenticated: response.data.isAuthenticated,
          username: response.data.username,
          email: response.data.email,
          userId: response.data.userId,
          companyId: response.data.companyId,
          roles: response.data.roles,
          expiresAt: response.data.expiresAt,
          isEmailVerified: response.data.isEmailVerified
        }, 'Authenticated');
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
