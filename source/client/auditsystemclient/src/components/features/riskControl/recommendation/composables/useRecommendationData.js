import { ref } from 'vue';
import { useSearchRecommendationsByUserId } from '@/composables/riskControl/recommendation/useSearchRecommendationsByUserId';
import { useGetRecommendationsByUserId } from '@/composables/riskControl/recommendation/useGetRecommendationsByUserId';

export function useRecommendationData() {
    const {
        searchResults = ref([]),
        isSearching = ref(false),
        totalCount = ref(0),
        searchRecommendations,
        clearSearch
    } = useSearchRecommendationsByUserId() || {};

    const {
        recommendations: allRecommendations = ref([]),
        isLoading: isLoadingAll = ref(false),
        getRecommendationsByUserId
    } = useGetRecommendationsByUserId() || {};

    return {
        searchResults,
        allRecommendations,
        isSearching,
        isLoadingAll,
        totalCount,
        searchRecommendations,
        clearSearch,
        getRecommendationsByUserId
    };
}