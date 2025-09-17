import { ref } from 'vue';
import { updateSubLocationByIdRepository } from '@/services/organization/sublocation/updateSubLocationByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useUpdateSubLocationById() {
  const isUpdating = ref(false);
  const error = ref(null);
  const updatedSubLocation = ref(null);
  const { showNotification } = useNotification();

  const handleUpdateSuccess = (response) => {
    updatedSubLocation.value = response.result;
    const message = response?.successMessage || 'SubLocation updated successfully!';
    showNotification(message, 'success');
    return { success: true, data: response.result };
  };
  
  const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update sublocation') => {
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

  const validateSubLocationData = (subLocationData) => {
    if (!subLocationData.id) {
      return 'SubLocation ID is required';
    }
    if (!subLocationData.name?.trim()) {
      return 'SubLocation name is required';
    }
    if (!subLocationData.locationId) {
      return 'Location ID is required';
    }
    return null;
  };

  const updateSubLocation = async (subLocationData) => {
    try {
      
      isUpdating.value = true;
      error.value = null;
      updatedSubLocation.value = null;
  
      // Validate required fields
      const validationError = validateSubLocationData(subLocationData);
      if (validationError) {
        showNotification(validationError, 'error');
        return handleUpdateFailure({ message: validationError });
      }
  
      const response = await updateSubLocationByIdRepository.updateSubLocationById(subLocationData);
  
      if (response?.success) {
        return handleUpdateSuccess(response);
      } else {
        return handleUpdateFailure(response);
      }
    } catch (err) {
      return handleUpdateFailure(err, err.message || 'Failed to update sublocation');
    } finally {
      isUpdating.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    updatedSubLocation.value = null;
    isUpdating.value = false;
  };

  return {
    // State
    isUpdating,
    error,
    updatedSubLocation,

    // Actions
    updateSubLocation,
    resetState,
  };
}