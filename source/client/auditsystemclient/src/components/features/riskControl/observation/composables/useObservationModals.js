import { ref } from "vue";
import { useDeleteObservationById } from "@/composables/riskControl/observation/useDeleteObservationById";
import { useNotification } from "@/composables/notification/useNotification";

export function useObservationModals(observations, emit) {
  const { deleteObservation } = useDeleteObservationById();
  const { showNotification } = useNotification();

  const showAddObservationModal = ref(false);
  const showEditObservationModal = ref(false);
  const observationToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  const handleAddObservation = () => {
    showAddObservationModal.value = true;
  };

  const handleCloseAddObservationModal = () => {
    showAddObservationModal.value = false;
  };

  const handleObservationAdded = async (newObservation) => {
    try {
      observations.value.push(newObservation);
      emit("observation-added", newObservation);
      handleCloseAddObservationModal();
    } catch (error) {
      console.error("Error in handleObservationAdded:", error);
      showNotification("Failed to refresh data after adding observation", "error");
    }
  };

  const handleEditObservation = (observation) => {
    observationToEdit.value = observation;
    showEditObservationModal.value = true;
  };

  const handleCloseEditObservationModal = () => {
    showEditObservationModal.value = false;
    observationToEdit.value = null;
  };

  const handleObservationUpdated = async (updatedObservation) => {
    try {
      const index = observations.value.findIndex((sp) => sp.id === updatedObservation.id);
      if (index !== -1) {
        observations.value[index] = updatedObservation;
      }

      if (index !== -1) {
        observations.value[index] = { ...observations.value[index], ...updatedObservation };
      }

      emit("observation-updated", updatedObservation);

      showNotification(
        `Observation "${updatedObservation.particular}" has been updated successfully!`,
        "success"
      );
      handleCloseEditObservationModal();
    } catch (error) {
      console.error("Error in handleObservationUpdated:", error);
      showNotification("Failed to refresh data after updating observation", "error");
    }
  };

  const handleDeleteObservation = (observation) => {
    itemToDelete.value = observation;
    deleteDialogTitle.value = "Delete Observation";
    deleteDialogMessage.value = `Are you sure you want to delete the observation "${observation.particular}"?`;
    showDeleteDialog.value = true;
  };

  const confirmDelete = async () => {
    if (itemToDelete.value) {
      const deletedObservation = { ...itemToDelete.value };
      const result = await deleteObservation(deletedObservation.id);

      if (result.success) {
        const index = observations.value.findIndex((p) => p.id === deletedObservation.id);
        if (index !== -1) {
          observations.value.splice(index, 1); // <-- reactive change
        }
        showNotification(
          `Observation "${deletedObservation.particular}" has been deleted successfully!`,
          "success"
        );
        localStorage.removeItem("riskControlSelectedObservationId");
        emit("observation-deleted", deletedObservation.id);
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
    showAddObservationModal,
    showEditObservationModal,
    observationToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddObservation,
    handleCloseAddObservationModal,
    handleObservationAdded,
    handleEditObservation,
    handleCloseEditObservationModal,
    handleObservationUpdated,
    handleDeleteObservation,
    confirmDelete,
    cancelDelete,
  };
}
