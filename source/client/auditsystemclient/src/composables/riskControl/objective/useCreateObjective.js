import { ref } from 'vue';
import { createObjectiveRepository } from '@/services/riskControl/objective/CreateObjectiveRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateObjective() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdObjective = ref(null);
    const { showNotification } = useNotification();

    const handleCreateSuccess = (response) => {
        createdObjective.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Objective created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create objective') => {
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

    const createObjective = async (objectiveData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdObjective.value = null;

            const response = await createObjectiveRepository.createObjective(objectiveData);

            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateObjective: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create objective');

        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdObjective.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdObjective,
        createObjective,
        resetState
    };
}
