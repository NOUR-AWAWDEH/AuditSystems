import { watch, nextTick } from 'vue';

export function useSubProcessWatchers({
    auditProcessesRef,
    selectedAuditProcess,
    selectedAuditProcessId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchSubProcesses,
    clearSearch,
    getSubProcessesByUserId,
    allSubProcesses,
    clearAuditProcess,
    getAuditProcessById,
    initializeData,
    resetToDefaultView
}) {
    // Watch Audit Process
    watch(auditProcessesRef, async (newAuditProcesses) => {
        if (!newAuditProcesses?.length || !selectedAuditProcess.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const auditProcessExists = newAuditProcesses.some(sub => sub.id === selectedAuditProcessId.value);
        if (auditProcessExists) {
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
                if (searchSubProcesses) {
                    await searchSubProcesses({
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
                
                // Reload audit processes based on current state
                if (selectedAuditProcessId.value) {
                    // If a audit process is selected, reload audit processes for that audit process
                    if (searchSubProcesses) {
                        await searchSubProcesses({
                            searchTerm: '',
                            auditProcessId: selectedAuditProcessId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no audit process is selected, load all audit processes
                    if (getSubProcessesByUserId) {
                        await getSubProcessesByUserId();
                    }
                }
            }
    });

    // Watch selected audit process
    watch(selectedAuditProcessId, async (newAuditProcessId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allSubProcesses?.value !== undefined) {
            allSubProcesses.value = [];
        }

        if (!newAuditProcessId) {
            if (clearAuditProcess) {
                clearAuditProcess();
            }
        }
        
        if (newAuditProcessId) {
            try {
                await getAuditProcessById(newAuditProcessId);
            } catch (error) {
                console.error('[Process] Error fetching audit process:', error);
            }
        }

        if (!newAuditProcessId) {
            if (!searchQuery.value.trim() && getSubProcessesByUserId) {
                getSubProcessesByUserId();
            }
            return;
        }

        if (newAuditProcessId) {
            await searchSubProcesses({
                searchTerm: searchQuery.value || '',
                auditProcessId: newAuditProcessId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}