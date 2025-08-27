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
        try {
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
        } catch (error) {
            console.error('Error refreshing audit process data:', error);
        }
    };

    const handleAuditProcessChange = async (newSubDepartmentId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allAuditProcesses?.value !== undefined) {
                allAuditProcesses.value = [];
            }

            selectedSubDepartmentId.value = newSubDepartmentId;
        } catch (error) {
            console.error('Error handling audit process change:', error);
        }
    };

    const resetToDefaultView = () => {
        try {
            selectedSubDepartmentId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allAuditProcesses?.value !== undefined) {
                allAuditProcesses.value = [];
            }

            if (clearSubDepartment) {
                clearSubDepartment();
            }
        } catch (error) {
            console.error('Error resetting to default view:', error);
        }
    };

    return {
        refreshData,
        handleAuditProcessChange,
        resetToDefaultView
    };
}
