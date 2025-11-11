import { watch, nextTick } from 'vue';

export function useRiskControlMatrixWatchers({
    subProcessesRef,
    selectedSubProcess,
    selectedSubProcessId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchRiskControlMatrices,
    clearSearch,
    getRiskControlMatricesByUserId,
    allRiskControlMatrices,
    clearSubProcess,
    getSubProcessById,
    initializeData,
    resetToDefaultView
}) {
    // Watch Sub Process
    watch(subProcessesRef, async (newSubProcesses) => {
        if (!newSubProcesses?.length || !selectedSubProcess.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const subProcessExists = newSubProcesses.some(sub => sub.id === selectedSubProcessId.value);
        if (subProcessExists) {
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
                if (searchRiskControlMatrices) {
                    await searchRiskControlMatrices({
                        searchTerm: search,
                        subProcessId: selectedSubProcessId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Clear search results
                if (clearSearch) {
                    clearSearch();
                }
                
                // Reload sub processes based on current state
                if (selectedSubProcessId.value) {
                    // If a sub process is selected, reload sub processes for that sub process
                    if (searchRiskControlMatrices) {
                        await searchRiskControlMatrices({
                            searchTerm: '',
                            subProcessId: selectedSubProcessId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no sub process is selected, load all sub processes
                    if (getRiskControlMatricesByUserId) {
                        await getRiskControlMatricesByUserId();
                    }
                }
            }
    });

    // Watch selected sub process
    watch(selectedSubProcessId, async (newSubProcessId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allRiskControlMatrices?.value !== undefined) {
            allRiskControlMatrices.value = [];
        }

        if (!newSubProcessId) {
            if (clearSubProcess) {
                clearSubProcess();
            }
        }
        
        if (newSubProcessId) {
            try {
                await getSubProcessById(newSubProcessId);
            } catch (error) {
                console.error('[SubProcess] Error fetching sub process:', error);
            }
        }

        if (!newSubProcessId) {
            if (!searchQuery.value.trim() && getRiskControlMatricesByUserId) {
                getRiskControlMatricesByUserId();
            }
            return;
        }

        if (newSubProcessId) {
            await searchRiskControlMatrices({
                searchTerm: searchQuery.value || '',
                subProcessId: newSubProcessId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}