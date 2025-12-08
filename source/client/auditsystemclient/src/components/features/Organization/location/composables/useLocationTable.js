import { watch, onMounted } from 'vue';
import { useSearchLocationsByUserId } from '@/composables/organization/location/useSearchLocationsByUserId';
import { useGetLocationsByUserId } from '@/composables/organization/location/useGetLocationsByUserId';
import { useSearchAndPagination } from '@/composables/common/pagination/useSearchAndPagination';

export function useLocationTable() {


  const {
    searchResults: locations,
    isSearching: isLoading,
    totalCount,
    searchLocations
  } = useSearchLocationsByUserId();

  const {
    locations: allLocations,
    getLocationsByUserId
  } = useGetLocationsByUserId();

  const {
    searchQuery,
    currentPage,
    pageSize,
    handleSearch,
  } = useSearchAndPagination();

  // 🔁 This handles live search or manual refresh
  const refreshData = async () => {
    if (searchQuery.value.trim()) {
      await searchLocations(searchQuery.value, currentPage.value, pageSize.value);
    } else {
      await getLocationsByUserId();
      locations.value = allLocations.value || [];
    }
  };


  onMounted(() => {
     refreshData();
  });

  watch([searchQuery, currentPage], async () => {
    await refreshData();
  });

  return {
    locations,
    isLoading,
    searchQuery,
    currentPage,
    pageSize,
    totalCount,
    handleSearch,
    refreshData
  };
}
