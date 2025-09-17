import { ref, onMounted, nextTick } from 'vue';

export function useDepartmentLifecycle({
    selectedSubLocationId,
    currentPage,
    pageSize,
    searchDepartments,
    subLocationsRef,
    searchQuery,
    getDepartmentsByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedSubLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchDepartments) {
                await searchDepartments({
                    subLocationId: selectedSubLocationId.value,
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

        if (selectedSubLocationId.value && !isInitialized.value && !isInitializing.value) {
            if (subLocationsRef.value?.length > 0) {
                const subLocationExists = subLocationsRef.value.some(
                    sub => sub.id === selectedSubLocationId.value);
                if (subLocationExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedSubLocationId.value 
            && !searchQuery.value.trim() 
            && getDepartmentsByUserId) {
                await getDepartmentsByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}