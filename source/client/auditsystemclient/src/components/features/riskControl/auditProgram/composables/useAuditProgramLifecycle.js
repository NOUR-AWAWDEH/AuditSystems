import { ref, onMounted, nextTick } from 'vue';

export function useAuditProgramLifecycle({
    selectedExitControlId,
    currentPage,
    pageSize,
    searchAuditPrograms,
    exitControlsRef,
    searchQuery,
    getAuditProgramsByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedExitControlId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchAuditPrograms) {
                await searchAuditPrograms({
                    exitControlId: selectedExitControlId.value,
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

        if (selectedExitControlId.value && !isInitialized.value && !isInitializing.value) {
            if (exitControlsRef.value?.length > 0) {
                const exitControlExists = exitControlsRef.value.some(
                    exitControl => exitControl.id === selectedExitControlId.value);
                if (exitControlExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedExitControlId.value 
            && !searchQuery.value.trim() 
            && getAuditProgramsByUserId) {
                await getAuditProgramsByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}