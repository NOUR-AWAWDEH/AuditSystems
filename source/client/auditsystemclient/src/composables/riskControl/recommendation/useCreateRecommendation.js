import { ref } from 'vue';
import { createRecommendationRepository } from '@/services/riskControl/recommendation/createRecommendationRepository';

export function useCreateRecommendation() {
    const isCreating = ref(false);
    const error = ref(null);
    const createdRecommendation = ref(null);

    const handleCreateSuccess = (response) => {
        createdRecommendation.value = response.result;
        return {
            success: true,
            data: response.result,
            message: response.successMessage || 'Recommendation created successfully.'
        };
    };

    const handleCreateFailure = (response, defaultErrorMessage = 'Failed to create Recommendation') => {
        const errorMessage = response?.errors?.length
            ? response.errors[0].message
            : response?.message || defaultErrorMessage;

        error.value = errorMessage;
        return {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }]
        };
    };

    const createRecommendation = async (recommendationData) => {
        try {
            isCreating.value = true;
            error.value = null;
            createdRecommendation.value = null;

            const response = await createRecommendationRepository.createRecommendation(recommendationData);


            if (response?.success) {
                return handleCreateSuccess(response);
            } else {
                return handleCreateFailure(response);
            }

        } catch (err) {
            console.error('useCreateRecommendation: Create error:', err);
            return handleCreateFailure(err, err.message || 'Failed to create Recommendation');

        } finally {
            isCreating.value = false;
        }
    };

    const resetState = () => {
        error.value = null;
        createdRecommendation.value = null;
        isCreating.value = false;
    };

    return {
        isCreating,
        error,
        createdRecommendation,
        createRecommendation,
        resetState
    };
}
