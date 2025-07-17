import { ref } from 'vue';
import { getSubDepartmentsByUserIdRepository } from '@/services/organization/subdepartment/getSubDepartmentsByUserIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetSubDepartmentsByUserId() {
  const subdepartments = ref([]);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const handleFetchSuccess = (response) => {
    // Fix: Extract subDepartments array from the nested result object
    const subdepartmentData = response.result?.subDepartments || [];
    subdepartments.value = subdepartmentData;
    return { success: true, data: subdepartmentData };
  };
  
  const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch subdepartments') => {
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

  const getSubDepartmentsByUserId = async () => {
    try {
      isLoading.value = true;
      error.value = null;
      subdepartments.value = [];

      const response = await getSubDepartmentsByUserIdRepository.getSubDepartmentsByUserId();

      if (response?.success) {
        return handleFetchSuccess(response);
      } else {
        return handleFetchFailure(response);
      }
    } catch (err) {
      return handleFetchFailure(err, err.message || 'Failed to fetch subdepartments');
    } finally {
      isLoading.value = false;
    }
  };

  return {
    // State
    subdepartments,
    isLoading,
    error,

    // Actions
    getSubDepartmentsByUserId,
  };
}