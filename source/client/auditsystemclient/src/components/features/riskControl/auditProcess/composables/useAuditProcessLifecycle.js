import { ref, onMounted, nextTick } from 'vue';

export function useAuditProcessLifecycle({
    selectedSubDepartmentId,
    currentPage,
    pageSize,
    searchAuditProcesses,
    subDepartmentsRef,
    searchQuery,
    getAuditProcessesByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedSubDepartmentId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchAuditProcesses) {
                await searchAuditProcesses({
                    subDepartmentId: selectedSubDepartmentId.value,
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

        if (selectedSubDepartmentId.value && !isInitialized.value && !isInitializing.value) {
            if (subDepartmentsRef.value?.length > 0) {
                const subDepartmentExists = subDepartmentsRef.value.some(sub => sub.id === selectedSubDepartmentId.value);
                if (subDepartmentExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedSubDepartmentId.value && !searchQuery.value.trim() && getAuditProcessesByUserId) {
            await getAuditProcessesByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}