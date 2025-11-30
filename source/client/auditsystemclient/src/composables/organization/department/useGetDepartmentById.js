import { ref } from 'vue';
import { getDepartmentByIdRepository } from '@/services/organization/department/getDepartmentByIdRepository';

export function useGetDepartmentById() {
    const department = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        department.value = response.result;
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch department') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateDepartmentId = (departmentId) => {
        if (!departmentId) {
            const message = 'Department ID is required';
            return { success: false, message };
        }
    };

    const getDepartmentById = async (departmentId) => {
        try {
            isLoading.value = true;
            error.value = null;
            department.value = null;

            const validationError = validateDepartmentId(departmentId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getDepartmentByIdRepository.getDepartmentById(departmentId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch department' );

        }finally {
            isLoading.value = false;
        }
    };

    const clearDepartment = () => {
        error.value = null;
        department.value = null;
        isLoading.value = false;
    };

    return {
        department,
        isLoading,
        error,
        getDepartmentById,
        clearDepartment,
    };
}
