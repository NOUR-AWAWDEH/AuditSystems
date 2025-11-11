import { ref, onMounted, nextTick } from 'vue';

export function useDepartmentLifecycle({
    selectedLocationId,
    currentPage,
    pageSize,
    searchDepartments,
    locationsRef,
    searchQuery,
    getDepartmentsByUserId,
    resetToDefaultView
}) {
    const isInitialized = ref(false);
    const isInitializing = ref(false);

    const initializeData = async () => {
        if (!selectedLocationId.value || isInitialized.value || isInitializing.value) {
            return;
        }

        try {
            isInitializing.value = true;
            if (searchDepartments) {
                await searchDepartments({
                    locationId: selectedLocationId.value,
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

        if (selectedLocationId.value && !isInitialized.value && !isInitializing.value) {
            if (locationsRef.value?.length > 0) {
                const locationExists = locationsRef.value.some(
                    loc => loc.id === selectedLocationId.value);
                if (locationExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedLocationId.value 
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