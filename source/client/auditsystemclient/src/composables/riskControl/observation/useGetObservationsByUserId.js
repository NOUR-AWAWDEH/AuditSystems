import { ref } from 'vue';
import { getObservationsByUserIdRepository } from '@/services/riskControl/observation/getObservationsByUserIdRepository';

export function useGetObservationsByUserId() {
    const observations = ref([]);
    const isLoading = ref(false);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        const observationsData =  Array.isArray(response.result)
        ? response.result
        : response.result?.observations || [];

        observations.value = observationsData;
        return { success: true, data: observationsData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch observations') => {    
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

    const getObservationsByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            observations.value = [];

            const response = await getObservationsByUserIdRepository.getObservationsByUserId();
            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch observations');
            
        } finally {
            isLoading.value = false;
        }
    };

    return {
        observations,
        isLoading,
        error,
        getObservationsByUserId
    };
}