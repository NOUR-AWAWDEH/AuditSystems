import { ref } from 'vue';
import { useSearchSubLocationByUserId } from '@/composables/organization/sublocation/useSearchSubLocationByUserId';
import { useGetSubLocationsByUserId } from '@/composables/organization/sublocation/useGetSubLocationsByUserId';

export function useSubLocationData() {
  const {
    searchResults = ref([]),
    isSearching = ref(false),
    totalCount = ref(0),
    searchSubLocations,
    clearSearch
  } = useSearchSubLocationByUserId() || {};

  const {
    subLocations: allSubLocations = ref([]),
    isLoading: isLoadingAll = ref(false),
    getSubLocationsByUserId
  } = useGetSubLocationsByUserId() || {};

  return {
    searchResults,
    allSubLocations,
    isSearching,
    isLoadingAll,
    totalCount,
    searchSubLocations,
    clearSearch,
    getSubLocationsByUserId
  };
}
