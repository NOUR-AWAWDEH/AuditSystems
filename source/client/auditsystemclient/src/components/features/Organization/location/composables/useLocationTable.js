import { ref, watch, onMounted } from 'vue';
import { useSearchLocationByUserId } from '@/composables/organization/location/useSearchLocationByUserId';
import { useGetLocationsByUserId } from '@/composables/organization/location/useGetLocationsByUserId';
import { useGetSubLocationsByUserId } from '@/composables/organization/sublocation/useGetSubLocationsByUserId';

export function useLocationTable() {
  const searchQuery = ref('');
  const sortBy = ref('name');
  const currentPage = ref(1);
  const pageSize = ref(10);

  const {
    searchResults: locations,
    isSearching: isLoading,
    totalCount,
    searchLocations
  } = useSearchLocationByUserId();

  const {
    locations: allLocations,
    fetchLocationsByUserId
  } = useGetLocationsByUserId();

  const {
    subLocations,
    getSubLocationsByUserId
  } = useGetSubLocationsByUserId();

  // 🔁 This handles live search or manual refresh
  const refreshData = async () => {
    if (searchQuery.value.trim()) {
      await searchLocations(searchQuery.value, currentPage.value, pageSize.value);
    } else {
      await fetchLocationsByUserId();
      // ✅ Assign after confirming data is fetched
      if (allLocations.value?.length) {
        locations.value = allLocations.value;
      } else {
        locations.value = []; // fallback to empty
      }
    }

    // Also refresh sublocations
    await getSubLocationsByUserId();
  };

  // ✅ Ensure refresh happens as soon as composable is used
  onMounted(() => {
    refreshData();
  });

  // 🔍 Optional: refresh on search changes (e.g., when typing)
  watch([searchQuery, currentPage], async ([search, page]) => {
    await refreshData();
  });

  return {
    locations,
    subLocations,
    isLoading,
    searchQuery,
    sortBy,
    currentPage,
    pageSize,
    totalCount,
    refreshData
  };
} 