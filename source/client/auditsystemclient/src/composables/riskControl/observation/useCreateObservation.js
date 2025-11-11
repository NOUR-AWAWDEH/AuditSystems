import { ref } from 'vue';
import { createObservationRepository } from '@/services/riskControl/observation/createObservationRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateObservation() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdObservation = ref(null);
    const { showNotification } = useNotification();

    const handleCreateSuccess = (response) => {
        createdObservation.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Observation created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create observation') => {
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

    const createObservation = async (observationData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdObservation.value = null;

            const response = await createObservationRepository.createObservation(observationData);

            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateObservation: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create observation');
            
        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdObservation.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdObservation,
        createObservation,
        resetState
    };
}