import { ref } from 'vue';
import { getRecommendationByIdRepository } from '@/services/riskControl/recommendation/getRecommendationByIdRepository.js';

export function useGetRecommendationById() {
    const recommendation = ref(null);
    const isLoading = ref(null);
    const error = ref(null);

    const handleFetchSuccess = (response) => {
        recommendation.value = response.result;
        return { success: true, data: response.result };
    };

    const handleFetchFailure = (response, defaultErrorMessage = 'Failed to fetch Recommendation!') => {
        const errorMessage = response?.errors?.length
            ? response?.errors[0]?.message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;

        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateRecommendationId = (recommendationId) => {
        if (!recommendationId) {
            const message = 'Recommendation ID is required.';
            return { success: false, message };
        }
    };

    const getRecommendationById = async (recommendationId) => {
        try {
            isLoading.value = true;
            error.value = null;
            recommendation.value = null;

            const validationError = validateRecommendationId(recommendationId);
            if (validationError) {
              return handleFetchFailure({
                errors: [{ message: validationError.message }],
              });
            }
            const response = await getRecommendationByIdRepository.getRecommendationById(recommendationId);

            if (response?.success) {
                return handleFetchSuccess(response);
            } else {
                return handleFetchFailure(response);
            }

        } catch (error) {
            return handleFetchFailure(error, error.message || 'Failed to fetch Recommendation');

        } finally {
            isLoading.value = false;
        }
    };

    const clearRecommendation = () => {
        error.value = null;
        recommendation.value = null;
        isLoading.value = false;
    };

    return {
        recommendation,
        isLoading,
        error,
        getRecommendationById,
        clearRecommendation,
    };
}
