import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { searchObservationsByUserIdRepository } from '@/services/riskControl/observation/searchObservationsByUserIdRepository';

export function useSearchObservationsByUserId() {   
    const searchResults = ref([]);
    const isSearching = ref(false);
    const error = ref(null);
    const totalCount = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(10);
    const searchTerm = ref('');
    const auditProgramId = ref(null);
    const { showNotification } = useNotification();

    const handleSearchSuccess = (response) => {
        const items = response.result?.result?.items || response.result?.items || [];
        const extractedTotalCount = response.result?.result?.totalCount || response.result?.totalCount || 0;

        const isValidArray = Array.isArray(items);
        
        searchResults.value = isValidArray ? items : [];
        totalCount.value = extractedTotalCount;

        return { success: true, data: response.result };
    };

    const handleSearchFailure = (response, defaultErrorMessage = 'Failed to search observations') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        showNotification(errorMessage, 'error');

        return { 
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const searchObservations = async (searchParams = {}) => {
        try {
            isSearching.value = true;
            error.value = null;

            const params = {
                searchTerm: searchParams.searchTerm || searchTerm.value,
                auditProgramId: searchParams.auditProgramId || auditProgramId.value,
                page: searchParams.page || currentPage.value,
                pageSize: searchParams.pageSize || pageSize.value,
            }

            if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
            if (searchParams.auditProgramId !== undefined) auditProgramId.value = searchParams.auditProgramId;
            if (searchParams.page !== undefined) currentPage.value = searchParams.page;
            if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

            const response = await searchObservationsByUserIdRepository.searchObservationsByUserId(
                params.searchTerm,
                params.page,
                params.pageSize,
                params.auditProgramId
            );
            
            if (response?.success) {
                return handleSearchSuccess(response);
            } else {
                return handleSearchFailure(response);
            }
            
        } catch (err) {
            return handleSearchFailure(err, err.message || 'Failed to search observations');

        } finally {
            isSearching.value = false;
        }
    };

    const clearSearch = () => {
        searchResults.value = [];
        searchTerm.value = '';
        auditProgramId.value = null;
        currentPage.value = 1;
        error.value = null;
        totalCount.value = 0;
        pageSize.value = 10;
    };
    
    const resetState = () => {
        searchResults.value = [];
        isSearching.value = false;
        error.value = null;
        totalCount.value = 0;
        currentPage.value = 1;
        searchTerm.value = '';
        auditProgramId.value = null;
    };

    return {
        searchResults,
        isSearching,
        error,
        totalCount,
        currentPage,
        pageSize,
        searchTerm,
        auditProgramId,
        searchObservations,
        clearSearch,
        resetState
    }
}