import { ref } from 'vue';
import { searchSubProcessesByUserIdRepository } from '@/services/riskControl/subProcess/searchSubProcessesByUserIdRepository';

export function useSearchSubProcessesByUserId() {
    const searchResults = ref([]);
    const isSearching = ref(false);
    const error = ref(null);
    const totalCount = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(10);
    const searchTerm = ref('');
    const auditProcessId = ref(null);

    const handleSearchSuccess = (response) => {
        const items = response.result?.result?.items || response.result?.items || [];
        const extractedTotalCount = response.result?.result?.totalCount || response.result?.totalCount || 0;

        const isValidArray = Array.isArray(items);
        
        searchResults.value = isValidArray ? items : [];
        totalCount.value = extractedTotalCount;

        return { success: true, data: response.result };
    };

    const handleSearchFailure = (response, defaultErrorMessage = 'Failed to search sub processes') => {
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

    const searchSubProcesses = async (searchParams = {}) => {
        try {
            isSearching.value = true;
            error.value = null;

            const params = {
                searchTerm: searchParams.searchTerm || searchTerm.value,
                auditProcessId: searchParams.auditProcessId || auditProcessId.value,
                page: searchParams.page || currentPage.value,
                pageSize: searchParams.pageSize || pageSize.value,
            }

            if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
            if (searchParams.auditProcessId !== undefined) auditProcessId.value = searchParams.auditProcessId;
            if (searchParams.page !== undefined) currentPage.value = searchParams.page;
            if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

            const response = await searchSubProcessesByUserIdRepository.searchSubProcessesByUserId(
                params.searchTerm,
                params.page,
                params.pageSize,
                params.auditProcessId
            );
            
            if (response?.success) {
                return handleSearchSuccess(response);
            } else {
                return handleSearchFailure(response);
            }
            
        } catch (err) {
            return handleSearchFailure(err, err.message || 'Failed to search sub processes');

        } finally {
            isSearching.value = false;
        }
    };

    const clearSearch = () => {
        searchResults.value = [];
        searchTerm.value = '';
        auditProcessId.value = null;
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
        auditProcessId.value = null;
    };

    return {
        searchResults,
        isSearching,
        error,
        totalCount,
        currentPage,
        pageSize,
        searchTerm,
        searchSubProcesses,
        clearSearch,
        resetState
    }
}