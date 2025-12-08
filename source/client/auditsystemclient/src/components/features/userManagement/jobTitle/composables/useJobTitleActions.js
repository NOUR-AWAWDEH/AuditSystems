import { ref } from "vue";
import { useDeleteJobTitle } from "@/composables/organization/jobTitle/useDeleteJobTitle";
import { useNotification } from "@/composables/notification/useNotification";

export function useJobTitleActions(jobTitles, emit) {
  const { deleteJobTitleById } = useDeleteJobTitle();
  const { showNotification } = useNotification();

  // Modal states
  const showEditJobTitleModal = ref(false);
  const jobTitleToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  // Add job title actions
  const handleJobTitleAdded = async (newJobTitle) => {
    jobTitles.value.push(newJobTitle);
    emit("job-title-added", newJobTitle);
  };

  // Job title actions
  const handleEditJobTitle = (jobTitle) => {
    jobTitleToEdit.value = jobTitle;
    showEditJobTitleModal.value = true;
  };

  const handleCloseEditJobTitleModal = () => {
    showEditJobTitleModal.value = false;
    jobTitleToEdit.value = null;
  };

  const handleJobTitleUpdated = async (updatedJobTitle) => {
    try {
      // Find and replace the updated job title in the local list
      const index = jobTitles.value.findIndex((jobTitle) => jobTitle.id === updatedJobTitle.id);

      if (index !== -1) {
        jobTitles.value[index] = { ...jobTitles.value[index], ...updatedJobTitle };
      }

      emit("job-title-updated", updatedJobTitle);

      showNotification(
        `Job title "${updatedJobTitle.name}" has been updated successfully!`,
        "success"
      );

      handleCloseEditJobTitleModal();
    } catch (error) {
      console.error("Error updating job title:", error);
      showNotification(`Failed to update job title.`, "error");
    }
  };

  const handleDeleteJobTitle = (jobTitle) => {
    itemToDelete.value = jobTitle;
    deleteDialogTitle.value = "Delete Job Title";
    deleteDialogMessage.value = `Are you sure you want to delete "${jobTitle.name}"? This action cannot be undone.`;
    showDeleteDialog.value = true;
  };

  // Delete confirmation
  const confirmDelete = async () => {
    if (!itemToDelete.value) return;

    const deletedJobTitle = { ...itemToDelete.value };
    const result = await deleteJobTitleById(deletedJobTitle.id);

    if (result.success) {
      // Remove the deleted job title from the local list
      const index = jobTitles.value.findIndex((jobTitle) => jobTitle.id === deletedJobTitle.id);
      if (index !== -1) {
        jobTitles.value.splice(index, 1); // <-- reactive change
      }

      showNotification(
        `Job title "${deletedJobTitle.name}" has been deleted successfully!`,
        "success"
      );


      emit("job-title-deleted", deletedJobTitle.id);
    } else {
      showNotification(result.message, "error");
    }
    cancelDelete();
  };

  const cancelDelete = () => {
    showDeleteDialog.value = false;
    itemToDelete.value = null;
    deleteDialogTitle.value = "";
    deleteDialogMessage.value = "";
  };

  return {
    // Modal states
    showEditJobTitleModal,
    jobTitleToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,

    // Action handlers
    handleJobTitleAdded,
    handleEditJobTitle,
    handleCloseEditJobTitleModal,
    handleJobTitleUpdated,
    handleDeleteJobTitle,
    confirmDelete,
    cancelDelete,
  };
}
