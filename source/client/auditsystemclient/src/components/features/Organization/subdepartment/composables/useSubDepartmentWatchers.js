import { watch, nextTick } from 'vue';

export function useSubDepartmentWatchers({
  departmentsRef = null,
  selectedDepartment,
  selectedDepartmentId,
  searchQuery,
  currentPage,
  pageSize,
  isInitialized,
  isInitializing,
  searchSubDepartments,
  clearSearch,
  getSubDepartmentsByUserId,
  allSubDepartments,
  clearDepartment,
  getDepartmentById,
  initializeData,
  resetToDefaultView
}) {
  // Watch Departments
  if (departmentsRef) {
    watch(departmentsRef, async (newDepartments) => {

      if (!newDepartments?.length || !selectedDepartment.value || isInitialized.value || isInitializing.value) {
        return;
      }

      const departmentExists = newDepartments.some(sub => sub.id === selectedDepartmentId.value);
      if (departmentExists) {
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
      if (searchSubDepartments) {
        await searchSubDepartments({
          searchTerm: search,
          departmentId: selectedDepartmentId.value,
          page: currentPage.value,
          pageSize: pageSize.value,
        });
      }
    } else {
      clearSearch?.();

      if (selectedDepartmentId.value) {
        if (searchSubDepartments) {
          await searchSubDepartments({
            searchTerm: '',
            departmentId: selectedDepartmentId.value,
            page: currentPage.value,
            pageSize: pageSize.value,
          });
        }
      } else {
        await getSubDepartmentsByUserId?.();
      }
    }
  });

  // Watch selected sub SubDepartment
  watch(selectedDepartmentId, async (newDepartmentId) => {

    clearSearch?.();

    if (allSubDepartments?.value !== undefined) {
      allSubDepartments.value = [];
    }

    if (!newDepartmentId) {
      clearDepartment?.();
      if (!searchQuery.value.trim()) {
        await getSubDepartmentsByUserId?.();
      }
      return;
    }

    try {
      await getDepartmentById?.(newDepartmentId);
    } catch (error) {
      console.error('[Process] Error fetching Sub Department:', error);
    }

    await searchSubDepartments?.({
      searchTerm: searchQuery.value || '',
      departmentId: newDepartmentId,
      page: currentPage.value,
      pageSize: pageSize.value,
    });
    
  });
}