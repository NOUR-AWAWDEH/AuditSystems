import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";

const AUTH_ENDPOINT = '/api/Password/reset-password';

const resetPassword = async (email, token, newPassword, confirmNewPassword) => {
  try {
    const response = await api.post(AUTH_ENDPOINT, { email, token, newPassword, confirmNewPassword });

    if (!response?.data?.success) {
      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
        : [{ message: response?.data?.message || 'Password reset failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.message);
    }

    return ApiResponseT.ok(null, response.data.successMessage);

  } catch (error) {
    console.error('Password reset error:', error);
    let errorMessage = 'Password reset failed';
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
};

export default resetPassword;
