import { ref } from 'vue';
import { updateLocationRepository } from '@/services/organization/location/updateLocationRepository';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

export function useUpdateLocation() {
  const isUpdating = ref(false);
  const error = ref(null);
  const { authStatus } = useAuthStatus();

  const updateLocation = async (locationData) => {
    if (!authStatus.value?.userId || !authStatus.value?.companyId) {
      const errorMsg = 'User not authenticated or missing company information';
      error.value = errorMsg;
      return { success: false, message: errorMsg };
    }

    isUpdating.value = true;
    error.value = null;

    try {
      // Add user and company IDs to the location data
      const payload = {
        ...locationData,
        userId: authStatus.value.userId,
        companyId: authStatus.value.companyId
      };

      const result = await updateLocationRepository.updateLocation(payload);
      
      if (result.success) {
        return { 
          success: true, 
          message: result.message || 'Location updated successfully',
          data: result.result 
        };
      } else {
        const errorMsg = result.message || 'Failed to update location';
        error.value = errorMsg;
        return { success: false, message: errorMsg, errors: result.errors };
      }
    } catch (err) {
      const errorMsg =  err.message || 'An error occurred while updating location';
      error.value = errorMsg;
      return { success: false, message: errorMsg };
    } finally {
      isUpdating.value = false;
    }
  };

  return {
    isUpdating,
    error,
    updateLocation
  };
}