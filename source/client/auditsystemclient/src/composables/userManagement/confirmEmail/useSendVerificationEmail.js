import { ref } from "vue";
import { sendVerificationEmailRepository } from "@/services/userManagement/confirmEmail/sendVerificationEmailRepository";
import { validateEmail } from "@/services/validations/validateEmail";

export const useVerificationEmail = () => {
  const isLoading = ref(false);
  const error = ref(null);
  const success = ref(null);

  const handleSuccess = (response) => {
    const message = response?.successMessage || "Email verification sent successfully.";
    const result = { success: true, message };
    success.value = result;
    return result;
  };

  const handleFailure = (response, defaultErrorMessage = "Failed to send verification email") => {
    const errorMessage =
      response?.errors?.length > 0
        ? response.errors[0].message
        : response?.message || defaultErrorMessage;

    error.value = errorMessage;

    const failureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return failureResult;
  };

  const sendVerificationEmail = async (email) => {
    resetState();

    try {
      const isValidEmail = validateEmail(email);
      if (!isValidEmail) {
        const validationError = "Invalid email format";
        error.value = validationError;
        return;
      }

      const response = await sendVerificationEmailRepository.sendVerificationEmailRepository(email);

      if (response.success) {
        return handleSuccess(response);
      } else {
        return handleFailure(response);
      }
    } catch (err) {
      console.error("Send verification email error:", err);
      return handleFailure(err, err.message || "Failed to send verification email");
    } finally {
      isLoading.value = false;
    }
  };

  const resetState = () => {
    isLoading.value = true;
    error.value = null;
    success.value = null;
  };

  return {
    isLoading,
    error,
    success,
    sendVerificationEmail,
    resetState,
  };
};
