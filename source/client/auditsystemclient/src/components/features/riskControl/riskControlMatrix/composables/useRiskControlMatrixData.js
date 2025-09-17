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
        riskControlMatrices: allRiskControlMatrices = ref([]),
        isLoading: isLoadingAll = ref(false),
        getRiskControlMatricesByUserId
    } = useGetRiskControlMatricesByUserId() || {};

    return {
        searchResults,
        allRiskControlMatrices,
        isSearching,
        isLoadingAll,
        totalCount,
        searchRiskControlMatrices,
        clearSearch,
        getRiskControlMatricesByUserId
    };
}