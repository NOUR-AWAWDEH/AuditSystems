import { ref } from "vue";
import { getUsersByAdminIdRepository } from "@/services/userManagement/users/getUsersByAdminIdRepository";

export const useGetUsersByAdminId = () => {
  const isLoading = ref(false);
  const error = ref(null);
  const users = ref([]);

  const handleSuccess = (response) => {
    users.value = response.result;
    const message = response.successMessage || "Users retrieved successfully.";

    return { success: true, data: response.result, message };
  };

  const handleFailure = (response, defaultErrorMessage = "Failed to retrieve users") => {
    const errorMessage =
      response?.errors?.length > 0
        ? response.errors[0].message
        : response?.message || defaultErrorMessage;

    error.value = errorMessage;
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const getUsersByAdminId = async () => {
    isLoading.value = true;
    error.value = null;
    users.value = [];

    try {
      const response = await getUsersByAdminIdRepository.getUsersByAdminId();

      if (response.success) {
        return handleSuccess(response);
      } else {
        return handleFailure(response);
      }
    } catch (err) {
      return handleFailure(err, err.message || "Failed to retrieve users");
    } finally {
      isLoading.value = false;
    }
  };

  const addUser = (newUser) => {
    users.value.push(newUser);
  };

  const removeUser = (userId) => {
    users.value = users.value.filter((user) => user.id !== userId);
  };

  const updateUser = (updatedUser) => {
    const index = users.value.findIndex((user) => user.id === updatedUser.id);
    if (index !== -1) {
      users.value[index] = updatedUser;
    }
  };

  return {
    isLoading,
    error,
    users,
    getUsersByAdminId,
    addUser,
    removeUser,
    updateUser,
  };
};
