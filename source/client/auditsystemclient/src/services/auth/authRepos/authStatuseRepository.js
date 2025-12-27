// src/services/auth/authRepos/authStatuseRepository.js

import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const AUTH_STATUS_ENDPOINT = '/api/Auth/auth-status';

export const authStatuseRepository = {
  async getAuthStatus(token) {
    try {
      const { data } = await api.post(AUTH_STATUS_ENDPOINT, { token });

      // Backend ALWAYS returns AuthStatusResponseDto
      // even when not authenticated
      if (!data || typeof data.isAuthenticated !== 'boolean') {
        console.error('Invalid auth status payload from server:', data);
        return ApiResponseT.badRequest(
          [{ message: 'Invalid auth status response' }],
          'Invalid response'
        );
      }

      if (!data.isAuthenticated) {
        return ApiResponseT.ok(
          { isAuthenticated: false },
          'Not authenticated'
        );
      }

      return ApiResponseT.ok(
        {
          isAuthenticated: data.isAuthenticated,
          userId: data.userId,
          username: data.username,
          email: data.email,
          companyId: data.companyId,
          roles: data.roles
            ? data.roles.split(',').map(r => r.trim())
            : [],
          authenticationType: data.authenticationType,
          isEmailVerified: data.isEmailVerified,
          expiresAt: data.expiresAt,
          authenticatedAt: data.authenticatedAt,
          ipAddress: data.ipAddress
        },
        'Authenticated'
      );
    } catch (error) {
      console.error('Error calling auth-status endpoint:', error?.response?.data || error);
      const message =
        error?.response?.data?.message ||
        error?.response?.data?.title ||
        error?.message ||
        'Failed to check authentication status';

      return ApiResponseT.badRequest(
        [{ message }],
        message
      );
    }
  }
};