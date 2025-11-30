import { ref } from "vue";
import { useDeleteRiskControlMatrixById } from "@/composables/riskControl/riskControlMatrix/useDeleteRiskControlMatrixById";
import { useNotification } from "@/composables/notification/useNotification";

export function useRiskControlMatrixModals(riskControlMatrices, emit) {
  const { deleteRiskControlMatrix } = useDeleteRiskControlMatrixById();
  const { showNotification } = useNotification();

  const showAddRiskControlMatrixModal = ref(false);
  const showEditRiskControlMatrixModal = ref(false);
  const riskControlMatrixToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  const handleAddRiskControlMatrix = () => {
    showAddRiskControlMatrixModal.value = true;
  };

  const handleCloseAddRiskControlMatrixModal = () => {
    showAddRiskControlMatrixModal.value = false;
  };

  const handleRiskControlMatrixAdded = async (newRiskControlMatrix) => {
    try {
      riskControlMatrices.value.push(newRiskControlMatrix);
      emit("risk-control-matrix-added");
      handleCloseAddRiskControlMatrixModal();
    } catch (error) {
      console.error("Error in handleRiskControlMatrixAdded:", error);
      showNotification("Failed to refresh data after adding risk control matrix", "error");
    }
  };

  const handleEditRiskControlMatrix = (riskControlMatrix) => {
    riskControlMatrixToEdit.value = riskControlMatrix;
    showEditRiskControlMatrixModal.value = true;
  };

  const handleCloseEditRiskControlMatrixModal = () => {
    showEditRiskControlMatrixModal.value = false;
    riskControlMatrixToEdit.value = null;
  };

  const handleRiskControlMatrixUpdated = async (updatedRiskControlMatrix) => {
    try {
      // Find and replace the updated risk control matrix in the local list
      const index = riskControlMatrices.value.findIndex(
        (matrix) => matrix.id === updatedRiskControlMatrix.id
      );

      if (index !== -1) {
        riskControlMatrices.value[index] = {
          ...riskControlMatrices.value[index],
          ...updatedRiskControlMatrix,
        };
      }

      emit("risk-control-matrix-updated", updatedRiskControlMatrix);

      showNotification(
        `Risk Control Matrix "${updatedRiskControlMatrix.description}" has been updated successfully!`,
        "success"
      );

      handleCloseEditRiskControlMatrixModal();
    } catch (error) {
      console.error("Error in handleRiskControlMatrixUpdated:", error);
      showNotification("Failed to refresh data after updating risk control matrix", "error");
    }
  };

  const handleDeleteRiskControlMatrix = (riskControlMatrix) => {
    itemToDelete.value = riskControlMatrix;
    deleteDialogTitle.value = "Delete Risk Control Matrix";
    deleteDialogMessage.value = `Are you sure you want to delete the risk control matrix "${riskControlMatrix.description}"?`;
    showDeleteDialog.value = true;
  };

  const confirmDelete = async () => {
    if (!itemToDelete.value) return;

    const deletedMatrix = { ...itemToDelete.value };
    const result = await deleteRiskControlMatrix(deletedMatrix.id);

    if (result.success) {
      const index = riskControlMatrices.value.findIndex((matrix) => matrix.id === deletedMatrix.id);
      if (index !== -1) {
        riskControlMatrices.value.splice(index, 1);
      }

      showNotification(
        `Risk Control Matrix "${deletedMatrix.description}" has been deleted successfully!`,
        "success"
      );

      localStorage.removeItem("riskControlSelectedRiskControlMatrixId");

      emit("risk-control-matrix-deleted", deletedMatrix.id);
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
    showAddRiskControlMatrixModal,
    showEditRiskControlMatrixModal,
    riskControlMatrixToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddRiskControlMatrix,
    handleCloseAddRiskControlMatrixModal,
    handleRiskControlMatrixAdded,
    handleEditRiskControlMatrix,
    handleCloseEditRiskControlMatrixModal,
    handleRiskControlMatrixUpdated,
    handleDeleteRiskControlMatrix,
    confirmDelete,
    cancelDelete,
  };
}
