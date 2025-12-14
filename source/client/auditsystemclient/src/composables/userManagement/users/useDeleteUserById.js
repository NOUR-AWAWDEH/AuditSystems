import { ref } from "vue";
import { deleteUserByIdRepository } from "@/services/userManagement/users/deleteUserByIdRepository";

export function useDeleteUserById() {
  const isDeleting = ref(false);
  const error = ref(null);
  const deletedUser = ref(null);

  const handleDeleteSuccess = (response) => {
    deletedUser.value = true;
    const message = response?.message || "User deleted successfuly";
    return { success: true, message };
  };

  const handleDeleteFailure = (response, defaultErrorMessage = "Failed to delete user") => {
    const errorMessage = response?.errors?.length > 0
      ? response.errors[0]?.message
      : response?.successMessage || defaultErrorMessage;

    error.value = errorMessage;

    const failureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return failureResult;
  };

  const validateDeleteUser = (userId) => {
    if (!userId) {
      return "User ID is required";
    }
    return null;
  };

  const deleteUser = async (userId) => {
    try {
      isDeleting.value = true;
      error.value = null;
      deleteUser.value = null;

      const validationError = validateDeleteUser(userId);
      if (validationError) {
        return handleDeleteFailure({ message: validationError });
      }

      const response = await deleteUserByIdRepository.deleteUserById(userId);

      if (response?.success) {
        return handleDeleteSuccess(response);
      } else {
        return handleDeleteFailure(response);
      }

    } catch (err) {
      return handleDeleteFailure(err, err.message || "Failed to delete user");
    } finally {
      isDeleting.value = false;
    }
  };

  const resetState = () => {
    isDeleting.value = false;
    error.value = null;
    deletedUser.value = null;
  }

  return {
    isDeleting,
    error,
    deletedUser,
    deleteUser,
    resetState
  };
};
