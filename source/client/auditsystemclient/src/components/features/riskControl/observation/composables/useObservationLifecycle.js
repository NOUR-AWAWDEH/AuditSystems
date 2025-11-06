import { ref, onMounted, nextTick } from 'vue';

export function useObservationLifecycle({
    selectedAuditProgramId,
    currentPage,
    pageSize,
    searchObservations,
    auditProgramsRef,
    searchQuery,
    getObservationsByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedAuditProgramId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchObservations) {
                await searchObservations({
                    auditProgramId: selectedAuditProgramId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
            isInitialized.value = true;
        } catch (error) {
            console.error('Error initializing data:', error);
        } finally {
            isInitializing.value = false;
        }
    };

    onMounted(async () => {
        await nextTick();

        if (selectedAuditProgramId.value && !isInitialized.value && !isInitializing.value) {
            if (auditProgramsRef.value?.length > 0) {
                const auditProgramExists = auditProgramsRef.value.some(
                    auditProgram => auditProgram.id === selectedAuditProgramId.value);
                if (auditProgramExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedAuditProgramId.value 
            && !searchQuery.value.trim() 
            && getObservationsByUserId) {
                await getObservationsByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}