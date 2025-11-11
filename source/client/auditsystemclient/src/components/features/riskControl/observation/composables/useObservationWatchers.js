import { watch, nextTick } from 'vue';

export function useObservationWatchers({
    auditProgramsRef,
    selectedAuditProgram,
    selectedAuditProgramId,
    searchQuery,
    currentPage,
    pageSize,
    isInitialized,
    isInitializing,
    searchObservations,
    clearSearch,
    getObservationsByUserId,
    allObservations,
    clearAuditProgram,
    getAuditProgramById,
    initializeData,
    resetToDefaultView
}) {
    // Watch Audit Program
    watch(auditProgramsRef, async (newAuditPrograms) => {
        if (!newAuditPrograms?.length || !selectedAuditProgram.value || isInitialized.value || isInitializing.value) {
            return;
        }

        const auditProgramExists = newAuditPrograms.some(obs => obs.id === selectedAuditProgramId.value);
        if (auditProgramExists) {
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
                if (searchObservations) {
                    await searchObservations({
                        searchTerm: search,
                        auditProgramId: selectedAuditProgramId.value,
                        page: currentPage.value,
                        pageSize: pageSize.value,
                    });
                }
            } else {
                // Clear search results
                if (clearSearch) {
                    clearSearch();
                }
                
                // Reload audit programs based on current state
                if (selectedAuditProgramId.value) {
                    // If a audit program is selected, reload audit programs for that audit program
                    if (searchObservations) {
                        await searchObservations({
                            searchTerm: '',
                            auditProgramId: selectedAuditProgramId.value,
                            page: currentPage.value,
                            pageSize: pageSize.value,
                        });
                    }
                } else {
                    // If no audit program is selected, load all audit programs
                    if (getObservationsByUserId) {
                        await getObservationsByUserId();
                    }
                }
            }
    });

    // Watch selected audit program
    watch(selectedAuditProgramId, async (newAuditProgramId) => {
        if (clearSearch) {
            clearSearch();
        }

        if (allObservations?.value !== undefined) {
            allObservations.value = [];
        }

        if (!newAuditProgramId) {
            if (clearAuditProgram) {
                clearAuditProgram();
            }
        }
        
        if (newAuditProgramId) {
            try {
                await getAuditProgramById(newAuditProgramId);
            } catch (error) {
                console.error('[Program] Error fetching audit program:', error);
            }
        }

        if (!newAuditProgramId) {
            if (!searchQuery.value.trim() && getObservationsByUserId) {
                getObservationsByUserId();
            }
            return;
        }

        if (newAuditProgramId) {
            await searchObservations({
                searchTerm: searchQuery.value || '',
                auditProgramId: newAuditProgramId,
                page: currentPage.value,
                pageSize: pageSize.value,
            });
        }
    });
}