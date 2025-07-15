import { ref } from 'vue';
import { getSubLocationByIdRepository } from '@/services/organization/sublocation/getSubLocationByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetSubLocationById() {
  const sublocation = ref(null);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const handleFetchSuccess = (response) => {
    sublocation.value = response.result;
    return { success: true, data: response.result };
  };
  
  const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch sublocation') => {
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

  const fetchSubLocationById = async (subLocationId) => {
    try {
      isLoading.value = true;
      error.value = null;
      sublocation.value = null;

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
    sublocation.value = null;
    isLoading.value = false;
  };

  return {
    // State
    sublocation,
    isLoading,
    error,

    // Actions
    fetchSubLocationById,
    clearSubLocation,
  };
}