import { ref } from 'vue';
import { createCompanyRepository } from '@/services/organization/company/createCompanyRepository';

export function useCreateCompany() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdCompany = ref(null);

    const handleCreateSuccess = (response) => {
        createdCompany.value = response.result;
        return { success: true, data: response.result };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create Company.') => {
        const errorMessage = response?.errors?.length
            ? response.errors[0].message
            : response?.message || defaultErrorMessage;
        
        error.value = errorMessage;

        return { 
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateCompanyData = (companyData) => {
        if (!companyData.name?.trim()) {
            return 'Company name is required.';
        }
        return null;
    };

    const createCompany = async (companyData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdCompany.value = null;

            const validationError = validateCompanyData(companyData);
            if (validationError) {
                return handleCreateFailure({ message: validationError });
            }

            const response = await createCompanyRepository.createCompany(companyData);

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
        createdCompany.value = null;
    };

    return {
        isCreating,
        error,
        createdCompany,
        createCompany,
        resetState
    };
}