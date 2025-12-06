import { ref } from "vue";
import { deleteUserRepository } from "@/services/userManagement/users/deleteUserRepository";

export const useDeleteUser = () => {
  const isDeleting = ref(false);
  const error = ref(null);
  const deleteUserSuccess = ref(null);

  const handleDeleteSuccess = (response) => {
    deleteUserSuccess.value = true;
    return {
      success: true,
      message: response.successMessage || "User deleted successfully.",
    };
  };

  const handleDeleteFailure = (response, defaultErrorMessage = "Failed to delete user") => {
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

  const deleteUser = async (userId) => {
    isDeleting.value = true;
    error.value = null;
    deleteUserSuccess.value = null;

    try {
      const response = await deleteUserRepository.deleteUser(userId);
      if (response.success) {
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

  return {
    isDeleting,
    error,
    deleteUserSuccess,
    deleteUser,
  };
};
