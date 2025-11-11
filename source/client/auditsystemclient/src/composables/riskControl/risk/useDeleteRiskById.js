import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { deleteRiskByIdRepository } from '@/services/riskControl/risk/deleteRiskByIdRepository';


export function useDeleteRiskById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedRisk = ref(null);
    const { showNotification } = useNotification();

    const handleDeleteSuccess = (response) => {
        deletedRisk.value = true;
        const message = response?.successMessage || 'Risk deleted successfully';
        showNotification(message, 'success');
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete risk') => {
        const errorMessage = response?.errors?.length > 0
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        const failureResult = {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };

        return failureResult;
    };

    const validateDeleteRisk = (riskId) => {
        if (!riskId) {
            return 'Risk ID is required';
        }
        return null;
    };

    const deleteRisk = async (riskId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedRisk.value = null;

            const validationError = validateDeleteRisk(riskId);
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteRiskByIdRepository.deleteRiskById(riskId);


            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete risk');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedRisk.value = null;
    };

    return {
        isDeleting,
        error,
        deletedRisk,
        deleteRisk,
        resetState,
    };
}
