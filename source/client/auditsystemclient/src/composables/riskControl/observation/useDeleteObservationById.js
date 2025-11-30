import { ref } from 'vue';
import { deleteObservationByIdRepository } from '@/services/riskControl/observation/deleteObservationByIdRepository';

export function useDeleteObservationById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedObservation = ref(null);

    const handleDeleteSuccess = (response) => {
        deletedObservation.value = true;
        const message = response?.successMessage || 'Observation deleted successfully';
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete observation') => {
        const errorMessage = response?.errors?.length > 0
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

    const validateDeleteObservation = (observationId) => {
        if (!observationId) {
            return 'Observation ID is required';
        }
        return null;
    };

    const deleteObservation = async (observationId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedObservation.value = null;

            const validationError = validateDeleteObservation(observationId);
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteObservationByIdRepository.deleteObservationById(observationId);

            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete observation');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedObservation.value = null;
    };

    return {
        isDeleting,
        error,
        deletedObservation,
        deleteObservation,
        resetState
    };
}
