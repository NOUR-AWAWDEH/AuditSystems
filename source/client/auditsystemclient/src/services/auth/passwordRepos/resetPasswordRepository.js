import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

const AUTH_ENDPOINT = '/api/password/reset-password';

const resetPassword = async (email, token, newPassword, confirmNewPassword) => {
  try {
    const response = await api.post(AUTH_ENDPOINT, { email, token, newPassword, confirmNewPassword });

    if (!response?.success) {
      const errors = Array.isArray(response?.errors)
        ? response.errors
        : [{ message: response?.message || 'Password reset failed' }];
      return ApiResponseT.badRequest(errors, response?.message);
    }

    return ApiResponseT.ok(null, response.successMessage);

  } catch (error) {
    console.error('Password reset error:', error);
    let errorMessage = 'Password reset failed';
    let errors = [{ message: errorMessage }];

    if (error?.response) {
      errorMessage = error.response.successMessage || errorMessage;
      errors = Array.isArray(error.response.errors)
        ? error.response.errors
        : [{ message: errorMessage }];
    } else if (error?.message) {
      errorMessage = error.message;
      errors = [{ message: errorMessage }];
    }

    return ApiResponseT.badRequest(errors, errorMessage);
  }
};

export default resetPassword;
