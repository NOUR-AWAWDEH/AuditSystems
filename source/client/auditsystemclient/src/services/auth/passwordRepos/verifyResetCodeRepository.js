// verifyResetCodeRepository.js
import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const AUTH_ENDPOINT = '/api/password/verify-reset-code';

const verifyResetCode = async (email, code) => {
  try {
    const response = await api.post(AUTH_ENDPOINT, { email, code });

    if (!response?.data?.success) {
      const errors = Array.isArray(response?.data?.errors)
        ? response.data.errors
        : [{ message: response?.data?.message || 'Verification failed' }];
      return ApiResponseT.badRequest(errors, response?.data?.message);
    }

    const result = response.data.result; // Return the `result` object
    return ApiResponseT.ok(result, response.data.successMessage);
  } catch (error) {
    console.error('verifyResetCode error:', error);
    console.error('Error response:', error.response?.data);
    console.error('Error status:', error.response?.status);
    let errorMessage = 'Verification failed';
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

export default verifyResetCode;
