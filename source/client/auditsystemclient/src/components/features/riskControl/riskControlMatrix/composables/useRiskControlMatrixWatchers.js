import { watch } from 'vue';

export function useRiskControlMatrixWatchers({
    selectedAuditProcessId,
    searchQuery,
    currentPage,
    pageSize,
    searchRiskControlMatrices,
    clearSearch,
    getRiskControlMatricesByUserId,
    allRiskControlMatrices,
    clearAuditProcess,
}) {
    // Watch Sub Process - Removed

    // Watch search query
    watch(searchQuery, async (search) => {
            if (search.trim()) {
                // Perform search when there's a search term
                if (searchRiskControlMatrices) {
                    await searchRiskControlMatrices({
                        searchTerm: search,
                        auditProcessId: selectedAuditProcessId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Clear search results
                if (clearSearch) {
                    clearSearch();
                }
                
                // Reload based on current state
                if (selectedAuditProcessId.value) {
                    // If an audit process is selected, reload 
                    if (searchRiskControlMatrices) {
                        await searchRiskControlMatrices({
                            searchTerm: '',
                            auditProcessId: selectedAuditProcessId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no process is selected, load all
                    if (getRiskControlMatricesByUserId) {
                        await getRiskControlMatricesByUserId();
                    }
                }
            }
    });

    // Watch selected audit process
    watch(selectedAuditProcessId, async (newAuditProcessId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allRiskControlMatrices?.value !== undefined) {
            allRiskControlMatrices.value = [];
        }

        if (!newAuditProcessId) {
            if (clearAuditProcess) {
                clearAuditProcess();
            }
        }
        
        // Remove getSubProcessById logic as we don't need to fetch extra process details separately usually, or if we do, it should be getAuditProcessById

        if (!newAuditProcessId) {
            if (!searchQuery.value.trim() && getRiskControlMatricesByUserId) {
                getRiskControlMatricesByUserId();
            }
            return;
        }

        if (newAuditProcessId) {
            await searchRiskControlMatrices({
                searchTerm: searchQuery.value || '',
                auditProcessId: newAuditProcessId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}