// src/services/auth/passwordRepos/forgetPasswordRepository.js
import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const FORGOT_PASSWORD_ENDPOINT = 'api/Password/forgot-password';

export const forgetPasswordRepository = {
  async forgetPasswordRequest(email) {
    try {
      const response = await api.post(FORGOT_PASSWORD_ENDPOINT, { email });

      if (!response?.data?.success) {
        const errors = Array.isArray(response?.data?.errors)
          ? response.data.errors
          : [{ message: response?.data?.message || 'Forgot password failed' }];
        return ApiResponseT.badRequest(errors, response?.data?.message);
      }

      return ApiResponseT.ok(null, response.data.message);

    } catch (error) {
      console.error('Forgot password error:', error);
      let errorMessage = 'Forgot password failed';
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
};
