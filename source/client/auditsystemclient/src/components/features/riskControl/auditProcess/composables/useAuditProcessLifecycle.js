import { ref, watch, onMounted } from 'vue';

export function useAuditProcessLifecycle({
    selectedSubDepartmentId,
    searchAuditProcesses,
    currentPage,
    pageSize,
    subDepartmentsRef,
    getAuditProcessesByUserId
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    // Fetch all audit processes on component mount
    const initializeAllAuditProcesses = async () => {
        if (isInitialized.value || isInitializing.value) return;

        try {
            isInitializing.value = true;

            // Fetch all audit processes when no sub-department is selected
            if (typeof getAuditProcessesByUserId === 'function' && !selectedSubDepartmentId.value) {
                await getAuditProcessesByUserId();
            }

            isInitialized.value = true;
        } catch (err) {
            console.error('Error initializing audit processes:', err);
        } finally {
            isInitializing.value = false;
        }
    };

    // Fetch data for specific sub-department
    const initializeSubDepartmentData = async () => {
        if (!selectedSubDepartmentId.value || isInitializing.value) return;

        try {
            isInitializing.value = true;
            if (typeof searchAuditProcesses === 'function') {
                await searchAuditProcesses({
                    subDepartmentId: selectedSubDepartmentId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
        } catch (err) {
            console.error('Error initializing sub-department audit processes:', err);
        } finally {
            isInitializing.value = false;
        }
    };

    // Immediate initialization without nextTick delay
    onMounted(async () => {
        await initializeAllAuditProcesses();
    });

    watch(subDepartmentsRef, async (subs) => {
        if (!subs?.length) {
            // If no sub-departments, ensure we at least try to fetch all audit processes
            if (!isInitialized.value && !isInitializing.value) {
                await initializeAllAuditProcesses();
            }
            return;
        }

        // Set default selectedSubDepartmentId if empty
        if (!selectedSubDepartmentId.value) {
            selectedSubDepartmentId.value = subs[0].id;
        }

        await initializeSubDepartmentData();
    }, { immediate: true, deep: true });

    return { isInitialized, isInitializing, initializeData: initializeSubDepartmentData };
}
