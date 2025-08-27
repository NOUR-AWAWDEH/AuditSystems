import { ref, onMounted, nextTick } from 'vue';

export function useSubLocationLifecycle({
    selectedLocationId,
    currentPage,
    pageSize,
    searchSubLocations,
    locationsRef,
    searchQuery,
    getSubLocationsByUserId,
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
            if (searchSubLocations) {
                await searchSubLocations({
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
                    sub => sub.id === selectedLocationId.value);
                if (locationExists) {
                    await initializeData();
                } else {
                    resetToDefaultView();
                }
            }
        } else if (!selectedLocationId.value 
            && !searchQuery.value.trim() 
            && getSubLocationsByUserId) {
                await getSubLocationsByUserId();
        }
    });

    return {
        isInitialized,
        isInitializing,
        initializeData
    };
}