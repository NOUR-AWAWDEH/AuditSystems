import { ref } from 'vue';
import { useSearchRiskControlMatricesByUserId } from '@/composables/riskControl/riskControlMatrix/useSearchRiskControlMatricesByUserId';
import { useGetRiskControlMatricesByUserId } from '@/composables/riskControl/riskControlMatrix/useGetRiskControlMatricesByUserId';

export function useRiskControlMatrixData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchRiskControlMatrices,
        clearSearch
    } = useSearchRiskControlMatricesByUserId() || {};

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