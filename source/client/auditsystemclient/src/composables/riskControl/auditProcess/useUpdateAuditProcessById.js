import { ref } from 'vue';
import { updateAuditProcessByIdRepository } from '@/services/riskControl/auditProcess/updateAuditProcessByIdRepository';

export function useUpdateAuditProcessById() {
    const isUpdating =ref(false);
    const error = ref(null);
    const updatedAuditProcess = ref(null);

    const handleUpdateSuccess = (response) => {
        updatedAuditProcess.value = response.result;
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update process') => {
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

    const validateUpdateAuditProcess = (auditProcessData) => {
        if (!auditProcessData.id) {
            return 'Process ID is required';
        }
        
        if (!auditProcessData.processName) {
            return 'Process name is required';
        }

        if (!auditProcessData.subDepartmentId) {
            return 'SubDepartment ID is required';
        }

        return null;
    };

    const updateAuditProcess = async (auditProcessData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updateAuditProcess.value = null;
            
            const validationError = validateUpdateAuditProcess(auditProcessData);
            if (validationError) {
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateAuditProcessByIdRepository.updateAuditProcessById(auditProcessData);
            
            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        } catch (error) {
            return handleUpdateFailure(error, error.message || 'Failed to update process');
        } finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updateAuditProcess.value = null;
        isUpdating.value = false;
    };

    return {
        isUpdating,
        error,
        updatedAuditProcess,
        updateAuditProcess,
        resetState,
    };
};