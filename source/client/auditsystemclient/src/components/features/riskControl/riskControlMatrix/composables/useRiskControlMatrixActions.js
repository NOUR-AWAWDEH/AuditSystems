export function useRiskControlMatrixActions({
    searchRiskControlMatrices,
    getRiskControlMatricesByUserId,
    clearSearch,
    clearAuditProcess,
    allRiskControlMatrices,
    searchQuery,
    selectedAuditProcessId,
    currentPage,
    pageSize
}) {

    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedAuditProcessId.value;

            if (hasSearchOrFilter) {
                if (searchRiskControlMatrices) {
                    await searchRiskControlMatrices({
                        searchTerm: searchQuery.value || '',
                        auditProcessId: selectedAuditProcessId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getRiskControlMatricesByUserId) {
                    await getRiskControlMatricesByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing risk control matrix data:', error);
        }
    };

    const handleRiskControlMatrixAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling risk control matrix addition:', error);
        }
    };

    const handleRiskControlMatrixChange = async (newAuditProcessId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allRiskControlMatrices?.value !== undefined) {
                allRiskControlMatrices.value = [];
            }

            selectedAuditProcessId.value = newAuditProcessId;
        } catch (error) {
            console.error('Error handling risk control matrix change:', error);
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

            if (allRiskControlMatrices?.value !== undefined) {
                allRiskControlMatrices.value = [];
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
        handleRiskControlMatrixAdded,
        handleRiskControlMatrixChange,
        resetToDefaultView
    };
}