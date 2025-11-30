import { ref } from "vue";
import { useDeleteExitControlById } from "@/composables/riskControl/exitControl/useDeleteExitControlById";
import { useNotification } from "@/composables/notification/useNotification";

export function useExitControlModals(exitControls, emit) {
  const { deleteExitControl } = useDeleteExitControlById();
  const { showNotification } = useNotification();

  const showAddExitControlModal = ref(false);
  const showEditExitControlModal = ref(false);
  const exitControlToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  const handleAddExitControl = () => {
    showAddExitControlModal.value = true;
  };

  const handleCloseAddExitControlModal = () => {
    showAddExitControlModal.value = false;
  };

  const handleExitControlAdded = async (newExitControl) => {
    try {
      exitControls.value.push(newExitControl);
      emit("exit-control-added", newExitControl);
      handleCloseAddExitControlModal();
    } catch (error) {
      console.error("Error in handleExitControlAdded:", error);
      showNotification("Failed to refresh data after adding exit control", "error");
    }
  };

  const handleEditExitControl = (exitControl) => {
    exitControlToEdit.value = exitControl;
    showEditExitControlModal.value = true;
  };

  const handleCloseEditExitControlModal = () => {
    showEditExitControlModal.value = false;
    exitControlToEdit.value = null;
  };

  const handleExitControlUpdated = async (updatedExitControl) => {
    try {
      const index = exitControls.value.findIndex((obj) => obj.id === updatedExitControl.id);

      if (index !== -1) {
        exitControls.value[index] = { ...exitControls.value[index], ...updatedExitControl };
      }
      emit("exit-control-updated", updatedExitControl);

      showNotification(
        `Exit Control "${updatedExitControl.description}" has been updated successfully!`,
        "success"
      );

      handleCloseEditExitControlModal();
    } catch (error) {
      console.error("Error in handleExitControlUpdated:", error);
      showNotification("Failed to refresh data after updating exit control", "error");
    }
  };

  const handleDeleteExitControl = (exitControl) => {
    itemToDelete.value = exitControl;
    deleteDialogTitle.value = "Delete Exit Control";
    deleteDialogMessage.value = `Are you sure you want to delete the exit control "${exitControl.description}"?`;
    showDeleteDialog.value = true;
  };

  const confirmDelete = async () => {
    if (!itemToDelete.value) return;

    const deletedExitControl = { ...itemToDelete.value };
    const result = await deleteExitControl(deletedExitControl.id);

    if (result.success) {
      const index = exitControls.value.findIndex((obj) => obj.id === deletedExitControl.id);
      if (index !== -1) {
        exitControls.value.splice(index, 1);
      }

      showNotification(
        `Exit Control "${deletedExitControl.description}" has been deleted successfully!`,
        "success"
      );

      localStorage.removeItem("riskControlSelectedExitControlId");

      emit("exit-control-deleted", deletedExitControl.id);
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
    showAddExitControlModal,
    showEditExitControlModal,
    exitControlToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddExitControl,
    handleCloseAddExitControlModal,
    handleExitControlAdded,
    handleEditExitControl,
    handleCloseEditExitControlModal,
    handleExitControlUpdated,
    handleDeleteExitControl,
    confirmDelete,
    cancelDelete,
  };
}
