import { ref } from 'vue';
import { deleteLocationRepository } from '@/services/organization/location/deleteLocationByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useDeleteLocationById() {

  const isDeleting = ref(false);
  const error = ref(null);
  const deleteStatus = ref(null);
  const { showNotification } = useNotification();

  const handleDeleteSuccess = (locationName) => {
    deleteStatus.value = { success: true, message: 'Location deleted successfully' };
    showNotification(`Location "${locationName}" has been deleted successfully!`, 'success');
    return { success: true, message: 'Location deleted successfully' };
  };

  const handleDeleteFailure = (response, defaultErrorMessage = 'Delete failed') => {
    const errorMessage = response?.errors?.length
      ? response.errors[0].message
      : response?.message || defaultErrorMessage;
    
    error.value = errorMessage;
    deleteStatus.value = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };
    
    showNotification(errorMessage, 'error');
    return { success: false, message: errorMessage };
  };

  const deleteLocation = async (locationId, locationName = '') => {
    if (!locationId) {
      const errorMsg = 'Location ID is required';
      error.value = errorMsg;
      showNotification(errorMsg, 'error');
      return { success: false, message: errorMsg };
    }

    isDeleting.value = true;
    error.value = null;
    deleteStatus.value = null;

    try {
      const result = await deleteLocationRepository.deleteLocation(locationId);
      
      if (result.success) {
        return handleDeleteSuccess(locationName);
      } else {
        return handleDeleteFailure(result);
      }
    } catch (err) {
      return handleDeleteFailure(err);
    } finally {
      isDeleting.value = false;
    }
  };

  const resetDeleteState = () => {
    error.value = null;
    deleteStatus.value = null;
  };

  return {
    isDeleting,
    error,
    deleteStatus,
    deleteLocation,
    resetDeleteState
  };
}