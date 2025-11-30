import { ref } from 'vue';
import { searchAuditProgramsByUserIdRepository } from '@/services/riskControl/auditProgram/searchAuditProgramsByUserIdRepository';

export function useSearchAuditProgramsByUserId() {
    const searchResults = ref([]);
    const isSearching = ref(false);
    const error = ref(null);
    const totalCount = ref(0);
    const currentPage = ref(1);
    const pageSize = ref(10);
    const searchTerm = ref('');
    const exitControlId = ref(null);

    const handleSearchSuccess = (response) => {
        const items = response.result?.items || [];
        const extractedTotalCount = response.result?.totalCount || 0;

        const isValidArray = Array.isArray(items);

        searchResults.value = isValidArray ? items : [];
        totalCount.value = extractedTotalCount;

        return { success: true, data: response.result };
    };

    const handleSearchFailure = (response, defaultErrorMessage = 'Failed to search audit programs') => {
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

    const searchAuditPrograms = async (searchParams = {}) => {
        try {
            isSearching.value = true;
            error.value = null;

            const params = {
                searchTerm: searchParams.searchTerm || searchTerm.value,
                exitControlId: searchParams.exitControlId || exitControlId.value,
                pageNumber: searchParams.pageNumber || currentPage.value,
                pageSize: searchParams.pageSize || pageSize.value,
            }


            if (searchParams.searchTerm !== undefined) searchTerm.value = searchParams.searchTerm;
            if (searchParams.exitControlId !== undefined) exitControlId.value = searchParams.exitControlId;
            if (searchParams.pageNumber !== undefined) currentPage.value = searchParams.pageNumber;
            if (searchParams.pageSize !== undefined) pageSize.value = searchParams.pageSize;

            // This line was missing! It calls the repository and assigns the result to 'response'.
            const response = await searchAuditProgramsByUserIdRepository.searchAuditProgramsByUserId(
                params.searchTerm,
                params.pageNumber,
                params.pageSize,
                params.exitControlId
            );

            if (response?.success) {
                return handleSearchSuccess(response);
            } else {
                return handleSearchFailure(response);
            }

        } catch (err) {
            return handleSearchFailure(err, err.message || 'Failed to search audit programs');

        } finally {
            isSearching.value = false;
        }
    };

    const clearSearch = () => {
        searchResults.value = [];
        searchTerm.value = '';
        exitControlId.value = null;
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
        exitControlId.value = null;
    };

    return {
        searchResults,
        isSearching,
        error,
        totalCount,
        currentPage,
        pageSize,
        searchTerm,
        exitControlId,
        searchAuditPrograms,
        clearSearch,
        resetState
    }
}