import { ref } from 'vue';
import { getCompanyByUserIdRepository } from '@/services/organization/company/getCompanysByUserIdRepository';

export function useGetCompanysByUserId() {
    const companies = ref([]);
    const isLoading = ref(false);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        const companiesData = response.result || [];
        companies.value = companiesData;
        return { success: true, data: companiesData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch companies') => {
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

    const getCompanysByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            companies.value = [];

            const response = await getCompanyByUserIdRepository.getCompanyByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }
            
        } catch (err) {
            return handleFetchFailure(err, err.message || 'Failed to fetch companies');
        } finally {
            isLoading.value = false;
        }
    };

    return {
        companies,
        isLoading,
        error,
        getCompanysByUserId,
    }
}