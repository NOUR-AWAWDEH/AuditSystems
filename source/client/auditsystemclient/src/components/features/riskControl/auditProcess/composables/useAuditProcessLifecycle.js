import { ref, watch } from 'vue';

export function useAuditProcessLifecycle({
    selectedSubDepartmentId,
    searchAuditProcesses,
    currentPage,
    pageSize,
    subDepartmentsRef
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedSubDepartmentId.value || isInitialized.value || isInitializing.value) return;

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
        } catch (err) {
            console.error('Error initializing audit processes:', err);
        } finally {
            isInitializing.value = false;
        }
    };

    watch(subDepartmentsRef, async (subs) => {
        if (!subs?.length) return;

        // set default selectedSubDepartmentId if empty
        if (!selectedSubDepartmentId.value) {
            selectedSubDepartmentId.value = subs[0].id;
        }

        await initializeData();
    }, { immediate: true, deep: true });

    return { isInitialized, isInitializing, initializeData };
}