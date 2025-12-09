import { ref } from "vue";
import { createUserRepository } from "@/services/userManagement/users/createUserRepository";
import { userValidators } from "@/services/validations/userValidators";

export const useCreateUser = () => {
  const isCreating = ref(false);
  const error = ref(null);
  const createdUser = ref(null);


  const handleCreateSuccess = (response) => {
    createdUser.value = response.success;
    return {
      success: true,
      data: response
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
    createdUser.value = null;

    try {

      const validationError = userValidators.validateUserData(userData);
      if (validationError !== null) {
        return handleCreateFailure(null, validationError);
      }

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

  const clearState = () => {
    isCreating.value = false;
    error.value = null;
    createdUser.value = null;
  };

  return {
    isCreating,
    error,
    createdUser,
    createUser,
    clearState,
  };
};
