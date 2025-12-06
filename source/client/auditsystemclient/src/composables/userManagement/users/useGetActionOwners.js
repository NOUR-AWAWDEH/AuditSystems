import { ref } from "vue";
import { getActionOwnersRepository } from "@/services/userManagement/users/getActionOwnersRepository";

export const useGetActionOwners = () => {
  const isLoading = ref(false);
  const error = ref(null);
  const actionOwners = ref([]);

  const handleSuccess = (response) => {
    actionOwners.value = response.result;
    return {
      success: true,
      data: response.result,
      message: response.successMessage || "Action owners retrieved successfully.",
    };
  };

  const handleFailure = (response, defaultErrorMessage = "Failed to retrieve action owners") => {
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

  const fetchActionOwners = async () => {
    isLoading.value = true;
    error.value = null;
    actionOwners.value = [];

    try {
      const response = await getActionOwnersRepository.getActionOwners();
      if (response.success) {
        return handleSuccess(response);
      } else {
        return handleFailure(response);
      }
    } catch (err) {
      return handleFailure(err, err.message || "Failed to retrieve action owners");
    } finally {
      isLoading.value = false;
    }
  };

  return {
    isLoading,
    error,
    actionOwners,
    fetchActionOwners,
  };
};
