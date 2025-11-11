import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const LOGOUT_ENDPOINT = 'api/auth/logout';
const AUTHORIZATION_HEADER = 'Authorization';

export const logoutRepository = {
  async logoutRequest(refreshToken) {
    try {
      const response = await api.post(LOGOUT_ENDPOINT, { refreshToken });

      if (response.status === 200 || response?.success !== false) {
        return ApiResponseT.ok(null, response.successMessage || 'Logout successful');
      }

      return ApiResponseT.badRequest(
        response.errors || [{ message: 'Logout failed' }],
        response.successMessage || 'Logout failed'
      );
    } catch (error) {
      console.error('Logout error:', error);
      let errorMessage = 'Logout failed';
      let errors = [{ message: errorMessage }];

      if (error?.response) {
        errorMessage = error.response?.data?.successMessage || 'Logout failed';
        errors = Array.isArray(error.response.errors)
          ? error.response.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      this.clearAuthHeader();
      return ApiResponseT.badRequest(errors, errorMessage);
    }
  },

  clearAuthHeader() {
    delete api.defaults.headers.common[AUTHORIZATION_HEADER];
  }
};
