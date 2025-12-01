import { ref } from "vue";
import { deleteAuditProgramByIdRepository } from "@/services/riskControl/auditProgram/deleteAuditProgramByIdRepository";

export function useDeleteAuditProgramById() {
  const isDeleting = ref(false);
  const error = ref(null);
  const deletedAuditProgram = ref(null);

  const handleDeleteSuccess = (response) => {
    deletedAuditProgram.value = true;
    const successMessage = response?.successMessage || "Audit Program deleted successfully";
    return { success: true, message: successMessage };
  };

  const handleDeleteFailure = (
    response,
    defaultErrorMessage = "Failed to delete audit program"
  ) => {
    const errorMessage =
      response?.errors?.length > 0
        ? response.errors[0]?.message
        : response?.successMessage || defaultErrorMessage;

    error.value = errorMessage;

    const failureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return failureResult;
  };

  const validateDeleteAuditProgram = (auditProgramId) => {
    if (!auditProgramId) {
      return "Audit Program ID is required";
    }
    return null;
  };

  const deleteAuditProgram = async (auditProgramId) => {
    try {
      isDeleting.value = true;
      error.value = null;
      deletedAuditProgram.value = null;

      const validationError = validateDeleteAuditProgram(auditProgramId);
      if (validationError) {
        return handleDeleteFailure({ message: validationError });
      }

      const response = await deleteAuditProgramByIdRepository.deleteAuditProgramById(
        auditProgramId
      );

      if (response?.success) {
        return handleDeleteSuccess(response);
      } else {
        return handleDeleteFailure(response);
      }
    } catch (error) {
      return handleDeleteFailure(error, error.message || "Failed to delete audit program");
    } finally {
      isDeleting.value = false;
    }
  };

  const resetState = () => {
    isDeleting.value = false;
    error.value = null;
    deletedAuditProgram.value = null;
  };

  return {
    isDeleting,
    error,
    deletedAuditProgram,
    deleteAuditProgram,
    resetState,
  };
}
