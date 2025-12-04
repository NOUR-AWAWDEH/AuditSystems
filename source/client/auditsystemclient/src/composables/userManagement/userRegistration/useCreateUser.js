import { ref } from "vue";
import { createUserRepository } from "@/services/userManagement/userRegistration/createUserRepository";

export const useCreateUser = () => {
  const isCreating = ref(false);
  const error = ref(null);
  const createdUserSuccess = ref(null);

  const handleCreateSuccess = (response) => {
    createdUserSuccess.value = response.success;
    return {
      success: true,
      data: response.result,
      message: response.successMessage || "User created successfully.",
    };
  };

  const handleCreateFailure = (response, defaultErrorMessage = "Failed to create user") => {
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

  const createUser = async (userData) => {
    isCreating.value = true;
    error.value = null;
    createdUserSuccess.value = null;

    try {
      const response = await createUserRepository.createUser(userData);
      if (response.success) {
        return handleCreateSuccess(response);
      } else {
        return handleCreateFailure(response);
      }
    } catch (err) {
      return handleCreateFailure(err, err.message || "Failed to create user");
    } finally {
      isCreating.value = false;
    }
  };

  return {
    isCreating,
    error,
    createdUserSuccess,
    createUser,
  };
};
