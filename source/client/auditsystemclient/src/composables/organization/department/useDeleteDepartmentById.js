import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { deleteDepartmentByIdRepository } from '@/services/organization/department/deleteDepartmentRepository';

export function useDeleteDepartmentById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedDepartment = ref(null);
    const { showNotification } = useNotification();

    const handleDeleteSuccess = (response) => {
        deletedDepartment.value = true;
        const message = response?.successMessage || 'Department deleted successfully!';
        showNotification(message, 'success');
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete department') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        const failureResult = {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };

        return failureResult;
    };

    const validateDepartmentId = (departmentId) => {
        if (!departmentId) {
            const message = 'Department ID is required';
            showNotification(message, 'error');
            return { success: false, message };
        }
    };

    const deleteDepartment = async (departmentId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedDepartment.value = null;

            const validationError = validateDepartmentId(departmentId);
            if (validationError) {
                return handleDeleteFailure({ message: validationError });
            }

            const response = await deleteDepartmentByIdRepository.deleteDepartmentById(departmentId);

            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (err) {
            return handleDeleteFailure(err, err.message || 'Failed to delete department');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        deletedDepartment.value = null;
        isDeleting.value = false;
    };

    return {
        isDeleting,
        error,
        deletedDepartment,
        deleteDepartment,
        resetState,
    };
}