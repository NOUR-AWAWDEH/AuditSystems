import { useDepartmentData } from './useDepartmentData';
import { useSubLocationManagement } from './useSubLocationManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useDepartmentActions } from './useDepartmentActions';
import { useDepartmentWatchers } from './useDepartmentWatchers';
import { useDepartmentLifecycle } from './useDepartmentLifecycle';
import { useDepartmentComputed } from './useDepartmentComputed';

export function useDepartmentTable(subLocations) {
  const dataComposable = useDepartmentData();
  const subLocationComposable = useSubLocationManagement(subLocations);
  const searchComposable = useSearchAndPagination();

  const actionsComposable = useDepartmentActions({
    ...dataComposable,
    ...subLocationComposable,
    ...searchComposable
  });

  const lifecycleComposable = useDepartmentLifecycle({
    ...dataComposable,
    ...subLocationComposable,
    ...searchComposable,
    ...actionsComposable,
  });

  const computedComposable = useDepartmentComputed({
    ...dataComposable,
    ...subLocationComposable,
    ...searchComposable,
    ...lifecycleComposable
  });

  useDepartmentWatchers({
    ...dataComposable,
    ...subLocationComposable,
    ...searchComposable,
    ...actionsComposable,
    ...lifecycleComposable
  });

  return {
    departments: computedComposable.departments,
    isLoading: computedComposable.isLoading,
    selectedSubLocationId: subLocationComposable.selectedSubLocationId,
    selectedSubLocation: subLocationComposable.selectedSubLocation,
    currentSubLocation: subLocationComposable.currentSubLocation,
    isSubLocationLoading: subLocationComposable.isSubLocationLoading,
    searchQuery: searchComposable.searchQuery,
    currentPage: searchComposable.currentPage,
    pageSize: searchComposable.pageSize,
    totalCount: dataComposable.totalCount,
    refreshData: actionsComposable.refreshData,
    handleDepartmentChange: actionsComposable.handleDepartmentChange,
    handleSearch: searchComposable.handleSearch,
    resetToDefaultView: actionsComposable.resetToDefaultView,
  };
}