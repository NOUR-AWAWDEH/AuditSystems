import { ref } from 'vue';
import { useSearchSubProcessesByUserId } from '@/composables/riskControl/subProcess/useSearchSubProcessesByUserId';
import { useGetSubProcessesByUserId } from '@/composables/riskControl/subProcess/useGetSubProcessesByUserId';

export function useSubProcessData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchSubProcesses,
        clearSearch
    } = useSearchSubProcessesByUserId() || {};

    const {
        subProcesses: allSubProcesses = ref([]),
        isLoading: isLoadingAll = ref(false),
        getSubProcessesByUserId
    } = useGetSubProcessesByUserId() || {};

    return {
        searchResults,
        allSubProcesses,
        isSearching,
        isLoadingAll,
        totalCount,
        searchSubProcesses,
        clearSearch,
        getSubProcessesByUserId
    };
}