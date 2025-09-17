import { ref } from 'vue';
import { getSubLocationsByUserIdRepository } from '@/services/organization/sublocation/getSubLocationsByUserIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetSubLocationsByUserId() {
  const subLocations = ref([]);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const handleFetchSuccess = (response) => {
    // Fix: Extract subLocations array from the nested result object
    const subLocationData = response.result || [];
    subLocations.value = subLocationData;
    return { success: true, data: subLocationData };
  };
  
  const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch sublocations') => {
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

  const getSubLocationsByUserId = async () => {
    try {
      isLoading.value = true;
      error.value = null;
      subLocations.value = [];

      const response = await getSubLocationsByUserIdRepository.getSubLocationsByUserId();

      if (response?.success) {
        return handleFetchSuccess(response);
      } else {
        return handleFetchFailure(response);
      }
    } catch (err) {
      return handleFetchFailure(err, err.message || 'Failed to fetch sublocations');
    } finally {
      isLoading.value = false;
    }
  };

  return {
    // State
    subLocations,
    isLoading,
    error,

    // Actions
    getSubLocationsByUserId,
  };
}