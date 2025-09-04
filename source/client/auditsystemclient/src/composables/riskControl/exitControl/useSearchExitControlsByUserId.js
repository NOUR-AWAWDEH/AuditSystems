import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { searchExitControlsByUserIdRepository } from '@/services/riskControl/exitControl/searchExitControlsByUserIdRepository';

export function useSearchExitControlsByUserId() {
    const searchResults = ref([]);
    const isSearching = ref(false);
    const error = ref(null);
    const totalCount = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(10);
    const searchTerm = ref('');
    const riskId = ref(null);
    const { showNotification } = useNotification();

    const handleSearchSuccess = (response) => {
        const items = response.result?.items || [];
        const extractedTotalCount = response.result?.totalCount || 0;

        const isValidArray = Array.isArray(items);

        searchResults.value = isValidArray ? items : [];
        totalCount.value = extractedTotalCount;

        return { success: true, data: response.result };
    };

    const handleSearchFailure = (response, defaultErrorMessage = 'Failed to search exit controls') => {
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

    const searchExitControls = async (searchParams = {}) => {
        try {
            isSearching.value = true;
            error.value = null;

            const params = {
                searchTerm: searchParams.searchTerm || searchTerm.value,
                riskId: searchParams.riskId || riskId.value,
                pageNumber: searchParams.pageNumber || currentPage.value,
                pageSize: searchParams.pageSize || pageSize.value,
            }


            if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
            if (searchParams.riskId !== undefined) riskId.value = searchParams.riskId;
            if (searchParams.pageNumber !== undefined) currentPage.value = searchParams.pageNumber;
            if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

            const response = await searchExitControlsByUserIdRepository.searchExitControlsByUserId(
                params.searchTerm,
                params.pageNumber,
                params.pageSize,
                params.riskId
            );

            if (response?.success) {
                return handleSearchSuccess(response);
            } else {
                return handleSearchFailure(response);
            }

        } catch (err) {
            return handleSearchFailure(err, err.message || 'Failed to search exit controls');

        } finally {
            isSearching.value = false;
        }
    };

    const clearSearch = () => {
        searchResults.value = [];
        searchTerm.value = '';
        riskId.value = null;
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
        riskId.value = null;
    };

    return {
        searchResults,
        isSearching,
        error,
        totalCount,
        currentPage,
        pageSize,
        searchTerm,
        riskId,
        searchExitControls,
        clearSearch,
        resetState
    }
}