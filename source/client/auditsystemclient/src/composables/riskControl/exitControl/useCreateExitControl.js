import { ref } from 'vue';
import { createExitControlRepository } from '@/services/riskControl/exitControl/createExitControlRepository.js';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateExitControl() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdExitControl = ref(null);
    const { showNotification } = useNotification();

    const handleCreateSuccess = (response) => {
        createdExitControl.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Exit Control created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create exit control') => {
        const errorMessage = response?.errors?.length
            ? response.errors[0].message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');
        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }]
        };
    };

    const createExitControl = async (exitControlData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdExitControl.value = null;

            const response = await createExitControlRepository.createExitControl(exitControlData);

            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateExitControl: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create exit control');

        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdExitControl.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdExitControl,
        createExitControl,
        resetState
    };
}
