import { ref } from 'vue';
import { getDepartmentByUserIdRepository } from '@/services/organization/department/getDepartmentsByUserIdRepository';

export function useGetDepartmentsByUserId() {
    const departments = ref([]);
    const isLoading = ref(false);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        const departmentsData = response.result?.departments || [];
        departments.value = departmentsData;
        return { success: true, data: departmentsData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch departments') => {
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

    const getDepartmentsByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            departments.value = [];

            const response = await getDepartmentByUserIdRepository.getDepartmentByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }
        } catch (err) {
            return handleFetchFailure(err, err.message || 'Failed to fetch departments');
        } finally {
            isLoading.value = false;
        }
    };

    return {
        departments,
        isLoading,
        error,
        getDepartmentsByUserId,
    }
}