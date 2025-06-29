import { ref } from 'vue';
import { updateLocationRepository } from '@/services/organization/location/updateLocationRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useUpdateLocation() {
  const isUpdating = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const updateLocation = async (locationData) => {
    if (!locationData.id) {
      const errorMsg = 'Location ID is required';
      error.value = errorMsg;
      showNotification(errorMsg, 'error');
      return { success: false, message: errorMsg };
    }

    isUpdating.value = true;
    error.value = null;

    try {
      // Note: userId is automatically injected due to UserAware pattern
      const result = await updateLocationRepository.updateLocation(locationData);
      
      if (result.success) {
        showNotification(result.message || 'Location updated successfully', 'success');
        return { 
          success: true, 
          message: result.message || 'Location updated successfully',
          data: result.result 
        };
      } else {
        const errorMsg = result.message || 'Failed to update location';
        error.value = errorMsg;
        showNotification(errorMsg, 'error');
        return { success: false, message: errorMsg, errors: result.errors };
      }
    } catch (err) {
      const errorMsg = err.message || 'An error occurred while updating location';
      error.value = errorMsg;
      showNotification(errorMsg, 'error');
      return { success: false, message: errorMsg };
    } finally {
      isUpdating.value = false;
    }
  };

  const clearState = () => {
    error.value = null;
  };

  return {
    isUpdating,
    error,
    updateLocation,
    clearState
  };
}