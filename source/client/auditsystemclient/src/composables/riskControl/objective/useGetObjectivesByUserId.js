import { ref } from 'vue';
import { getObjectivesByUserIdRepository } from '@/services/riskControl/objective/getObjectivesByUserIdRepository';

export function useGetObjectivesByUserId() {
    const objectives = ref([]);
    const isLoading = ref(false);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        const objectivesData = Array.isArray(response.result) ? response.result : [];
        objectives.value = objectivesData;
        return { success: true, data: objectivesData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch objectives') => {
        const errorMessage = response?.errors?.length > 0
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const getObjectivesByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            objectives.value = [];

            const response = await getObjectivesByUserIdRepository.getObjectivesByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch objectives');


        } finally {
            isLoading.value = false;
        }
    };

    return {
        objectives,
        isLoading,
        error,
        getObjectivesByUserId,
    };
}
