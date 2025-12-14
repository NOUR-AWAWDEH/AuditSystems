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
    const errorMessage = response?.errors?.length > 0
        ? response.errors[0].message
        : response?.message || defaultErrorMessage;

    error.value = errorMessage;
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const getUsers = async () => {
    isLoading.value = true;
    error.value = null;
    users.value = [];

    try {
      const response = await getUsersByAdminIdRepository.getAllUsers();
      
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

  return {
    isLoading,
    error,
    users,
    getUsers,
  };
};