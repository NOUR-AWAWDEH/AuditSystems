// verifyResetCodeRepository.js
import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

const AUTH_ENDPOINT = '/api/password/verify-reset-code';

const verifyResetCode = async (email, code) => {
  try {
    const response = await api.post(AUTH_ENDPOINT, { email, code });

    if (!response?.success) {
      const errors = Array.isArray(response?.errors)
        ? response.errors
        : [{ message: response?.message || 'Verification failed' }];
      return ApiResponseT.badRequest(errors, response?.message);
    }

    const result = response.result; // Return the `result` object
    return ApiResponseT.ok(result, response.successMessage);
  } catch (error) {
    console.error('verifyResetCode error:', error);
    console.error('Error response:', error.response?.data);
    console.error('Error status:', error.response?.status);
    let errorMessage = 'Verification failed';
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

export default verifyResetCode;
