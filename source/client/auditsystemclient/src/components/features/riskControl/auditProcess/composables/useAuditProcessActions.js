export function useAuditProcessActions({
    searchAuditProcesses,
    getAuditProcessesByUserId,
    clearSearch,
    clearSubDepartment,
    allAuditProcesses,
    searchQuery,
    selectedSubDepartmentId,
    currentPage,
    pageSize
}) {
    const refreshData = async () => {
        const hasSearchOrFilter = searchQuery.value.trim() || selectedSubDepartmentId.value;
        if (hasSearchOrFilter) {
            if (searchAuditProcesses) {
                await searchAuditProcesses({
                    searchTerm: searchQuery.value || '',
                    subDepartmentId: selectedSubDepartmentId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
        } else {
            if (getAuditProcessesByUserId) {
                await getAuditProcessesByUserId();
            }
        }
    };

    const handleAuditProcessChange = async (newSubDepartmentId) => {
        if (clearSearch) clearSearch();
        if (allAuditProcesses?.value !== undefined) allAuditProcesses.value = [];
        selectedSubDepartmentId.value = newSubDepartmentId;
    };

    const resetToDefaultView = () => {
        selectedSubDepartmentId.value = null;
        searchQuery.value = '';
        currentPage.value = 1;
        pageSize.value = 10;
        if (clearSearch) clearSearch();
        if (allAuditProcesses?.value !== undefined) allAuditProcesses.value = [];
        if (clearSubDepartment) clearSubDepartment();
    };

    return { refreshData, handleAuditProcessChange, resetToDefaultView };
}