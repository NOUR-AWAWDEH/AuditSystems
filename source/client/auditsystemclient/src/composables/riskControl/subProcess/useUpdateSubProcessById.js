import { ref } from 'vue';
import { updateSubProcessByIdRepository } from '@/services/riskControl/subProcess/updateSubProcessByIdRepository';

export function useUpdateSubProcessById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedSubProcess = ref(null);

    const handleUpdateSuccess = (response) => {
        updatedSubProcess.value = response.result;
        const message = response?.successMessage || 'SubProcess updated successfully';
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update SubProcess') => {    
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

    const validateUpdateSubProcess = (subProcessData) => {
        if (!subProcessData.id) {
            return 'SubProcess ID is required';
        }
        
        if (!subProcessData.particular) {
            return 'SubProcess particular is required';
        }

        if (!subProcessData.auditProcessId) {
            return 'AuditProcess ID is required';
        }

        return null;
    };

    const updateSubProcess = async (subProcessData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedSubProcess.value = null;
            
            const validationError = validateUpdateSubProcess(subProcessData);
            if (validationError) {
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateSubProcessByIdRepository.updateSubProcessById(subProcessData);
            
            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update sub process');

        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedSubProcess.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedSubProcess,
        updateSubProcess,
        resetState,
    };
};