import { ref } from "vue";
import { createUserRepository } from "@/services/userManagement/users/createUserRepository";
import { validateUserData } from "@/services/validations/userValidators";

export const useCreateUser = () => {
  const isCreating = ref(false);
  const error = ref(null);
  const createdUser = ref(null);


  const handleCreateSuccess = (response) => {
    createdUser.value = response;
    const message = response.message || "User created successfully.";
    const data = response.result;
    console.log(response);
    
    return { success: true, data, message };
  };

  const handleCreateFailure = (response, defaultErrorMessage = "Failed to create user") => {
    const errorMessage = response?.errors?.length
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

  const createUser = async (userData) => {
    try {
      isCreating.value = true;
      error.value = null;
      createdUser.value = null;

      // Validate input data
      const validationError = validateUserData(userData);
      if (validationError) {
        return handleCreateFailure({ message: validationError });
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