import { ref } from "vue";
import { updateUserRepository } from "@/services/userManagement/users/updateUserRepository";
import { validateUpdateUserData } from "@/services/validations/validateUpdateUserData";

export const useUpdateUser = () => {
  const isUpdating = ref(false);
  const error = ref(null);
  const updatedUser = ref(null);

  const handleSuccess = (response) => {
    updatedUser.value = true;
    const message = response.message || "User updated successfully.";
    const data = response.result;

    return { success: true, data ,message};
  };

  const handleFailure = (response, defaultErrorMessage = "Failed to update user") => {
    const errorMessage = response?.errors?.length > 0
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

  const updateUser = async (userData) => {
    try {
      isUpdating.value = true;
      error.value = null;
      updatedUser.value = null;

      // Validate input data
      const validationError = validateUpdateUserData(userData);
      if (validationError) {
        return handleFailure({ message: validationError });
      }

      const response = await updateUserRepository.updateUser(userData);

      if (response.success) {
        return handleSuccess(response);
      } else {
        return handleFailure(response);
      }

    } catch (err) {
      return handleFailure(err, err.message || "Failed to update user");
    } finally {
      isUpdating.value = false;
    }
  };

  const clearState = () => {
    isUpdating.value = false;
    error.value = null;
    updatedUser.value = null;
  };

  return {
    isUpdating,
    error,
    updatedUser,
    updateUser,
    clearState
  };
};
