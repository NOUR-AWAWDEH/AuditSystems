import { watch, nextTick } from 'vue';

export function useExitControlWatchers({
    risksRef,
    selectedRisk,
    selectedRiskId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchExitControls,
    clearSearch,
    getExitControlsByUserId,
    allExitControls,
    clearRisk,
    getExitControlById,
    initializeData,
    resetToDefaultView
}) {
    // Watch Risk
    watch(risksRef, async (newRisks) => {
        if (!newRisks?.length || !selectedRisk.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const riskExists = newRisks.some(risk => risk.id === selectedRiskId.value);
        if (riskExists) {
            await nextTick();
            await initializeData();
        } else {
            resetToDefaultView();
        }
    }, { immediate: true });

    // Watch search query
    watch(searchQuery, async (search) => {
            if (search.trim()) {
                // Perform search when there's a search term
                if (searchExitControls) {
                    await searchExitControls({
                        searchTerm: search,
                        riskId: selectedRiskId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Clear search results
                if (clearSearch) {
                    clearSearch();
                }
                
                // Reload risk based on current state
                if (selectedRiskId.value) {
                    // If a risk is selected, reload risk for that risk
                    if (searchExitControls) {
                        await searchExitControls({
                            searchTerm: '',
                            riskId: selectedRiskId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no risk is selected, load all risks
                    if (getExitControlsByUserId) {
                        await getExitControlsByUserId();
                    }
                }
            }
    });

    // Watch selected risk
    watch(selectedRiskId, async (newRiskId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (risksRef.value !== undefined) {
            risksRef.value = [];
        }

        if (!newRiskId) {
            if (clearRisk) {
                clearRisk();
            }
        }
        
        if (newRiskId) {
            try {
                await getExitControlById(newRiskId);
            } catch (error) {
                console.error('[ExitControl] Error fetching exit control:', error);
            }
        }

        if (!newRiskId) {
            if (!searchQuery.value.trim() && getExitControlsByUserId) {
                getExitControlsByUserId();
            }
            return;
        }

        if (newRiskId) {
            await searchExitControls({
                searchTerm: searchQuery.value || '',
                riskId: newRiskId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}