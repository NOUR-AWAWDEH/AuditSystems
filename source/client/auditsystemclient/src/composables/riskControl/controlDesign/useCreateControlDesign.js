import { ref } from 'vue';
import { createControlDesignRepository } from '@/services/riskControl/controlDesign/createControlDesignRepository.js';

export function useCreateControlDesign() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdControlDesign = ref(null);

    const handleCreateSuccess = (response) => {
        createdControlDesign.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Control Design created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create control design') => {
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

    const createControlDesign = async (controlDesignData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdControlDesign.value = null;
            controlDesignData.Inadequate = false;

            const response = await createControlDesignRepository.createControlDesign(controlDesignData);

            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateControlDesign: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create control design');

        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdControlDesign.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdControlDesign,
        createControlDesign,
        resetState
    };
}
