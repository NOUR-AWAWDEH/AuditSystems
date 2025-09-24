export function useAuditProgramActions({
    searchAuditPrograms,
    getAuditProgramsByUserId,
    clearSearch,
    clearExitControl,
    allAuditPrograms,
    searchQuery,
    selectedExitControlId,
    currentPage,
    pageSize
}) {

    const refreshData = async () => {
        try {
            const hasSearchOrFilter = searchQuery.value.trim() || selectedExitControlId.value;

            if (hasSearchOrFilter) {
                if (searchAuditPrograms) {
                    await searchAuditPrograms({
                        searchTerm: searchQuery.value || '',
                        exitControlId: selectedExitControlId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                if (getAuditProgramsByUserId) {
                    await getAuditProgramsByUserId();
                }
            }
            
        } catch (error) {
            console.error('Error refreshing Audit Program data:', error);
        }
    };

    const handleAuditProgramAdded = async () => {
        try {
            await refreshData();
        } catch (error) {
            console.error('Error handling Audit Program addition:', error);
        }
    };

    const handleAuditProgramChange = async (newExitControlId) => {
        try { 
            if (clearSearch) {
                clearSearch();
            }

            // Clear allAuditPrograms if it's being used for display when no filter is applied
            if (allAuditPrograms?.value !== undefined) {
                allAuditPrograms.value = [];
            }

            selectedExitControlId.value = newExitControlId;                   
            
            // IMPORTANT: Trigger the search immediately after changing the selectedExitControlId
            if (searchAuditPrograms) {
                await searchAuditPrograms({
                    searchTerm: searchQuery.value || '',
                    exitControlId: newExitControlId,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }

        } catch (error) {
            console.error('Error handling Audit Program change:', error);
        }
    };

    const resetToDefaultView = () => {
        try {
            selectedExitControlId.value = ''; // Changed from null to empty string for consistency
            searchQuery.value = '';
            currentPage.value = 1;

            if (clearSearch) {
                clearSearch();
            }

            if (allAuditPrograms?.value !== undefined) {
                allAuditPrograms.value = [];
            }

            if (clearExitControl) {
                clearExitControl();
            }
        } catch (error) {
            console.error('Error resetting to default view:', error);
        }
    };

    return {
        refreshData,
        handleAuditProgramAdded,    
        handleAuditProgramChange,
        resetToDefaultView
    };
}