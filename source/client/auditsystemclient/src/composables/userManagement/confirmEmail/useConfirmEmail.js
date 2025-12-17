import { ref } from "vue";
import { confirmEmailRepository } from "@/services/userManagement/confirmEmail/confirmEmailRepository";

export const useConfirmEmail = () => {
  const isLoading = ref(false);
  const error = ref(null);
  const success = ref(null);

  const validateConfirmEmail = (uid, token) => {
    if (!uid || !token) {
      return "User ID and token are required";
    }
    return null;
  };

  const handleSuccess = (response) => {
    const message = response?.successMessage || "Email confirmed successfully.";
    const result = { success: true, message };
    success.value = result;
    return result;
  };

  const handleFailure = (response, defaultErrorMessage = "Failed to confirm email") => {
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

  const confirmEmail = async (uid, token) => {
    isLoading.value = true;
    error.value = null;
    success.value = null;

    try {
      const validationError = validateConfirmEmail(uid, token);
      if (validationError) {
        error.value = validationError;
        return;
      }

      const response = await confirmEmailRepository.confirmEmailRepository(uid, token);
      console.log(response);
      if (response.success) {
        return handleSuccess(response);
      } else {
        return handleFailure(response);
      }

    } catch (err) {
      console.error("Confirm email error:", err);
      return handleFailure(err, err.message || "Failed to confirm email");
    } finally {
      isLoading.value = false;
    }
  };

  return {
    isLoading,
    error,
    success,
    confirmEmail,
  };
};