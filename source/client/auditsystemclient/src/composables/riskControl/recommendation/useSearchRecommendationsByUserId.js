import { ref } from 'vue';
import { searchRecommendationsByUserIdRepository } from '@/services/riskControl/recommendation/searchRecommendationsByUserIdRepository';

export function useSearchRecommendationsByUserId() {
    const searchResults = ref([]);
    const isSearching = ref(false);
    const error = ref(null);
    const totalCount = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(10);
    const searchTerm = ref('');
    const observationId = ref(null);

    const handleSearchSuccess = (response) => {
        const items = response.result?.items || [];
        const extractedTotalCount = response.result?.totalCount || 0;


        const isValidArray = Array.isArray(items);

        searchResults.value = isValidArray ? items : [];
        totalCount.value = extractedTotalCount;

        return { success: true, data: response.result };
    };

    const handleSearchFailure = (response, defaultErrorMessage = 'Failed to search Recommendations') => {
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

    const searchRecommendations = async (searchParams = {}) => {
        try {
            isSearching.value = true;
            error.value = null;

            const params = {
                searchTerm: (searchParams.searchTerm || searchTerm.value || '').trim(),
                observationId: searchParams.observationId || observationId.value,
                pageNumber: searchParams.pageNumber || currentPage.value,
                pageSize: searchParams.pageSize || pageSize.value,
            };



            if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
            if (searchParams.observationId !== undefined) observationId.value = searchParams.observationId;
            if (searchParams.pageNumber !== undefined) currentPage.value = searchParams.pageNumber;
            if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

            const response = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId(
                params.searchTerm,
                params.pageNumber,
                params.pageSize,
                params.observationId
            );


            if (response?.success) {
                return handleSearchSuccess(response);
            } else {
                return handleSearchFailure(response);
            }

        } catch (err) {
            return handleSearchFailure(err, err.message || 'Failed to search Recommendations');

        } finally {
            isSearching.value = false;
        }
    };

    const clearSearch = () => {
        searchResults.value = [];
        searchTerm.value = '';
        observationId.value = null;
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
        observationId.value = null;
    };

    return {
        searchResults,
        isSearching,
        error,
        totalCount,
        currentPage,
        pageSize,
        searchTerm,
        observationId,
        searchRecommendations,
        clearSearch,
        resetState
    }
}