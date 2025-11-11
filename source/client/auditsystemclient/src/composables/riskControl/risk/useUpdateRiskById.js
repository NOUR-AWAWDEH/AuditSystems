import { ref } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import { updateRiskByIdRepository } from '@/services/riskControl/risk/updateRiskByIdRepository';

export function useUpdateRiskById() {
  const isUpdating = ref(false);
  const error = ref(null);
  const updatedRisk = ref(null);
  const { showNotification } = useNotification();

  const handleUpdateSuccess = (response) => {
    updatedRisk.value = response.result;
    const message = response?.successMessage || 'Risk updated successfully';
    showNotification(message, 'success');
    return { success: true, data: response.result };
  };

  const handleUpdateFailure = (message = 'Failed to update risk', errors = []) => {
    error.value = message;
    showNotification(message, 'error');
    return {
      success: false,
      message,
      errors: errors.length ? errors : [{ message }],
    };
  };

  const validateUpdateRisk = (riskData) => {
    if (!riskData.id) return 'Risk ID is required';
    if (!riskData.riskName?.trim()) return 'Risk name is required';
    if (!riskData.description?.trim()) return 'Risk description is required';
    if (!riskData.objectiveId) return 'Objective ID is required';
    if (!riskData.rateId) return 'Rate ID is required';
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
          response?.errors?.[0]?.message || response?.successMessage || 'Failed to update risk';
        return handleUpdateFailure(message, response?.errors || []);
      }
    } catch (err) {
      const message = err?.message || 'Unexpected error while updating risk';
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