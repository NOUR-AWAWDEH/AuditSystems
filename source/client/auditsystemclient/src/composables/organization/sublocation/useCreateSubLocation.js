import { ref } from 'vue';
import { createSubLocationRepository } from '@/services/organization/sublocation/createSubLocationRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateSubLocation() {
  const isCreating = ref(false);
  const error = ref(null);
  const createdSubLocation = ref(null);
  const { showNotification } = useNotification();

  const handleCreateSuccess = (response) => {
    createdSubLocation.value = response.result;
    const message = response?.successMessage || 'SubLocation created successfully!';
    showNotification(message, 'success');
    return { success: true, data: response.result };
  };
  
  const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create sublocation') => {
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

  const validateSubLocationData = (subLocationData) => {
    
    if (!subLocationData.name?.trim()) {
      return 'SubLocation name is required';
    }
    if (!subLocationData.locationId) {
      return 'Location ID is required';
    }
    
    return null;
  };

  const createSubLocation = async (subLocationData) => {
    
    try {
        isCreating.value = true;
        error.value = null;
        createdSubLocation.value = null;

        // Validate required fields
        const validationError = validateSubLocationData(subLocationData);
        if (validationError) {
          return handleCreateFailure({ message: validationError });
        }

        const response = await createSubLocationRepository.createSubLocation(subLocationData);
       
        if (response?.success) {
            return handleCreateSuccess(response);
        } else {
            return handleCreateFailure(response);
        }
    } catch (err) {
        return handleCreateFailure(err, err.message || 'Failed to create sublocation');
    } finally {
        isCreating.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    createdSubLocation.value = null;
    isCreating.value = false;
  };

  return {
    // State
    isCreating,
    error,
    createdSubLocation,
    // Actions
    createSubLocation,
    resetState,
  };
}