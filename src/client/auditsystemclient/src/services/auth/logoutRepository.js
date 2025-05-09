import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const LOGOUT_ENDPOINT = 'api/auth/logout';
const AUTHORIZATION_HEADER = 'Authorization';

export const logoutRepository = {
  async logoutRequest(refreshToken) {
    try {
      const response = await api.post(LOGOUT_ENDPOINT, { refreshToken });

      if (!response?.data?.success) {
        const errors = Array.isArray(response?.data?.errors)
          ? response.data.errors
          : [{ message: response?.data?.message || 'Logout failed' }];
        return ApiResponseT.badRequest(errors, response?.data?.message);
      }

      this.clearAuthHeader();
      return ApiResponseT.ok(null, response.data.message || 'Successfully logged out');

    } catch (error) {
      console.error('Logout error:', error);
      let errorMessage = 'Logout failed';
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

      this.clearAuthHeader();
      return ApiResponseT.badRequest(errors, errorMessage);
    }
  },

  clearAuthHeader() {
    delete api.defaults.headers.common[AUTHORIZATION_HEADER];
  }
};
