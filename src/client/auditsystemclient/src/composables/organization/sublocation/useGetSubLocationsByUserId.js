import { ref } from 'vue';
import { getSubLocationsRepository } from '@/services/organization/sublocation/getSubLocationsByUserIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetSubLocationsByUserId() {
  const sublocations = ref([]);
  const isLoading = ref(false);
  const error = ref(null);
  const { showNotification } = useNotification();

  const handleFetchSuccess = (response) => {
    sublocations.value = response.result || [];
    return { success: true, data: response.result };
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
      sublocations.value = [];

      const response = await getSubLocationsRepository.getSubLocationsByUserId();

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

  const addSublocation = (newSublocation) => {
    sublocations.value.push(newSublocation);
  };

  const removeSublocation = (sublocationId) => {
    sublocations.value = sublocations.value.filter(sublocation => sublocation.id !== sublocationId);
  };

  const updateSublocation = (updatedSublocation) => {
    const index = sublocations.value.findIndex(sublocation => sublocation.id === updatedSublocation.id);
    if (index !== -1) {
      sublocations.value[index] = updatedSublocation;
    }
  };

  const getSublocationsByLocationId = (locationId) => {
    return sublocations.value.filter(sublocation => sublocation.locationId === locationId);
  };

  const resetState = () => {
    error.value = null;
    sublocations.value = [];
    isLoading.value = false;
  };

  return {
    // State
    sublocations,
    isLoading,
    error,

    // Actions
    getSubLocationsByUserId,
    addSublocation,
    removeSublocation,
    updateSublocation,
    getSublocationsByLocationId,
    resetState,
  };
}