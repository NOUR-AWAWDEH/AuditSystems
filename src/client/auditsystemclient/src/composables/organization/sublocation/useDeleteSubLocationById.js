import { ref } from 'vue';
import { deleteSubLocationByIdRepository } from '@/services/organization/sublocation/deleteSubLocationByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useDeleteSubLocationById() {
  const isDeleting = ref(false);
  const error = ref(null);
  const deletedSubLocation = ref(null);
  const { showNotification } = useNotification();

  const handleDeleteSuccess = (response) => {
    deletedSubLocation.value = true;
    const message = response.message || 'SubLocation deleted successfully!';
    showNotification(message, 'success');
    return { success: true, message };
  };
  
  const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete sublocation') => {
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

  const validateSubLocationId = (subLocationId) => {
    if (!subLocationId) {
      return 'SubLocation ID is required';
    }
    return null;
  };

  const deleteSubLocation = async (subLocationId) => {
    try {
      isDeleting.value = true;
      error.value = null;
      deletedSubLocation.value = null;

      // Validate required fields
      const validationError = validateSubLocationId(subLocationId);
      if (validationError) {
        return handleDeleteFailure({ message: validationError });
      }

      const response = await deleteSubLocationByIdRepository.deleteSubLocationById(subLocationId);

      if (response?.success) {
        return handleDeleteSuccess(response);
      } else {
        return handleDeleteFailure(response);
      }
    } catch (err) {
      return handleDeleteFailure(err, err.message || 'Failed to delete sublocation');
    } finally {
      isDeleting.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    deletedSubLocation.value = null;
    isDeleting.value = false;
  };

  return {
    // State
    isDeleting,
    error,
    deletedSubLocation,

    // Actions
    deleteSubLocation,
    resetState,
  };
}