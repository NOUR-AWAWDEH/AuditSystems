import { ref } from 'vue';
import { getLocationByIdRepository } from '@/services/organization/location/getLocationByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetLocationById() {
  const location = ref(null);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const fetchLocationById = async (locationId) => {
    if (!locationId) {
      const errorMsg = 'Location ID is required';
      error.value = errorMsg;
      showNotification(errorMsg, 'error');
      return { success: false, message: errorMsg };
    }

    isLoading.value = true;
    error.value = null;
    location.value = null;

    try {
      const result = await getLocationByIdRepository.getLocationById(locationId);
      
      if (result.success) {
        location.value = result.result;
        return { 
          success: true, 
          message: result.message || 'Location fetched successfully',
          data: result.result 
        };
      } else {
        const errorMsg = result.message || 'Failed to fetch location';
        error.value = errorMsg;
        showNotification(errorMsg, 'error');
        return { success: false, message: errorMsg, errors: result.errors };
      }
    } catch (err) {
      const errorMsg = err.message || 'An error occurred while fetching location';
      error.value = errorMsg;
      showNotification(errorMsg, 'error');
      return { success: false, message: errorMsg };
    } finally {
      isLoading.value = false;
    }
  };

  const clearLocation = () => {
    location.value = null;
    error.value = null;
  };

  return {
    location,
    isLoading,
    error,
    fetchLocationById,
    clearLocation
  };
}