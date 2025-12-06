import { ref } from "vue";
import { updateUserRepository } from "@/services/userManagement/users/updateUserRepository";

export const useUpdateUser = () => {
  const isUpdating = ref(false);
  const error = ref(null);
  const updateUserSuccess = ref(null);

  const handleSuccess = (response) => {
    updateUserSuccess.value = true;
    return {
      success: true,
      message: response.successMessage || "User updated successfully.",
    };
  };

  const handleFailure = (response, defaultErrorMessage = "Failed to update user") => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : response?.message || defaultErrorMessage;

    error.value = errorMessage;
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const updateUser = async (userData) => {
    isUpdating.value = true;
    error.value = null;
    updateUserSuccess.value = null;

    try {
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

  return {
    isUpdating,
    error,
    updateUserSuccess,
    updateUser,
  };
};
