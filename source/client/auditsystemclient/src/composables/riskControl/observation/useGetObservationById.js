import { ref } from 'vue';
import { getObservationByIdRepository } from '@/services/riskControl/observation/getObservationByIdRepository';


export function useGetObservationById() {
    const observation = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        observation.value = response.result;
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch observation!') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateObservationId = (observationId) => {
        if (!observationId) {
            const message = 'Observation ID is required.';
            return { success: false, message };
        }
    };

    const getObservationById = async (observationId) => {
        try {
            isLoading.value = true;
            error.value = null;
            observation.value = null;

            const validationError = validateObservationId(observationId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getObservationByIdRepository.getObservationById(observationId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch observation');
        
        } finally {
            isLoading.value = false;
        }
    };

    const clearObservation = () => {
        error.value = null;
        observation.value = null;
        isLoading.value = false;
    };

    return {
        observation,
        isLoading,
        error,
        getObservationById,
        clearObservation,
    };
}