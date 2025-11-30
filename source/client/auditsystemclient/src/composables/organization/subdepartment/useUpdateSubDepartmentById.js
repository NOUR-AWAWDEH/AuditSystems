import { ref } from 'vue';
import { updateSubDepartmentByIdRepository } from '@/services/organization/subdepartment/updateSubDepartmentByIdRepository';

export function useUpdateSubDepartmentById() {
  const isUpdating = ref(false);
  const error = ref(null);
  const updatedSubDepartment = ref(null);

  const handleUpdateSuccess = (response) => {
    updatedSubDepartment.value = response.result;
    const message = response?.successMessage || 'SubDepartment updated successfully!';
    return { success: true, data: response.result };
  };
  
  const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update subdepartment') => {
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

  const validateSubDepartmentData = (subDepartmentData) => {
    if (!subDepartmentData.id) {
      return 'SubDepartment ID is required';
    }
    if (!subDepartmentData.name?.trim()) {
      return 'SubDepartment name is required';
    }
    if (!subDepartmentData.departmentId) {
      return 'Department ID is required';
    }
    return null;
  };

  const updateSubDepartment = async (subDepartmentData) => {
    try {
      
      isUpdating.value = true;
      error.value = null;
      updatedSubDepartment.value = null;
  
      // Validate required fields
      const validationError = validateSubDepartmentData(subDepartmentData);
      if (validationError) {
        return handleUpdateFailure({ message: validationError });
      }
  
      const response = await updateSubDepartmentByIdRepository.updateSubDepartmentById(subDepartmentData);
  
      if (response?.success) {
        return handleUpdateSuccess(response);
      } else {
        return handleUpdateFailure(response);
      }
    } catch (err) {
      return handleUpdateFailure(err, err.message || 'Failed to update subdepartment');
    } finally {
      isUpdating.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    updatedSubDepartment.value = null;
    isUpdating.value = false;
  };

  return {
    // State
    isUpdating,
    error,
    updatedSubDepartment,

    // Actions
    updateSubDepartment,
    resetState,
  };
}