import { ref } from "vue";
import { useDeleteSubProcessById } from "@/composables/riskControl/subProcess/useDeleteSubProcessById";
import { useNotification } from "@/composables/notification/useNotification";

export function useSubProcessModals(subProcesses, emit) {
  const { deleteSubProcess } = useDeleteSubProcessById();
  const { showNotification } = useNotification();

  const showAddSubProcessModal = ref(false);
  const showEditSubProcessModal = ref(false);
  const subProcessToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  const handleAddSubProcess = () => {
    showAddSubProcessModal.value = true;
  };

  const handleCloseAddSubProcessModal = () => {
    showAddSubProcessModal.value = false;
  };

  const handleSubProcessAdded = async (newSubProcess) => {
    try {
      subProcesses.value.push(newSubProcess);
      emit("sub-process-added", newSubProcess);
      handleCloseAddSubProcessModal();
    } catch (error) {
      console.error("Error in handleSubProcessAdded:", error);
      showNotification("Failed to refresh data after adding subprocess", "error");
    }
  };

  const handleEditSubProcess = (subProcess) => {
    subProcessToEdit.value = subProcess;
    showEditSubProcessModal.value = true;
  };

  const handleCloseEditSubProcessModal = () => {
    showEditSubProcessModal.value = false;
    subProcessToEdit.value = null;
  };

  const handleSubProcessUpdated = async (updatedSubProcess) => {
    try {
      const index = subProcesses.value.findIndex((sp) => sp.id === updatedSubProcess.id);
      if (index !== -1) {
        subProcesses.value[index] = updatedSubProcess;
      }

      if (index !== -1) {
        subProcesses.value[index] = { ...subProcesses.value[index], ...updatedSubProcess };
      }

      emit("sub-process-updated", updatedSubProcess);

      showNotification(
        `SubProcess "${updatedSubProcess.particular}" has been updated successfully!`,
        "success"
      );
      handleCloseEditSubProcessModal();
    } catch (error) {
      console.error("Error in handleSubProcessUpdated:", error);
      showNotification("Failed to refresh data after updating subprocess", "error");
    }
  };

  const handleDeleteSubProcess = (subProcess) => {
    itemToDelete.value = subProcess;
    deleteDialogTitle.value = "Delete SubProcess";
    deleteDialogMessage.value = `Are you sure you want to delete the subprocess "${subProcess.particular}"?`;
    showDeleteDialog.value = true;
  };

  const confirmDelete = async () => {
    if (itemToDelete.value) {
      const deletedSubProcess = { ...itemToDelete.value };
      const result = await deleteSubProcess(deletedSubProcess.id);

      if (result.success) {
        const index = subProcesses.value.findIndex((p) => p.id === deletedSubProcess.id);
        if (index !== -1) {
          subProcesses.value.splice(index, 1); // <-- reactive change
        }
        showNotification(
          `SubProcess "${deletedSubProcess.particular}" has been deleted successfully!`,
          "success"
        );
        localStorage.removeItem("riskControlSelectedSubProcessId");
        emit("sub-process-deleted", deletedSubProcess.id);
      } else {
        showNotification(result.message, "error");
      }

      cancelDelete();
    }
  };

  const cancelDelete = () => {
    showDeleteDialog.value = false;
    itemToDelete.value = null;
    deleteDialogTitle.value = "";
    deleteDialogMessage.value = "";
  };

  return {
    showAddSubProcessModal,
    showEditSubProcessModal,
    subProcessToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddSubProcess,
    handleCloseAddSubProcessModal,
    handleSubProcessAdded,
    handleEditSubProcess,
    handleCloseEditSubProcessModal,
    handleSubProcessUpdated,
    handleDeleteSubProcess,
    confirmDelete,
    cancelDelete,
  };
}
