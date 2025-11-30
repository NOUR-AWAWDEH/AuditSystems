import { ref } from 'vue';
import { updateControlDesignByIdRepository } from '@/services/riskControl/controlDesign/updateControlDesignByIdRepository';

export function useUpdateControlDesignById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedControlDesign = ref(null);

    const handleUpdateSuccess = (response) => {
        updatedControlDesign.value = response.result;
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update control design') => {    
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

    const validateUpdateControlDesign = (controlDesignData) => {
        if (!controlDesignData.id) {
            return 'Control Design ID is required';
        }
        
        if (!controlDesignData.description) {
            return 'Control Design description is required';
        }

        if (!controlDesignData.exitControlId) {
            return 'Exit Control ID is required';
        }

        return null;
    };

    const updateControlDesign = async (controlDesignData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedControlDesign.value = null;
            
            const validationError = validateUpdateControlDesign(controlDesignData);
            if (validationError) {
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateControlDesignByIdRepository.updateControlDesignById(controlDesignData);
            

            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update control design');

        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedControlDesign.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedControlDesign,
        updateControlDesign,
        resetState,
    };
};