import { watch, nextTick } from 'vue';

export function useControlDesignWatchers({
    exitControlsRef,
    selectedExitControl,
    selectedExitControlId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchControlDesigns,
    clearSearch,
    getControlDesignsByUserId,
    allControlDesigns,
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
                if (searchControlDesigns) {
                    await searchControlDesigns({
                        searchTerm: search,
                        controlDesignId: selectedExitControlId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Clear search results
                if (clearSearch) {
                    clearSearch();
                }
                
                // Reload Exit Control based on current state
                if (selectedExitControlId.value) {
                    // If a Exit Control is selected, reload Exit Control for that Exit Control
                    if (searchControlDesigns) {
                        await searchControlDesigns({
                            searchTerm: '',
                            exitControlId: selectedExitControlId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no Exit Control is selected, load all Control Designs
                    if (getControlDesignsByUserId) {
                        await getControlDesignsByUserId();
                    }
                }
            }
    });

    // Watch selected Exit Control
    watch(selectedExitControlId, async (newExitControlId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allControlDesigns?.value !== undefined) {
            allControlDesigns.value = [];
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
                console.error('[Exit Control] Error fetching exit control:', error);
            }
        }

        if (!newExitControlId) {
            if (!searchQuery.value.trim() && getControlDesignsByUserId) {
                getControlDesignsByUserId();
            }
            return;
        }

        if (newExitControlId) {
            await searchControlDesigns({
                searchTerm: searchQuery.value || '',
                exitControlId: newExitControlId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}