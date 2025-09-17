import { watch, nextTick } from 'vue';

export function useSubLocationWatchers({
  locationsRef = null,
  selectedLocation,
  selectedLocationId,
  searchQuery,
  currentPage,
  pageSize,
  isInitialized,
  isInitializing,
  searchSubLocations,
  clearSearch,
  getSubLocationsByUserId,
  allSubLocations,
  clearLocation,
  getLocationById,
  initializeData,
  resetToDefaultView
}) {
  // Watch locations
  if (locationsRef) {
    watch(locationsRef, async (newLocations) => {

      if (!newLocations?.length || !selectedLocation.value || isInitialized.value || isInitializing.value) {
        return;
      }

      const locationExists = newLocations.some(sub => sub.id === selectedLocationId.value);
      if (locationExists) {
        await nextTick();
        await initializeData?.();
      } else {
        resetToDefaultView?.();
      }
    }, { immediate: true });
  }

  // Watch search query
  watch(searchQuery, async (search) => {

    if (search.trim()) {
      if (searchSubLocations) {
        await searchSubLocations({
          searchTerm: search,
          locationId: selectedLocationId.value,
          page: currentPage.value,
          pageSize: pageSize.value,
        });
      }
    } else {
      clearSearch?.();

      if (selectedLocationId.value) {
        if (searchSubLocations) {
          await searchSubLocations({
            searchTerm: '',
            locationId: selectedLocationId.value,
            page: currentPage.value,
            pageSize: pageSize.value,
          });
        }
      } else {
        await getSubLocationsByUserId?.();
      }
    }
  });

  // Watch selected sub department
  watch(selectedLocationId, async (newLocationId) => {

    clearSearch?.();

    if (allSubLocations?.value !== undefined) {
      allSubLocations.value = [];
    }

    if (!newLocationId) {
      clearLocation?.();
      if (!searchQuery.value.trim()) {
        await getSubLocationsByUserId?.();
      }
      return;
    }

    try {
      await getLocationById?.(newLocationId);
    } catch (error) {
      console.error('[Process] Error fetching location:', error);
    }

    await searchSubLocations?.({
      searchTerm: searchQuery.value || '',
      locationId: newLocationId,
      page: currentPage.value,
      pageSize: pageSize.value,
    });
  });
}