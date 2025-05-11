import { ref } from 'vue';
import { forgetPasswordRepository } from '@/services/auth/passwordRepos/forgetPasswordRepository';

export function useForgetPassword() {
  const error = ref(null);
  const successMessage = ref(null);

  const handleForgetPasswordSuccess = (response) => {
    successMessage.value = response?.message || 'Password reset email sent successfully';
    return { success: true };
  };

  const handleForgetPasswordFailure = (response, defaultErrorMessage = 'Forgot password failed') => {
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

  const forgetPassword = async (email) => {
    try {
      error.value = null;
      const response = await forgetPasswordRepository.forgetPasswordRequest(email);

      if (response?.success) {
        return handleForgetPasswordSuccess(response);
      } else {
        return handleForgetPasswordFailure(response);
      }
    } catch (err) {
      console.error('Forgot password error:', err);
      return handleForgetPasswordFailure(err, err.message || 'Forgot password failed');
    }
  };

  return {
    error,
    successMessage,
    forgetPassword,
  };
}
