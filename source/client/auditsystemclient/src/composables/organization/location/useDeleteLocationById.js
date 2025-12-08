import { ref } from "vue";
import { deleteLocationByIdRepository } from "@/services/organization/location/deleteLocationByIdRepository";

export function useDeleteLocationById() {
  const isDeleting = ref(false);
  const error = ref(null);
  const deletedLocation = ref(null);

  const handleDeleteSuccess = (response) => {
    deletedLocation.value = true;
    const message = response?.message || "Location deleted successfully";
    return { success: true, message };
  };

  const handleDeleteFailure = (response, defaultErrorMessage = "Failed to delete location") => {
    const errorMessage =
      response?.errors?.length > 0
        ? response.errors[0]?.message
        : response?.successMessage || defaultErrorMessage;

    error.value = errorMessage;

    const failureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return failureResult;
  };

  const validateDeleteLocation = (locationId) => {
    if (!locationId) {
      return "location ID is required";
    }
    return null;
  };

  const deleteLocation = async (locationId) => {
    try {
      isDeleting.value = true;
      error.value = null;
      deletedLocation.value = null;

      const validationError = validateDeleteLocation(locationId);
      if (validationError) {
        return handleDeleteFailure({ message: validationError });
      }

      const response = await deleteLocationByIdRepository.deleteLocationById(locationId);

      if (response?.success) {
        return handleDeleteSuccess(response);
      } else {
        return handleDeleteFailure(response);
      }
    } catch (error) {
      return handleDeleteFailure(error, error.message || "Failed to delete Location");
    } finally {
      isDeleting.value = false;
    }
  };

  const resetState = () => {
    isDeleting.value = false;
    error.value = null;
    deletedLocation.value = null;
  };

  return {
    isDeleting,
    error,
    deletedLocation,
    deleteLocation,
    resetState,
  };
}
