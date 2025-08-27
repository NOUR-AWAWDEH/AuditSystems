import { useSubLocationData } from './useSubLocationData';
import { useLocationManagement } from './useLocationManagement';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';
import { useSubLocationActions } from './useSubLocationActions';
import { useSubLocationWatchers } from './useSubLocationWatchers';
import { useSubLocationLifecycle } from './useSubLocationLifecycle';
import { useSubLocationComputed } from './useSubLocationComputed';

export function useSubLocationTable(locations) {
  const dataComposable = useSubLocationData();
  const locationComposable = useLocationManagement(locations);
  const searchComposable = useSearchAndPagination();

  const actionsComposable = useSubLocationActions({
    ...dataComposable,
    ...locationComposable,
    ...searchComposable
  });

  const lifecycleComposable = useSubLocationLifecycle({
    ...dataComposable,
    ...locationComposable,
    ...searchComposable,
    ...actionsComposable,
  });

  const computedComposable = useSubLocationComputed({
    ...dataComposable,
    ...locationComposable,
    ...searchComposable,
    ...lifecycleComposable
  });

  useSubLocationWatchers({
    ...dataComposable,
    ...locationComposable,
    ...searchComposable,
    ...actionsComposable,
    ...lifecycleComposable
  });

  return {
    subLocations: computedComposable.subLocations,
    isLoading: computedComposable.isLoading,
    selectedLocationId: locationComposable.selectedLocationId,
    selectedLocation: locationComposable.selectedLocation,
    currentLocation: locationComposable.currentLocation,
    isLocationLoading: locationComposable.isLocationLoading,
    searchQuery: searchComposable.searchQuery,
    currentPage: searchComposable.currentPage,
    pageSize: searchComposable.pageSize,
    totalCount: dataComposable.totalCount,
    refreshData: actionsComposable.refreshData,
    handleSubLocationChange: actionsComposable.handleSubLocationChange,
    handleSearch: searchComposable.handleSearch,
    resetToDefaultView: actionsComposable.resetToDefaultView,
  };
}