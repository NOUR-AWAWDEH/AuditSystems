import { ref } from "vue";
import { getUserByIdRepository } from "@/services/userManagement/users/getUserByIdRepository";

export const useGetUserById = () => {
  const isLoading = ref(false);
  const error = ref(null);
  const user = ref(null);

  const handleSuccess = (response) => {
    user.value = response.result;
    return {
      success: true,
      data: response.result,
      message: response.successMessage || "User retrieved successfully.",
    };
  };

  const handleFailure = (response, defaultErrorMessage = "Failed to retrieve user") => {
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

  const fetchUserById = async (userId) => {
    isLoading.value = true;
    error.value = null;
    user.value = null;

    try {
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

  return {
    isLoading,
    error,
    user,
    fetchUserById,
  };
};
