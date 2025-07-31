// src/services/auth/authRepos/authStatuseRepository.js

import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const AUTH_STATUS_ENDPOINT = 'api/Auth/auth-status';

export const authStatuseRepository = {
  async getAuthStatus(token) {
    try {      
      const response = await api.post(AUTH_STATUS_ENDPOINT, { token });
      
      // The backend returns the AuthStatusResponseDto directly in response.data
      // NOT nested under response.data.result
      if (response?.data?.isAuthenticated) {
        return ApiResponseT.ok({
          isAuthenticated: response.data.isAuthenticated,
          username: response.data.username,
          email: response.data.email,
          userId: response.data.userId,
          companyId: response.data.companyId,
          roles: response.data.roles,
          expiresAt: response.data.expiresAt,
          isEmailVerified: response.data.isEmailVerified,
          companyName: response.data.companyName,
          companyLogo: response.data.companyLogo
        }, 'Authenticated');
      } else {
        
        return ApiResponseT.badRequest(
          [{ message: 'Not authenticated' }],
          'Not authenticated'
        );
      }
    } catch (error) {
      console.error('🚨 [AUTH STATUS] Auth status error:', error);
      
      let errorMessage = 'Failed to check authentication status';
      let errors = [{ message: errorMessage }];
      
      if (error?.response?.data) {
        // Handle backend error responses
        errorMessage = error.response.data.message || error.response.data.title || errorMessage;
        errors = [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }
      
      return ApiResponseT.badRequest(errors, errorMessage);
    }
  }
};
