import { ref } from 'vue';
import { createObservationRepository } from '@/services/riskControl/observation/createObservationRepository';

export function useCreateObservation() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdObservation = ref(null);

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

        const failureResult = {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }]
        };

        return failureResult;
    };

    const validateCreateObservation = (observationData) => {
        if (!observationData.highlight) {
            return 'Observation highlight is required';
        }
        if (!observationData.background) {
            return 'Observation background is required';
        }
        if (observationData.isActive === null || observationData.isActive === undefined) {
            return 'Observation is active is required';
        }
        if (!observationData.auditProgramId) {
            return 'AuditProgram ID is required';
        }
        if (!observationData.rateId) {
            return 'Rate ID is required';
        }
        if (!observationData.classification) {
            return 'Observation classification is required';
        }
        return null;
    };

    const createObservation = async (observationData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdObservation.value = null;

            const validationError = validateCreateObservation(observationData);
            if (validationError) {
                return handleCreateFailure(validationError);
            }

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