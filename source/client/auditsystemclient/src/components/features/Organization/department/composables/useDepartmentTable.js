import { useDepartmentData } from './useDepartmentData';
import { useLocationManagement } from './useLocationManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useDepartmentActions } from './useDepartmentActions';
import { useDepartmentWatchers } from './useDepartmentWatchers';
import { useDepartmentLifecycle } from './useDepartmentLifecycle';
import { useDepartmentComputed } from './useDepartmentComputed';

export function useDepartmentTable(locations) {
  const dataComposable = useDepartmentData();
  const locationComposable = useLocationManagement(locations);
  const searchComposable = useSearchAndPagination();

  const actionsComposable = useDepartmentActions({
    ...dataComposable,
    ...locationComposable,
    ...searchComposable
  });

  const lifecycleComposable = useDepartmentLifecycle({
    ...dataComposable,
    ...locationComposable,
    ...searchComposable,
    ...actionsComposable,
  });

  const computedComposable = useDepartmentComputed({
    ...dataComposable,
    ...locationComposable,
    ...searchComposable,
    ...lifecycleComposable
  });

  useDepartmentWatchers({
    ...dataComposable,
    ...locationComposable,
    ...searchComposable,
    ...actionsComposable,
    ...lifecycleComposable
  });

  return {
    departments: computedComposable.departments,
    isLoading: computedComposable.isLoading,
    selectedLocationId: locationComposable.selectedLocationId,
    selectedLocation: locationComposable.selectedLocation,
    currentLocation: locationComposable.currentLocation,
    isLocationLoading: locationComposable.isLocationLoading,
    searchQuery: searchComposable.searchQuery,
    currentPage: searchComposable.currentPage,
    pageSize: searchComposable.pageSize,
    totalCount: dataComposable.totalCount,
    handleDepartmentChange: actionsComposable.handleDepartmentChange,
    handleSearch: searchComposable.handleSearch,
    resetToDefaultView: actionsComposable.resetToDefaultView,
  };
}