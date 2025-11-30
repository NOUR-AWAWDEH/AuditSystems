import { ref } from 'vue';
import { getControlDesignByIdRepository } from '@/services/riskControl/controlDesign/getControlDesignByIdRepository';

export function useGetControlDesignById() {
    const controlDesign = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        controlDesign.value = response.result;
        const message = response?.successMessage || 'Control Design fetched successfully!';
        return { success: true, data: controlDesign.value };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch control design!') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateControlDesignId = (controlDesignId) => {
        if (!controlDesignId) {
            const message = 'Control Design ID is required.';
            return { success: false, message };
        }
    };

    const getControlDesignById = async (controlDesignId) => {
        try {
            isLoading.value = true;
            error.value = null;
            controlDesign.value = null;

            const validationError = validateControlDesignId(controlDesignId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getControlDesignByIdRepository.getControlDesignById(controlDesignId);
            
            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch control design');
        
        } finally {
            isLoading.value = false;
        }
    };

    const clearControlDesign = () => {
        error.value = null;
        controlDesign.value = null;
        isLoading.value = false;
    };

    return {
        controlDesign,
        isLoading,
        error,
        getControlDesignById,
        clearControlDesign,
    };
}