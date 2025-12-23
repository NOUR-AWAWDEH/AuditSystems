import { ref } from 'vue';
import { updateObservationByIdRepository } from '@/services/riskControl/observation/updateObservationByIdRepository';

export function useUpdateObservationById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedObservation = ref(null);

    const handleUpdateSuccess = (response) => {
        updatedObservation.value = response.result;
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update Observation') => {    
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

    const validateUpdateObservation = (observationData) => {
        if (!observationData.id) {
            return 'Observation ID is required';
        }
        
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

        return null;
    };
    
    const updateObservation = async (observationData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedObservation.value = null;
            
            const validationError = validateUpdateObservation(observationData);
            if (validationError) {
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateObservationByIdRepository.updateObservationById(observationData);
            
            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update Observation');

        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedObservation.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedObservation,
        updateObservation,
        resetState
    };
};