// src/composables/password/useResetPassword.js
import { ref } from 'vue';
import resetPasswordRepository from '@/services/auth/passwordRepos/resetPasswordRepository';

export function useResetPassword() {
  const error = ref(null);
  const successMessage = ref(null);

  const resetPassword = async (email, token, newPassword, confirmNewPassword) => {
    try {
      error.value = null;
      const response = await resetPasswordRepository(email, token, newPassword, confirmNewPassword);

      if (response?.success) {
        successMessage.value = response.message;
        return { success: true };
      } else {
        error.value = response?.errors?.[0]?.message || response.message || 'Password reset failed';
        return { success: false, message: error.value, errors: response?.errors || [{ message: error.value }] };
      }
    } catch (err) {
      console.error('Password reset error:', err);
      error.value = err.message || 'Password reset failed';
      return { success: false, message: error.value, errors: [{ message: error.value }] };
    }
  };

  return {
    error,
    successMessage,
    resetPassword,
  };
}
