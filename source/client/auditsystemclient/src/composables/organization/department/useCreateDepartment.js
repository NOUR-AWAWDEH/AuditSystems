import { ref } from 'vue';
import { createDepartmentRepository } from '@/services/organization/department/createDepartmentRepository';
import { useNotification } from '@/composables/notification/useNotification';

export function useCreateDepartment() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdDepartment = ref(null);
    const { showNotification } = useNotification();

    const handleCreateSuccess = (response) => {
        createdDepartment.value = response.result;
        showNotification(response?.successMessage || 'Department created successfully.', 'success');
        return { success: true, data: response.result };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create department.') => {
        const errorMessage = response?.errors?.length
            ? response.errors[0].message  // Fixed: was response.error[0].message
            : response?.message || defaultErrorMessage;
        
        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        return { 
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateDepartmentData = (departmentData) => {
        if (!departmentData.name?.trim()) {
            return 'Department name is required.';
        }
        if (!departmentData.subLocationId) {
            return 'Sub-location is required.';
        }
        return null;
    };

    const createDepartment = async (departmentData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdDepartment.value = null;

            const validationError = validateDepartmentData(departmentData);
            if (validationError) {
                return handleCreateFailure({ message: validationError });
            }

            const response = await createDepartmentRepository.createDepartment(departmentData);

            if (response.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }
        } catch (err) {
            return handleCreateFailure(err, err.message || 'Create failed' );
        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        isCreating.value = false;
        error.value = null;
        createdDepartment.value = null;
    };

    return {
        isCreating,
        error,
        createdDepartment,
        createDepartment,
        resetState
    };
}