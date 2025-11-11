import { watch, nextTick } from 'vue';

export function useExitControlWatchers({
    objectivesRef,
    selectedObjective,
    selectedObjectiveId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchExitControls,
    clearSearch,
    getExitControlsByUserId,
    allExitControls,
    clearObjective,
    getObjectiveById,
    initializeData,
    resetToDefaultView
}) {
   
    // Watch Objective
    watch(objectivesRef, async (newObjectives) => {
        if (!newObjectives?.length || !selectedObjective.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const objectiveExists = newObjectives.some(
            objective => objective.id === selectedObjectiveId.value
        );
        if (objectiveExists) {
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
                        objectiveId: selectedObjectiveId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Clear search results
                if (clearSearch) {
                    clearSearch();
                }
                
                // Reload objective based on current state
                if (selectedObjectiveId.value) {
                    // If a objective is selected, reload objective for that objective
                    if (searchExitControls) {
                        await searchExitControls({
                            searchTerm: '',
                            objectiveId: selectedObjectiveId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no objective is selected, load all objectives
                    if (getExitControlsByUserId) {
                        await getExitControlsByUserId();
                    }
                }
            }
    });

    // Watch selected objective
    watch(selectedObjectiveId, async (newObjectiveId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allExitControls?.value !== undefined) {
            allExitControls.value = [];
        }

        if (!newObjectiveId) {
            if (clearObjective) {
                clearObjective();
            }
        }
        
        if (!newObjectiveId) {
            try {
                await getObjectiveById(newObjectiveId);
            } catch (error) {
                console.error('[Objective] Error fetching objective:', error);
            }
        }

        if (!newObjectiveId) {
            if (!searchQuery.value.trim() && getExitControlsByUserId) {
                getExitControlsByUserId();
            }
            return;
        }

        if (newObjectiveId) {
            await searchExitControls({
                searchTerm: searchQuery.value || '',
                objectiveId: newObjectiveId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}