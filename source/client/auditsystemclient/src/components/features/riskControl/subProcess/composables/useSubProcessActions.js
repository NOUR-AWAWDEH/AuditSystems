export function useSubProcessActions({
    searchSubProcesses,
    getSubProcessesByUserId,
    clearSearch,
    clearAuditProcess,
    allSubProcesses,
    searchQuery,
    selectedAuditProcessId,
    currentPage,
    pageSize
}) {

    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedAuditProcessId.value;

            if (hasSearchOrFilter) {
                if (searchSubProcesses) {
                    await searchSubProcesses({
                        searchTerm: searchQuery.value || '',
                        auditProcessId: selectedAuditProcessId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getSubProcessesByUserId) {
                    await getSubProcessesByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing sub process data:', error);
        }
    };

    const handleSubProcessAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling sub process addition:', error);
        }
    };

    const handleSubProcessChange = async (newAuditProcessId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allSubProcesses?.value !== undefined) {
                allSubProcesses.value = [];
            }

            selectedAuditProcessId.value = newAuditProcessId;
        } catch (error) {
            console.error('Error handling sub process change:', error);
        }
    };

    const resetToDefaultView = () => {
        try {
            selectedAuditProcessId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allSubProcesses?.value !== undefined) {
                allSubProcesses.value = [];
            }

            if (clearAuditProcess) {
                clearAuditProcess();
            }
        } catch (error) {
            console.error('Error resetting to default view:', error);
        }
    };

    return {
        refreshData,
        handleSubProcessAdded,
        handleSubProcessChange,
        resetToDefaultView
    };
}