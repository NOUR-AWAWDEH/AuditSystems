import { watch, nextTick } from 'vue';

export function useObjectiveWatchers({
    riskControlMatricesRef,
    selectedRiskControlMatrix,
    selectedRiskControlMatrixId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchObjectives,
    clearSearch,
    getObjectivesByUserId,
    allObjectives,
    clearRiskControlMatrix,
    getRiskControlMatrixById,
    initializeData,
    resetToDefaultView
}) {
  
    // Watch Risk Control Matrix
    watch(riskControlMatricesRef, async (newRiskControlMatrices) => {
        if (!newRiskControlMatrices?.length || !selectedRiskControlMatrix.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const riskControlMatrixExists = newRiskControlMatrices.some(
          matrix => matrix.id === selectedRiskControlMatrixId.value
        );
        if (riskControlMatrixExists) {
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
                if (searchObjectives) {
                    await searchObjectives({
                        searchTerm: search,
                        riskControlMatrixId: selectedRiskControlMatrixId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Clear search results
                if (clearSearch) {
                    clearSearch();
                }

                // Reload risk control matrices based on current state
                if (selectedRiskControlMatrixId.value) {
                    // If a risk control matrix is selected, reload risk control matrices for that risk control matrix
                    if (searchObjectives) {
                        await searchObjectives({
                            searchTerm: '',
                            riskControlMatrixId: selectedRiskControlMatrixId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no risk control matrix is selected, load all risk control matrices
                    if (getObjectivesByUserId) {
                        await getObjectivesByUserId();
                    }
                }
            }
    });

    // Watch selected risk control matrix
    watch(selectedRiskControlMatrixId, async (newRiskControlMatrixId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allObjectives?.value !== undefined) {
            allObjectives.value = [];
        }

        if (!newRiskControlMatrixId) {
            if (clearRiskControlMatrix) {
                clearRiskControlMatrix();
            }
        }

        if (newRiskControlMatrixId) {
            try {
                await getRiskControlMatrixById(newRiskControlMatrixId);
            } catch (error) {
                console.error('[RiskControlMatrix] Error fetching risk control matrix:', error);
            }
        }

        if (!newRiskControlMatrixId) {
            if (!searchQuery.value.trim() && getObjectivesByUserId) {
                getObjectivesByUserId();
            }
            return;
        }

        if (newRiskControlMatrixId) {
            await searchObjectives({
                searchTerm: searchQuery.value || '',
                riskControlMatrixId: newRiskControlMatrixId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}
