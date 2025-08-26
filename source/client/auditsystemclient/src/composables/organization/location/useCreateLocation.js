import { ref } from 'vue';
import { createLocationRepository } from '@/services/organization/location/createLocationRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateLocation() {
  const isCreating = ref(false);
  const error = ref(null);
  const createdLocation = ref(null);
  const { showNotification } = useNotification();

  const handleCreateSuccess = (response) => {

    createdLocation.value = response;

    showNotification(response?.successMessage || 'Location created successfully!', 'success');

    return { success: true, data: response };
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
    return null;
  };

  const createLocation = async (locationData) => {
    try {
      isCreating.value = true;
      error.value = null;
      createdLocation.value = null;

      // Validate input data
      const validationError = validateLocationData(locationData);
      if (validationError) {
        return handleCreateFailure({ message: validationError });
      }

      const result = await createLocationRepository.createLocation(locationData);

      if (result.success) {
        return handleCreateSuccess(result);
      } else {
        return handleCreateFailure(result);
      }
    } catch (err) {
      return handleCreateFailure(err, err.message || 'Create failed');
    } finally {
      isCreating.value = false;
    }
  };

  const clearState = () => {
    error.value = null;
    createdLocation.value = null;
  };

  return {
    isCreating,
    error,
    createdLocation,
    createLocation,
    clearState
  };
}
