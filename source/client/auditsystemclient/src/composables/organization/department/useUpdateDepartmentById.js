import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { updateDepartmentByIdRepository } from '@/services/organization/department/updateDepartmentByIdRepository';

export function useUpdateDepartmentById() {
    const isUpdating = ref(false);
    const error = ref(null);
    const updatedDepartment = ref(null);
    const { showNotification } = useNotification();

    const handleUpdateSuccess = (response) => {
        updatedDepartment.value = response.result;
        const message = response?.successMessage || 'Department updated successfully';
        showNotification(message, 'success');
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update department') => {
        const errorMessage = response?.errors?.length
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        return { 
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateDepartmentData = (departmentData) => {
        if (!departmentData.id) {
            return 'Department Id is required';
        }

        if (!departmentData.name) {
            return 'Department name is required';
        }

        if (!departmentData.locationId) {
            return 'Location is required';
        }

        return null;
    };

    const updateDepartment = async (departmentData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedDepartment.value = null;

            const validationError = validateDepartmentData(departmentData);
            if (validationError) {
                showNotification(validationError, 'error');
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateDepartmentByIdRepository.updateDepartmentById(departmentData);

            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }
        }catch(err) {
            return handleUpdateFailure(err, err.message || 'Failed to update department');
        }finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedDepartment.value = null;
        isUpdating.value =false;
    };

    return {
        isUpdating,
        error,
        updatedDepartment,
        updateDepartment,
        resetState,
    };
}