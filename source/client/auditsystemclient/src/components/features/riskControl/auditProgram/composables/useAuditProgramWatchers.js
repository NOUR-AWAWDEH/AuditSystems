import { watch, nextTick } from 'vue';

export function useAuditProgramWatchers({
    exitControlsRef,
    selectedExitControl,
    selectedExitControlId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchAuditPrograms,
    clearSearch,
    getAuditProgramsByUserId,
    allAuditPrograms,
    clearExitControl,
    getExitControlById,
    initializeData,
    resetToDefaultView
}) {
   
    // Watch ExitControl
    watch(exitControlsRef, async (newExitControls) => {
        if (!newExitControls?.length || !selectedExitControl.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const exitControlExists = newExitControls.some(
            exitControl => exitControl.id === selectedExitControlId.value
        );
        if (exitControlExists) {
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
                if (searchAuditPrograms) {
                    await searchAuditPrograms({
                        searchTerm: search,
                        exitControlId: selectedExitControlId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Clear search results
                if (clearSearch) {
                    clearSearch();
                }
                
                // Reload ExitControl based on current state
                if (selectedExitControlId.value) {
                    // If a ExitControl is selected, reload ExitControl for that ExitControl
                    if (searchAuditPrograms) {
                        await searchAuditPrograms({
                            searchTerm: '',
                            exitControlId: selectedExitControlId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no ExitControl is selected, load all ExitControls
                    if (getAuditProgramsByUserId) {
                        await getAuditProgramsByUserId();
                    }
                }
            }
    });

    // Watch selected ExitControl
    watch(selectedExitControlId, async (newExitControlId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allAuditPrograms?.value !== undefined) {
            allAuditPrograms.value = [];
        }

        if (!newExitControlId) {
            if (clearExitControl) {
                clearExitControl();
            }
        }
        
        if (!newExitControlId) {
            try {
                await getExitControlById(newExitControlId);
            } catch (error) {
                console.error('[ExitControl] Error fetching ExitControl:', error);
            }
        }

        if (!newExitControlId) {
            if (!searchQuery.value.trim() && getAuditProgramsByUserId) {
                getAuditProgramsByUserId();
            }
            return;
        }

        if (newExitControlId) {
            await searchAuditPrograms({
                searchTerm: searchQuery.value || '',
                exitControlId: newExitControlId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}