import { ref } from "vue";
import { updateRecommendationByIdRepository } from "@/services/riskControl/recommendation/updateRecommendationByIdRepository";
import { validateUpdateRecommendation } from "@/services/validations/validateRecommendationData";

export function useUpdateRecommendationById() {
  const isUpdating = ref(false);
  const error = ref(null);
  const updatedRecommendation = ref(null);

  const handleUpdateSuccess = (response) => {
    updatedRecommendation.value = response.result;
    return { success: true, data: response.result };
  };

  const handleUpdateFailure = (message = "Failed to update Recommendation", errors = []) => {
    error.value = message;
    return {
      success: false,
      message,
      errors: errors.length ? errors : [{ message }],
    };
  };


  const updateRecommendation = async (recommendationData) => {
    try {
      isUpdating.value = true;
      error.value = null;
      updatedRecommendation.value = null;

      // Client-side validation
      const validationError = validateUpdateRecommendation(recommendationData);
      if (validationError) {
        return handleUpdateFailure(validationError);
      }

      // API call
      const response = await updateRecommendationByIdRepository.updateRecommendationById(recommendationData);

      if (response?.success) {
        return handleUpdateSuccess(response);
      } else {
        const message =
          response?.errors?.[0]?.message || response?.successMessage || "Failed to update Recommendation.";
        return handleUpdateFailure(message, response?.errors || []);
      }
    } catch (err) {
      const message = err?.message || "Unexpected error while updating Recommendation";
      return handleUpdateFailure(message);
    } finally {
      isUpdating.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    updatedRecommendation.value = null;
    isUpdating.value = false;
  };

  return {
    isUpdating,
    error,
    updatedRecommendation,
    updateRecommendation,
    resetState,
  };
}