import { ref, onMounted, nextTick } from 'vue';

export function useSubProcessLifecycle({
    selectedAuditProcessId,
    currentPage,
    pageSize,
    searchSubProcesses,
    auditProcessesRef,
    searchQuery,
    getSubProcessesByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedAuditProcessId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchSubProcesses) {
                await searchSubProcesses({
                    auditProcessId: selectedAuditProcessId.value,
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

        if (selectedAuditProcessId.value && !isInitialized.value && !isInitializing.value) {
            if (auditProcessesRef.value?.length > 0) {
                const auditProcessExists = auditProcessesRef.value.some(
                    auditProcess => auditProcess.id === selectedAuditProcessId.value);
                if (auditProcessExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedAuditProcessId.value 
            && !searchQuery.value.trim() 
            && getSubProcessesByUserId) {
                await getSubProcessesByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}