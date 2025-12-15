// src/composables/password/useResetPassword.js
import { ref } from "vue";
import verifyResetCodeRepository from "@/services/auth/passwordRepos/verifyResetCodeRepository";

export function useResetPassword() {
  const error = ref(null);
  const successMessage = ref(null);
  const isVerified = ref(false);

  const verifyToken = async (email, code) => {
    try {
      error.value = null;
      const response = await verifyResetCodeRepository(email, code);

      if (response?.success) {
        isVerified.value = true;
        return { success: true };
      } else {
        isVerified.value = false;
        error.value =
          response?.errors?.[0]?.message ||
          response?.successMessage ||
          "Invalid verification code.";
        return { success: false, message: error.value };
      }
    } catch (err) {
      console.error("Token verification error:", err);
      isVerified.value = false;
      error.value = err.message || "Token verification failed.";
      return { success: false, message: error.value };
    }
  };

  const resetPassword = async (email, token, newPassword, confirmNewPassword) => {
    try {
      error.value = null;
      const response = await resetPasswordRepository(email, token, newPassword, confirmNewPassword);

      if (response?.success) {
        successMessage.value = response?.successMessage || "Password reset successfully";
        return { success: true };
      } else {
        error.value =
          response?.errors?.[0]?.message || response?.successMessage || "Password reset failed";
        return {
          success: false,
          message: error.value,
          errors: response?.errors || [{ message: error.value }],
        };
      }
    } catch (err) {
      console.error("Password reset error:", err);
      error.value = err.message || "Password reset failed";
      return { success: false, message: error.value, errors: [{ message: error.value }] };
    }
  };

  return {
    error,
    successMessage,
    isVerified,
    verifyToken,
    resetPassword,
  };
}
