// useVerifyResetCode.js
import { ref } from 'vue';
import verifyResetCode from '@/services/auth/passwordRepos/verifyResetCodeRepository';

export function useVerifyResetCode() {
  const isVerified = ref(false);
  const error = ref(null);

  const handleVerificationSuccess = (response) => {
    isVerified.value = true;
    return {
      success: true,
      data: response.result,
      message: response.successMessage || response.result.successMessage,
    };
  };

  const handleVerificationFailure = (response, defaultErrorMessage = 'Verification failed') => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : defaultErrorMessage;
    error.value = errorMessage;
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const verifyCode = async (email, code) => {
    try {
      error.value = null;
      const response = await verifyResetCode(email, code);

      if (response?.success) {
        return handleVerificationSuccess(response);
      } else {
        return handleVerificationFailure(response);
      }
    } catch (err) {
      console.error('useVerifyResetCode: Verification error:', err);
      return handleVerificationFailure(err, err.message || 'Verification failed');
    }
  };

  return {
    isVerified,
    error,
    verifyCode,
  };
}
