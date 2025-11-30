import { ref } from "vue";
import { getSubDepartmentByIdRepository } from "@/services/organization/subdepartment/getSubDepartmentByIdRepository";

export function useGetSubDepartmentById() {
  const subDepartment = ref(null);
  const isLoading = ref(false);
  const error = ref(null);

  const handleFetchSuccess = (response) => {
    const subDepartmentData = response.result.subDepartmentModel;
    subDepartment.value = subDepartmentData;
    return { success: true, data: subDepartmentData };
  };

  const handleFetchFailure = (response, defaultErrorMessage = "Failed to fetch subDepartment") => {
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

  const validateSubDepartmentId = (subDepartmentId) => {
    if (!subDepartmentId) {
      return "SubDepartment ID is required";
    }
    return null;
  };

  const getSubDepartmentById = async (subDepartmentId) => {
    try {
      isLoading.value = true;
      error.value = null;
      subDepartment.value = null;

      // Validate required fields
      const validationError = validateSubDepartmentId(subDepartmentId);
      if (validationError) {
        return handleFetchFailure({ message: validationError });
      }

      const response = await getSubDepartmentByIdRepository.getSubDepartmentById(subDepartmentId);

      if (response?.success) {
        return handleFetchSuccess(response);
      } else {
        return handleFetchFailure(response);
      }
    } catch (err) {
      return handleFetchFailure(err, err.message || "Failed to fetch subdepartment");
    } finally {
      isLoading.value = false;
    }
  };

  const clearSubDepartment = () => {
    error.value = null;
    subDepartment.value = null;
    isLoading.value = false;
  };

  return {
    // State
    subDepartment,
    isLoading,
    error,

    // Actions
    getSubDepartmentById,
    clearSubDepartment,
  };
}
