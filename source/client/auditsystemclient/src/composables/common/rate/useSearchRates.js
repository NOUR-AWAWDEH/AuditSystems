import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { searchRatesRepository } from '@/services/common/rate/searchRatesRepository';

export function useSearchRates() {
    // State
    const searchResults = ref([]);
    const isSearching = ref(false);
    const error = ref(null);
    const totalCount = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(10);
    const searchTerm = ref('');
    const { showNotification } = useNotification();

    // --- Handlers ---
    const handleSearchSuccess = (response) => {
        const items = response.result?.items || [];
        const extractedTotalCount = response.result?.totalCount || 0;

        searchResults.value = Array.isArray(items) ? items : [];
        totalCount.value = extractedTotalCount;

        return { success: true, data: response.result };
    };

    const handleSearchFailure = (response, defaultErrorMessage = 'Failed to retrieve rates') => {
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

    // --- Actions ---
    const searchRates = async (searchParams = {}) => {
        try {
            isSearching.value = true;
            error.value = null;

            const params = {
                searchTerm: searchParams.searchTerm ?? searchTerm.value,
                pageNumber: searchParams.pageNumber ?? currentPage.value,
                pageSize: searchParams.pageSize ?? pageSize.value,
            };

            // Sync local state with params
            if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
            if (searchParams.pageNumber !== undefined) currentPage.value = searchParams.pageNumber;
            if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

            const response = await searchRatesRepository.searchRates(params);

            if (response?.success) {
                return handleSearchSuccess(response);
            } else {
                return handleSearchFailure(response);
            }
        } catch (err) {
            return handleSearchFailure(err, err.message || 'Failed to retrieve rates');
        } finally {
            isSearching.value = false;
        }
    };

    const clearSearch = () => {
        searchResults.value = [];
        searchTerm.value = '';
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
        pageSize.value = 10;
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
        searchRates,
        clearSearch,
        resetState,
    };
}