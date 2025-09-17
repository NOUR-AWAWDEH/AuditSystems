import { ref, onMounted, nextTick } from 'vue';

export function useSubDepartmentLifecycle({
    selectedDepartmentId,
    currentPage,
    pageSize,
    searchSubDepartments,
    departmentsRef,
    searchQuery,
    getSubDepartmentsByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedDepartmentId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchSubDepartments) {
                await searchSubDepartments({
                    departmentId: selectedDepartmentId.value,
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

        if (selectedDepartmentId.value && !isInitialized.value && !isInitializing.value) {
            if (departmentsRef.value?.length > 0) {
                const departmentExists = departmentsRef.value.some(
                    sub => sub.id === selectedDepartmentId.value);
                if (departmentExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedDepartmentId.value 
            && !searchQuery.value.trim() 
            && getSubDepartmentsByUserId) {
                await getSubDepartmentsByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}