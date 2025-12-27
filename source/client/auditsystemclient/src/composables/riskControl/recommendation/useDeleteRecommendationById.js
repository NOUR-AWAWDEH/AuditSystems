import { ref } from 'vue';
import { deleteRecommendationByIdRepository } from '@/services/riskControl/recommendation/deleteRecommendationByIdRepository';


export function useDeleteRecommendationById() {
    const isDeleting = ref(false);
    const error = ref(null);
    const deletedRecommendation = ref(null);

    const handleDeleteSuccess = (response) => {
        deletedRecommendation.value = true;
        const message = response?.successMessage || 'Recommendation deleted successfully';
        return { success: true, message };
    };

    const handleDeleteFailure = (response, defaultErrorMessage = 'Failed to delete Recommendation') => {
        const errorMessage = response?.errors?.length > 0
            ? response.errors[0]?.message
            : response?.successMessage || defaultErrorMessage;

        error.value = errorMessage;

        return  {
            success: false,
            message: errorMessage,
            errors: response?.errors || [{ message: errorMessage }],
        };
    };

    const validateDeleteRecommendation = (recommendationId) => {
        if (!recommendationId) {
            return 'Recommendation ID is required';
        }
        return null;
    };

    const deleteRecommendation = async (recommendationId) => {
        try {
            isDeleting.value = true;
            error.value = null;
            deletedRecommendation.value = null;

          const validationError = validateDeleteRecommendation(recommendationId);
          if (validationError) {
            return handleDeleteFailure({
              errors: [{ message: validationError }],
            });
          }

          const response = await deleteRecommendationByIdRepository.deleteRecommendationById(recommendationId);


            if (response?.success) {
                return handleDeleteSuccess(response);
            } else {
                return handleDeleteFailure(response);
            }

        } catch (error) {
            return handleDeleteFailure(error, error.message || 'Failed to delete Recommendation');
        } finally {
            isDeleting.value = false;
        }
    };

    const resetState = () => {
        isDeleting.value = false;
        error.value = null;
        deletedRecommendation.value = null;
    };

    return {
        isDeleting,
        error,
        deletedRecommendation,
        deleteRecommendation,
        resetState,
    };
}
