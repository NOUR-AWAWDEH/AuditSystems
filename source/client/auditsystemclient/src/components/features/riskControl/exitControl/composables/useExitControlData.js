import { ref } from 'vue';
import { useSearchExitControlsByUserId } from '@/composables/riskControl/exitControl/useSearchExitControlsByUserId';
import { useGetExitControlsByUserId } from '@/composables/riskControl/exitControl/useGetExitControlsByUserId';

export function useExitControlData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchExitControls,
        clearSearch
    } = useSearchExitControlsByUserId() || {};

    const {
        exitControls: allExitControls = ref([]),
        isLoading: isLoadingAll = ref(false),
        getExitControlsByUserId
    } = useGetExitControlsByUserId() || {};

    return {
        searchResults,
        allExitControls,
        isSearching,
        isLoadingAll,
        totalCount,
        searchExitControls,
        clearSearch,
        getExitControlsByUserId
    };
}