import { ref } from 'vue';
import { useSearchObservationsByUserId } from '@/composables/riskControl/observation/useSearchObservationsByUserId';
import { useGetObservationsByUserId } from '@/composables/riskControl/observation/useGetObservationsByUserId';

export function useObservationData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchObservations,
        clearSearch
    } = useSearchObservationsByUserId() || {};

    const {
        observations: allObservations = ref([]),
        isLoading: isLoadingAll = ref(false),
        getObservationsByUserId
    } = useGetObservationsByUserId() || {};

    return {
        searchResults,
        allObservations,
        isSearching,
        isLoadingAll,
        totalCount,
        searchObservations,
        clearSearch,
        getObservationsByUserId
    };
}