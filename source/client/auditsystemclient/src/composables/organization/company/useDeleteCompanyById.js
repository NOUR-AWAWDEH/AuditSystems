import { ref } from "vue";
import { deleteCompanyByIdRepository } from "@/services/organization/company/deleteCompanyRepository";

export function useDeleteCompanyById() {
  const isDeleting = ref(false);
  const error = ref(null);
  const deletedCompany = ref(null);

  const handleDeleteSuccess = (response) => {
    deletedCompany.value = true;
    const message = response?.successMessage || "Company deleted successfully!";
    return { success: true, message };
  };

  const handleDeleteFailure = (response, defaultErrorMessage = "Failed to delete Company") => {
    const errorMessage = response?.errors?.length
      ? response?.errors[0]?.message
      : response?.message || defaultErrorMessage;

    error.value = errorMessage;

    const failureResult = {
      success: false,
      message: errorMessage,
      errors: response?.errors || [{ message: errorMessage }],
    };

    return failureResult;
  };

  const validateCompanyId = (companyId) => {
    if (!companyId) {
      const message = "Company ID is required";
      return { success: false, message };
    }
  };

  const deleteCompany = async (companyId) => {
    try {
      isDeleting.value = true;
      error.value = null;
      deletedCompany.value = null;

      const validationError = validateCompanyId(companyId);
      if (validationError) {
        return handleDeleteFailure({ message: validationError });
      }

      const response = await deleteCompanyByIdRepository.deleteCompanyById(companyId);

      if (response?.success) {
        return handleDeleteSuccess(response);
      } else {
        return handleDeleteFailure(response);
      }
      
    } catch (err) {
      return handleDeleteFailure(err, err.message || "Failed to delete Company");
    } finally {
      isDeleting.value = false;
    }
  };

  const resetState = () => {
    error.value = null;
    deletedCompany.value = null;
    isDeleting.value = false;
  };

  return {
    isDeleting,
    error,
    deletedCompany,
    deleteCompany,
    resetState,
  };
}
