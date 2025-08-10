import { ref } from 'vue';
import { getSubDepartmentByIdRepository } from '@/services/organization/subdepartment/getSubDepartmentByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetSubDepartmentById() {
  const subdepartment = ref(null);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const handleFetchSuccess = (response) => {
    // Handle potential nested structure - extract the actual subdepartment data
    const subdepartmentData = response.result?.subDepartmentModel || response.result;
    subdepartment.value = subdepartmentData;
    return { success: true, data: subdepartmentData };
  };
  
  const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch subdepartment') => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : response?.message || defaultErrorMessage;
    
    error.value = errorMessage;
    showNotification(errorMessage, 'error');
    
    return {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
  };

  const validateSubDepartmentId = (subDepartmentId) => {
    if (!subDepartmentId) {
      return 'SubDepartment ID is required';
    }
    return null;
  };

  const getSubDepartmentById = async (subDepartmentId) => {
    try {
      isLoading.value = true;
      error.value = null;
      subdepartment.value = null;

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
      return handleFetchFailure(err, err.message || 'Failed to fetch subdepartment');
    } finally {
      isLoading.value = false;
    }
  };

  const clearSubDepartment = () => {
    error.value = null;
    subdepartment.value = null;
    isLoading.value = false;
  };

  return {
    // State
    subdepartment,
    isLoading,
    error,

    // Actions
    getSubDepartmentById,
    clearSubDepartment,
  };
}