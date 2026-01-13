import { ref } from 'vue';
import { getCompanyByIdRepository } from '@/services/organization/company/getCompanyByIdRepository';

export function useGetCompanyById() {
    const company = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        company.value = response.result;
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch Company') => {
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

    const validateCompanyId = (companyId) => {
        if (!companyId) {
            const message = 'Company ID is required';
            return { success: false, message };
        }
    };

    const getCompanyById = async (companyId) => {
        try {
            isLoading.value = true;
            error.value = null;
            company.value = null;

            const validationError = validateCompanyId(companyId);
            if (validationError) {
                return handleFetchFailure({ message: validationError });
            }

            const response = await getCompanyByIdRepository.getCompanyById(companyId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch Company' );

        }finally {
            isLoading.value = false;
        }
    };

    const clearCompany = () => {
        error.value = null;
        company.value = null;
        isLoading.value = false;
    };

    return {
        company,
        isLoading,
        error,
        getCompanyById,
        clearCompany,
    };
}
