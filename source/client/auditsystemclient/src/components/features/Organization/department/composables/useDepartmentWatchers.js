import { watch, nextTick } from 'vue';

export function useDepartmentWatchers({
  subLocationsRef = null,
  selectedSubLocation,
  selectedSubLocationId,
  searchQuery,
  currentPage,
  pageSize,
  isInitialized,
  isInitializing,
  searchDepartments,
  clearSearch,
  getDepartmentsByUserId,
  allDepartments,
  clearSubLocation,
  getSubLocationById,
  initializeData,
  resetToDefaultView
}) {
  // Watch SubLocations
  if (subLocationsRef) {
    watch(subLocationsRef, async (newSubLocations) => {

      if (!newSubLocations?.length || !selectedSubLocation.value || isInitialized.value || isInitializing.value) {
        return;
      }

      const subLocationExists = newSubLocations.some(sub => sub.id === selectedSubLocationId.value);
      if (subLocationExists) {
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
          subLocationId: selectedSubLocationId.value,
          page: currentPage.value,
          pageSize: pageSize.value,
        });
      }
    } else {
      clearSearch?.();

      if (selectedSubLocationId.value) {
        if (searchDepartments) {
          await searchDepartments({
            searchTerm: '',
            subLocationId: selectedSubLocationId.value,
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
  watch(selectedSubLocationId, async (newSubLocationId) => {

    clearSearch?.();

    if (allDepartments?.value !== undefined) {
      allDepartments.value = [];
    }

    if (!newSubLocationId) {
      clearSubLocation?.();
      if (!searchQuery.value.trim()) {
        await getDepartmentsByUserId?.();
      }
      return;
    }

    try {
      await getSubLocationById?.(newSubLocationId);
    } catch (error) {
      console.error('[Process] Error fetching SubLocation:', error);
    }

    await searchDepartments?.({
      searchTerm: searchQuery.value || '',
      subLocationId: newSubLocationId,
      page: currentPage.value,
      pageSize: pageSize.value,
    });
  });
}