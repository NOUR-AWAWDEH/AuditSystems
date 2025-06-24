import { ref } from 'vue';
import { createLocationRepository } from '@/services/organization/location/createLocationRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateLocation() {
  const isCreating = ref(false);
  const error = ref(null);
  const createdLocation = ref(null);
  const { showNotification } = useNotification();

  const handleCreateSuccess = (response) => {
    createdLocation.value = response.result;
    showNotification(response.message || 'Location created successfully!', 'success');
    return { success: true, data: response.result };
  };
  
  const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create location') => {
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

  const validateLocationData = (locationData) => {
    if (!locationData.name?.trim()) {
      return 'Location name is required';
    }
    if (!locationData.companyId) {
      return 'Company ID is required';
    }
    if (!locationData.userId) {
      return 'User ID is required';
    }
    return null;
  };

  const createLocation = async (locationData) => {
    try {
        isCreating.value = true;
        error.value = null;
        createdLocation.value = null;

        // Validate required fields
        const validationError = validateLocationData(locationData);
        if (validationError) {
          return handleCreateFailure({ message: validationError });
        }

        const response = await createLocationRepository.createLocation(locationData);

        if (response?.success) {
            return handleCreateSuccess(response);
        } else {
            return handleCreateFailure(response);
        }
    } catch (err) {
        console.error('Create location error:', err);
        return handleCreateFailure(err, err.message || 'Failed to create location');
    } finally {
        isCreating.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    createdLocation.value = null;
    isCreating.value = false;
  };

  return {
    // State
    isCreating,
    error,
    createdLocation,

    // Actions
    createLocation,
    resetState,
  };
}