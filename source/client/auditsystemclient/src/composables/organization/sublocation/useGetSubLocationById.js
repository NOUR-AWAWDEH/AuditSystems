import { ref } from 'vue';
import { getSubLocationByIdRepository } from '@/services/organization/sublocation/getSubLocationByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetSubLocationById() {
  const subLocation = ref(null);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const handleFetchSuccess = (response) => {
    // Handle potential nested structure - extract the actual subLocation data
    const subLocationData = response.result?.subLocationModel || response.result;
    subLocation.value = subLocationData;
    return { success: true, data: subLocationData };
  };
  
  const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch subLocation') => {
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

  const validateSubLocationId = (subLocationId) => {
    if (!subLocationId) {
      return 'SubLocation ID is required';
    }
    return null;
  };

  const getSubLocationById = async (subLocationId) => {
    try {
      isLoading.value = true;
      error.value = null;
      subLocation.value = null;

      // Validate required fields
      const validationError = validateSubLocationId(subLocationId);
      if (validationError) {
        return handleFetchFailure({ message: validationError });
      }

      const response = await getSubLocationByIdRepository.getSubLocationById(subLocationId);

      if (response?.success) {
        return handleFetchSuccess(response);
      } else {
        return handleFetchFailure(response);
      }
    } catch (err) {
      return handleFetchFailure(err, err.message || 'Failed to fetch sublocation');
    } finally {
      isLoading.value = false;
    }
  };

  const clearSubLocation = () => {
    error.value = null;
    subLocation.value = null;
    isLoading.value = false;
  };

  return {
    // State
    subLocation,
    isLoading,
    error,

    // Actions
    getSubLocationById,
    clearSubLocation,
  };
}