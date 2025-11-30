import { ref } from 'vue';
import { deleteControlDesignByIdRepository } from '@/services/riskControl/controlDesign/deleteControlDesignByIdRepository';


export function useDeleteControlDesignById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedControlDesign = ref(null);

    const handleDeleteSuccess = (response) => {
        deletedControlDesign.value = true;
        const message = response?.successMessage || 'Control Design deleted successfully';
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete control design') => {
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

    const validateDeleteControlDesign = (controlDesignId) => {
        if (!controlDesignId) {
            return 'Control Design ID is required';
        }
        return null;
    };
    
    const deleteControlDesign = async (controlDesignId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedControlDesign.value = null;
            
            const validationError = validateDeleteControlDesign(controlDesignId); 
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteControlDesignByIdRepository.deleteControlDesignById(controlDesignId);
            
            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete control design');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedControlDesign.value = null;
    };

    return {
        isDeleting,
        error,
        deletedControlDesign,
        deleteControlDesign,
        resetState,
    };
}