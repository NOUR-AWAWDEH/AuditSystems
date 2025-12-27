import { ref } from 'vue';
import { getRecommendationsByUserIdRepository } from '@/services/riskControl/recommendation/getRecommendationsByUserIdRepository';

export function useGetRecommendationsByUserId() {
    const recommendations = ref([]);
    const isLoading = ref(false);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        const recommendationsData = response.result?.recommendationModel || [];
        recommendations.value = recommendationsData;
        return { success: true, data: recommendationsData };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch Recommendations') => {
        const errorMessage = response?.errors?.length > 0
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const getRecommendationsByUserId = async () => {
        try {
            isLoading.value = true;
            error.value = null;
            recommendations.value = [];

            const response = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch recommendations');
        } finally {
            isLoading.value = false;
        }
    };

    return {
        recommendations,
        isLoading,
        error,
        getRecommendationsByUserId,
    };
}
