// src/composables/password/useResetPassword.js
import { ref } from "vue";
import resetPasswordRepository from "@/services/auth/passwordRepos/resetPasswordRepository";
import { validateResetPasswordData } from "@/services/validations/validateResetPasswordData";

export function useResetPassword() {
  const error = ref(null);

  const handleResetPasswordSuccess = (response) => ({
    success: true,
    message: response?.successMessage || "Password reset successfully",
  });

  const handleResetPasswordFailure = (response, defaultErrorMessage = "Password reset failed") => {
    const errorMessage =
      response?.errors?.length > 0
        ? response.errors[0].message
        : response?.message || response?.successMessage || defaultErrorMessage;

    error.value = errorMessage;

    const falureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return falureResult;
  };

  const resetPassword = async (resetPasswordData) => {
    try {
      resetState();

      const validationError = validateResetPasswordData(resetPasswordData);
      if (validationError) {
        return handleResetPasswordFailure({ message: validationError });
      }

      const response = await resetPasswordRepository(resetPasswordData);

      if (response?.success) {
        return handleResetPasswordSuccess(response);
      } else {
        return handleResetPasswordFailure(response);
      }
    } catch (err) {
      console.error("Password reset error:", err);
      return handleResetPasswordFailure(err, err.message || "Password reset failed");
    }
  };

  const resetState = () => {
    error.value = null;
  };

  return {
    error,
    resetPassword,
    resetState,
  };
}
