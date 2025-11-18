import { ref } from 'vue';
import { getObjectiveByIdRepository } from '@/services/riskControl/objective/getObjectiveByIdRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useGetObjectiveById() {
    const objective = ref(null);
    const isLoading = ref(null);
    const error = ref(null);
    const { showNotification } = useNotification();

    const handleFetchSuccess = (response) => {
        objective.value = response.result;
        const message = response?.successMessage || 'Objective fetched successfully!';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch objective!') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        //showNotification(errorMessage, 'error');

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateObjectiveId = (objectiveId) => {
        if (!objectiveId) {
            const message = 'Objective ID is required.';
            //showNotification(message, 'error');
            return { success: false, message };
        }
    };

    const getObjectiveById = async (objectiveId) => {
        try {
            isLoading.value = true;
            error.value = null;
            objective.value = null;

            const validationError = validateObjectiveId(objectiveId);
            if (validationError) {
                return handleFetchFailure(validationError);
            }

            const response = await getObjectiveByIdRepository.getObjectiveById(objectiveId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch objective');

        } finally {
            isLoading.value = false;
        }
    };

    const clearObjective = () => {
        error.value = null;
        objective.value = null;
        isLoading.value = false;
    };

    return {
        objective,
        isLoading,
        error,
        getObjectiveById,
        clearObjective,
    };
}
