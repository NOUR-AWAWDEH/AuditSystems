import { ref } from "vue";
import { getUserByIdRepository } from "@/services/userManagement/users/getUserByIdRepository";

export const useGetUserById = () => {
  const isLoading = ref(false);
  const error = ref(null);
  const user = ref(null);

  const handleSuccess = (response) => {
    user.value = response.result;
    const message = response?.successMessage || "User retrieved successfully.";
    return { success: true, data: response.result, message };
  };

  const handleFailure = (response, defaultErrorMessage = "Failed to retrieve user") => {
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

  const validateGetUser = (userId) => {
    if (!userId) {
      return "User ID is required";
    }
    return null;
  };

  const getUserById = async (userId) => {
    try {
      isLoading.value = true;
      error.value = null;
      user.value = null;

      const validationError = validateGetUser(userId);
      if (validationError) {
        return handleFailure({ message: validationError });
      }

      const response = await getUserByIdRepository.getUserById(userId);
      
      if (response.success) {
        return handleSuccess(response);
      } else {
        return handleFailure(response);
      }

    } catch (err) {
      return handleFailure(err, err.message || "Failed to retrieve user");
    } finally {
      isLoading.value = false;
    }
  };

  const resetState = () => {
    isLoading.value = false;
    error.value = null;
    user.value = null;
  }

  return {
    isLoading,
    error,
    user,
    getUserById,
    resetState,
  };
};
