import { watch, nextTick } from 'vue';

export function useDepartmentWatchers({
  locationsRef = null,
  selectedLocation,
  selectedLocationId,
  searchQuery,
  currentPage,
  pageSize,
  isInitialized,
  isInitializing,
  searchDepartments,
  clearSearch,
  getDepartmentsByUserId,
  allDepartments,
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

      const locationExists = newLocations.some(loc => loc.id === selectedLocationId.value);
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
      if (searchDepartments) {
        await searchDepartments({
          searchTerm: search,
          locationId: selectedLocationId.value,
          page: currentPage.value,
          pageSize: pageSize.value,
        });
      }
    } else {
      clearSearch?.();

      if (selectedLocationId.value) {
        if (searchDepartments) {
          await searchDepartments({
            searchTerm: '',
            locationId: selectedLocationId.value,
            page: currentPage.value,
            pageSize: pageSize.value,
          });
        }
      } else {
        await getDepartmentsByUserId?.();
      }
    }
  });

  // Watch selected sub department
  watch(selectedLocationId, async (newLocationId) => {

    clearSearch?.();

    if (allDepartments?.value !== undefined) {
      allDepartments.value = [];
    }

    if (!newLocationId) {
      clearLocation?.();
      if (!searchQuery.value.trim()) {
        await getDepartmentsByUserId?.();
      }
      return;
    }

    try {
      await getLocationById?.(newLocationId);
    } catch (error) {
      console.error('[Process] Error fetching location:', error);
    }

    await searchDepartments?.({
      searchTerm: searchQuery.value || '',
      locationId: newLocationId,
      page: currentPage.value,
      pageSize: pageSize.value,
    });
  });
}