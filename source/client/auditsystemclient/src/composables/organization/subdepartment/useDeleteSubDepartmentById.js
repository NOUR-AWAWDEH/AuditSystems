import { ref } from 'vue';
import { deleteSubDepartmentByIdRepository } from '@/services/organization/subdepartment/deleteSubDepartmentByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useDeleteSubDepartmentById() {
  const isDeleting = ref(false);
  const error = ref(null);
  const deletedSubDepartment = ref(null);
  const { showNotification } = useNotification();

  const handleDeleteSuccess = (response) => {
    deletedSubDepartment.value = true;
    const message = response?.successMessage || 'SubDepartment deleted successfully!';
    showNotification(message, 'success');
    return { success: true, message };
  };
  
  const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete subdepartment') => {
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

  const validateSubDepartmentId = (subDepartmentId) => {
    if (!subDepartmentId) {
      return 'SubDepartment ID is required';
    }
    return null;
  };

  const deleteSubDepartment = async (subDepartmentId) => {
    try {
      isDeleting.value = true;
      error.value = null;
      deletedSubDepartment.value = null;

      // Validate required fields
      const validationError = validateSubDepartmentId(subDepartmentId);
      if (validationError) {
        return handleDeleteFailure({ message: validationError });
      }

      const response = await deleteSubDepartmentByIdRepository.deleteSubDepartmentById(subDepartmentId);

      if (response?.success) {
        return handleDeleteSuccess(response);
      } else {
        return handleDeleteFailure(response);
      }
    } catch (err) {
      return handleDeleteFailure(err, err.message || 'Failed to delete subdepartment');
    } finally {
      isDeleting.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    deletedSubDepartment.value = null;
    isDeleting.value = false;
  };

  return {
    // State
    isDeleting,
    error,
    deletedSubDepartment,

    // Actions
    deleteSubDepartment,
    resetState,
  };
}