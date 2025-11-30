import { ref } from 'vue';
import { createRiskRepository } from '@/services/riskControl/risk/createRiskRepository';

export function useCreateRisk() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdRisk = ref(null);

    const handleCreateSuccess = (response) => {
        createdRisk.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Risk created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create risk') => {
        const errorMessage = response?.errors?.length
            ? response.errors[0].message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }]
        };
    };

    const createRisk = async (riskData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdRisk.value = null;

            const response = await createRiskRepository.createRisk(riskData);


            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateRisk: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create risk');

        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdRisk.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdRisk,
        createRisk,
        resetState
    };
}
