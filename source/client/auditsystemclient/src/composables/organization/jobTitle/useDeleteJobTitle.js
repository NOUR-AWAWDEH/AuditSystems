import { ref } from "vue";
import { deleteJobTitleByIdRepository } from "@/services/organization/JobTitle/deleteJobTitleRepository";

export function useDeleteJobTitle() {
  const isDeleting = ref(false);
  const error = ref(null);

  const deleteJobTitle = async (id) => {
    isDeleting.value = true;
    error.value = null;

    try {
      const result = await deleteJobTitleByIdRepository.deleteJobTitleById(id);

      if (result.success) {
        return { success: true };
      } else {
        const errorMsg = result.message || "Failed to delete job title";
        error.value = errorMsg;
        return { success: false, message: errorMsg, errors: result.errors };
      }
    } catch (err) {
      const errorMsg = err.message || "An error occurred while deleting job title";
      error.value = errorMsg;
      console.error("Error deleting job title:", err);
      return { success: false, message: errorMsg };
    } finally {
      isDeleting.value = false;
    }
  };

  return {
    isDeleting,
    error,
    deleteJobTitle,
  };
}
