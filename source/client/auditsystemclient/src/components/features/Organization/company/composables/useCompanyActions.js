export function useCompanyActions({
    searchCompanys,
    getCompanysByUserId,
    clearSearch,
    allCompanies,
    searchQuery,
    currentPage,
    pageSize
}) {
    const refreshData = async () => {
        const hasSearchOrFilter = searchQuery.value.trim();
        if (hasSearchOrFilter) {
            if (searchCompanys) {
                await searchCompanys({
                    searchTerm: searchQuery.value || '',
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
        } else {
            if (getCompanysByUserId) {
                await getCompanysByUserId();
            }
        }
    };

    const handleCompanyChange = async () => {
        if (clearSearch) clearSearch();
        if (allCompanies?.value !== undefined) allCompanies.value = [];
        // Logic for selected user if needed, or remove if not applicable in this context
    };

    const resetToDefaultView = () => {
        searchQuery.value = '';
        currentPage.value = 1;
        if (clearSearch) clearSearch();
        // if (allCompanies?.value !== undefined) allCompanies.value = []; // Don't clear all companies on reset, maybe just search results
    };

    return { refreshData, handleCompanyChange, resetToDefaultView };
}