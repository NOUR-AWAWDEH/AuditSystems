import { ref } from 'vue';
import { createSubDepartmentRepository } from '@/services/organization/subdepartment/createSubDepartmentRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateSubDepartment() {
  const isCreating = ref(false);
  const error = ref(null);
  const createdSubDepartment = ref(null);
  const { showNotification } = useNotification();

  const handleCreateSuccess = (response) => {
    createdSubDepartment.value = response.result;
    const message = response?.successMessage || 'SubDepartment created successfully!';
    showNotification(message, 'success');
    return { success: true, data: response.result };
  };
  
  const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create subdepartment') => {
    
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : response?.message || defaultErrorMessage;
    
    error.value = errorMessage;
    showNotification(errorMessage, 'error');
    
    const failureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
    
    return failureResult;
  };

  const validateSubDepartmentData = (subDepartmentData) => {
    
    if (!subDepartmentData.name?.trim()) {
      return 'SubDepartment name is required';
    }
    if (!subDepartmentData.departmentId) {
      return 'Department ID is required';
    }
    
    return null;
  };

  const createSubDepartment = async (subDepartmentData) => {
    
    try {
        isCreating.value = true;
        error.value = null;
        createdSubDepartment.value = null;

        // Validate required fields
        const validationError = validateSubDepartmentData(subDepartmentData);
        if (validationError) {
          return handleCreateFailure({ message: validationError });
        }

        const response = await createSubDepartmentRepository.createSubDepartment(subDepartmentData);

        if (response?.success) {
            return handleCreateSuccess(response);
        } else {
            return handleCreateFailure(response);
        }
    } catch (err) {
        return handleCreateFailure(err, err.message || 'Failed to create subdepartment');
    } finally {
        isCreating.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    createdSubDepartment.value = null;
    isCreating.value = false;
  };

  return {
    // State
    isCreating,
    error,
    createdSubDepartment,

    // Actions
    createSubDepartment,
    resetState,
  };
}