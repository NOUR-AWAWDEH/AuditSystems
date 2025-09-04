import { ref } from 'vue';
import { useSearchObjectivesByUserId } from '@/composables/riskControl/objective/useSearchObjectivesByUserId';
import { useGetObjectivesByUserId } from '@/composables/riskControl/objective/useGetObjectivesByUserId';

export function useObjectiveData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchObjectives,
        clearSearch
    } = useSearchObjectivesByUserId() || {};

    const {
        objectives: allObjectives = ref([]),
        isLoading: isLoadingAll = ref(false),
        getObjectivesByUserId
    } = useGetObjectivesByUserId() || {};

    return {
        searchResults,
        allObjectives,
        isSearching,
        isLoadingAll,
        totalCount,
        searchObjectives,
        clearSearch,
        getObjectivesByUserId
    };
}