import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { searchDepartmentsByUserIdRepository } from '@/services/organization/department/searchDepartmentsByUserIdRepository';

export function useSearchDepartmentsByUserId() {
    const searchResults = ref([]);
    const isSearching = ref(false);
    const error = ref(null);
    const totalCount = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(10);
    const searchTerm = ref('');
    const subLocationId = ref(null);
    const { showNotification } = useNotification();

    const handleSearchSuccess = (response) => {
        const items = response.result?.result?.items || response.result?.items || [];
        const extractedTotalCount = response.result?.result?.totalCount || response.result?.totalCount || 0;

        const isValidArray = Array.isArray(items);
        
        searchResults.value = isValidArray ? items : [];
        totalCount.value = extractedTotalCount;

        return { success: true, data: response.result };
    };

    const handleSearchFailure = (response, defaultErrorMessage = 'Failed to search departments') => {
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

    const searchDepartments = async (searchParams = {}) => {
        try {
            isSearching.value = true;
            error.value = null;

            const params = {
                searchTerm: searchParams.searchTerm || searchTerm.value,
                subLocationId: searchParams.subLocationId || subLocationId.value,
                page: searchParams.page || currentPage.value,
                pageSize: searchParams.pageSize || pageSize.value,
            }

            if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
            if (searchParams.subLocationId !== undefined) subLocationId.value = searchParams.subLocationId;
            if (searchParams.page !== undefined) currentPage.value = searchParams.page;
            if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

            const response = await searchDepartmentsByUserIdRepository.searchDepartmentsByUserId(params);
            
            if (response?.success) {
                return handleSearchSuccess(response);
            } else {
                return handleSearchFailure(response);
            }
            
        } catch (err) {
            return handleSearchFailure(err, err.message || 'Failed to search departments');
        } finally {
            isSearching.value = false;
        }
    };

    const clearSearch = () => {
        searchResults.value = [];
        searchTerm.value = '';
        subLocationId.value = null;
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
        subLocationId.value = null;
    };

    return {
        // State
        searchResults,
        isSearching,
        error,
        totalCount,
        currentPage,
        pageSize,
        searchTerm,
        // Actions
        searchDepartments,
        clearSearch,
        resetState
    }
}