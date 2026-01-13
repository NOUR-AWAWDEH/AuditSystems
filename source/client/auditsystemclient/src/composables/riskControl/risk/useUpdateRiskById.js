import { ref } from "vue";
import { updateRiskByIdRepository } from "@/services/riskControl/risk/updateRiskByIdRepository";

export function useUpdateRiskById() {
  const isUpdating = ref(false);
  const error = ref(null);
  const updatedRisk = ref(null);

  const handleUpdateSuccess = (response) => {
    updatedRisk.value = response.result;
    return { success: true, data: response.result };
  };

  const handleUpdateFailure = (message = "Failed to update risk", errors = []) => {
    error.value = message;
    return {
      success: false,
      message,
      errors: errors.length ? errors : [{ message }],
    };
  };

  const validateUpdateRisk = (riskData) => {
    if (!riskData.id) return "Risk ID is required";
    if (!riskData.factor?.trim()) return "Risk factor is required";
    if (!riskData.impact?.trim()) return "Risk impact is required";
    if (!riskData.observationId) return "Observation ID is required";

    return null;
  };

  const updateRisk = async (riskData) => {
    try {
      isUpdating.value = true;
      error.value = null;
      updatedRisk.value = null;

      // Client-side validation
      const validationError = validateUpdateRisk(riskData);
      if (validationError) {
        return handleUpdateFailure(validationError);
      }

      // API call
      const response = await updateRiskByIdRepository.updateRiskById(riskData);

      if (response?.success) {
        return handleUpdateSuccess(response);
      } else {
        const message =
          response?.errors?.[0]?.message || response?.successMessage || "Failed to update risk";
        return handleUpdateFailure(message, response?.errors || []);
      }
    } catch (err) {
      const message = err?.message || "Unexpected error while updating risk";
      return handleUpdateFailure(message);
    } finally {
      isUpdating.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    updatedRisk.value = null;
    isUpdating.value = false;
  };

  return {
    isUpdating,
    error,
    updatedRisk,
    updateRisk,
    resetState,
  };
}
