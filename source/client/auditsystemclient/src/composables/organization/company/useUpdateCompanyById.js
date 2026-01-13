import { ref } from 'vue';
import { updateCompanyByIdRepository } from '@/services/organization/company/updateCompanyByIdRepository';

export function useUpdateCompanyById() {
    const isUpdating = ref(false);
    const error = ref(null);
    const updatedCompany = ref(null);

    const handleUpdateSuccess = (response) => {
        updatedCompany.value = response.result;
        return { success: true, data: response.result };
    };

    const handleUpdateFailure = (response, defaultErrorMessage = 'Failed to update company') => {
        const errorMessage = response?.errors?.length
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;

        return { 
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateCompanyData = (companyData) => {
        if (!companyData.id) {
            return 'Company Id is required';
        }

        if (!companyData.name) {
            return 'Company name is required';
        }
        
        return null;
    };

    const updateCompany = async (companyData) => {
        try {
            isUpdating.value = true;
            error.value = null;
            updatedCompany.value = null;

            const validationError = validateCompanyData(companyData);
            if (validationError) {
                return handleUpdateFailure({ message: validationError });
            }

            const response = await updateCompanyByIdRepository.updateCompanyById(companyData);

            if (response?.success) {
                return handleUpdateSuccess(response);
            } else {
                return handleUpdateFailure(response);
            }

        }catch(err) {
            return handleUpdateFailure(err, err.message || 'Failed to update company');
        }finally {
            isUpdating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        updatedCompany.value = null;
        isUpdating.value =false;
    };

    return {
        isUpdating,
        error,
        updatedCompany,
        updateCompany,
        resetState,
    };
}