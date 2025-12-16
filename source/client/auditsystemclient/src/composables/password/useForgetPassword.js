import { ref } from "vue";
import { forgetPasswordRepository } from "@/services/auth/passwordRepos/forgetPasswordRepository";
import { validateEmail } from "@/services/validations/validateEmail";

export function useForgetPassword() {
  const error = ref(null);

  const handleForgetPasswordSuccess = (response) => ({
    success: true,
    message: response?.message || "Password reset email sent successfully",
  });

  const handleForgetPasswordFailure = (
    response,
    defaultErrorMessage = "Forgot password failed"
  ) => {
    const errorMessage =
      response?.errors?.length > 0
        ? response.errors[0].message
        : response?.message || defaultErrorMessage;

    error.value = errorMessage;

    const falureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return falureResult;
  };

  const forgetPassword = async (email) => {
    try {
      resetState();

      const isValid = validateEmail(email);
      if (!isValid) {
        return handleForgetPasswordFailure({ message: "Invalid email format" });
      }

      const response = await forgetPasswordRepository.forgetPasswordRequest(email);

      if (response?.success) {
        return handleForgetPasswordSuccess(response);
      }

      return handleForgetPasswordFailure(response);
    } catch (err) {
      console.error("Forgot password error:", err);
      return handleForgetPasswordFailure(err, err?.message || "Forgot password failed");
    }
  };

  const resetState = () => {
    error.value = null;
  };

  return {
    error,
    forgetPassword,
    resetState,
  };
}
