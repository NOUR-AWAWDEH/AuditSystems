// src/services/auth/passwordRepos/forgetPasswordRepository.js
import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const FORGOT_PASSWORD_ENDPOINT = 'api/password/forgot-password';

export const forgetPasswordRepository = {
  async forgetPasswordRequest(email) {
    try {
      const response = await api.post(FORGOT_PASSWORD_ENDPOINT, { email });

      if (response.status === 200 || response?.success !== false) {
        return ApiResponseT.ok(response.result, response.successMessage || 'Reset code sent successfully');
      }

      return ApiResponseT.badRequest(
        response.errors || [{ message: 'Failed to send reset code' }],
        response.successMessage || 'Failed to send reset code'
      );
    } catch (error) {
      console.error('Forgot password error:', error);
      let errorMessage = 'Forgot password failed';
      let errors = [{ message: errorMessage }];

      if (error?.response) {
        errorMessage = error.response?.data?.successMessage || 'Failed to send reset code';
        errors = Array.isArray(error.response.errors)
          ? error.response.errors
          : [{ message: errorMessage }];
      } else if (error?.message) {
        errorMessage = error.message;
        errors = [{ message: errorMessage }];
      }

      return ApiResponseT.badRequest(errors, errorMessage);
    }
  },
};
