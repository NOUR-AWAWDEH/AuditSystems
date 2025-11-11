export function useRiskControlMatrixActions({
    searchRiskControlMatrices,
    getRiskControlMatricesByUserId,
    clearSearch,
    clearSubProcess,
    allRiskControlMatrices,
    searchQuery,
    selectedSubProcessId,
    currentPage,
    pageSize
}) {

    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedSubProcessId.value;

            if (hasSearchOrFilter) {
                if (searchRiskControlMatrices) {
                    await searchRiskControlMatrices({
                        searchTerm: searchQuery.value || '',
                        subProcessId: selectedSubProcessId.value,
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

    const handleRiskControlMatrixChange = async (newSubProcessId) => {
        try {
            if (clearSearch) {
                clearSearch();
            }

            if (allRiskControlMatrices?.value !== undefined) {
                allRiskControlMatrices.value = [];
            }

            selectedSubProcessId.value = newSubProcessId;
        } catch (error) {
            console.error('Error handling risk control matrix change:', error);
        }
    };

    const resetToDefaultView = () => {
        try {
            selectedSubProcessId.value = null;
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allRiskControlMatrices?.value !== undefined) {
                allRiskControlMatrices.value = [];
            }

            if (clearSubProcess) {
                clearSubProcess();
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