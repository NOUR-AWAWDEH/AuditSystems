import { ref } from "vue";
import { useDeleteLocationById } from "@/composables/organization/location/useDeleteLocationById";
import { useNotification } from "@/composables/notification/useNotification";

export function useLocationActions(locations, emit) {
  const { deleteLocation } = useDeleteLocationById();
  const { showNotification } = useNotification();

  // Modal states
  const showAddLocationModal = ref(false);
  const showEditLocationModal = ref(false);
  const locationToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  // Add location actions
  const handleAddLocation = () => {
    showAddLocationModal.value = true;
  };

  const handleCloseAddLocationModal = () => {
    showAddLocationModal.value = false;
  };

  const handleLocationAdded = async (newLocation) => {
    locations.value.push(newLocation);
    emit("location-added", newLocation);
    handleCloseAddLocationModal();
  };

  // Location actions
  const handleEditLocation = (location) => {
    locationToEdit.value = location;
    showEditLocationModal.value = true;
  };

  const handleCloseEditLocationModal = () => {
    showEditLocationModal.value = false;
    locationToEdit.value = null;
  };

  const handleLocationUpdated = async (updatedLocation) => {
    try {
      // Find and replace the updated location in the local list
      const index = locations.value.findIndex((loc) => loc.id === updatedLocation.id);

      if (index !== -1) {
        locations.value[index] = { ...locations.value[index], ...updatedLocation };
      }

      emit("location-updated", updatedLocation);

      showNotification(
        `Location "${updatedLocation.name}" has been updated successfully!`,
        "success"
      );

      handleCloseEditLocationModal();
    } catch (error) {
      console.error("Error updating location:", error);
      showNotification(`Failed to update location.`, "error");
    }
  };

  const handleDeleteLocation = (location) => {
    itemToDelete.value = location;
    deleteDialogTitle.value = "Delete Location";
    deleteDialogMessage.value = `Are you sure you want to delete "${location.name}"? This action cannot be undone.`;
    showDeleteDialog.value = true;
  };

  // Delete confirmation
  const confirmDelete = async () => {
    if (!itemToDelete.value) return;

    const deletedLocation = { ...itemToDelete.value };
    const result = await deleteLocation(deletedLocation.id);

    if (result.success) {
      // Remove the deleted location from the local list
      const index = locations.value.findIndex((loc) => loc.id === deletedLocation.id);
      if (index !== -1) {
        locations.value.splice(index, 1); // <-- reactive change
      }

      showNotification(
        `Location "${deletedLocation.name}" has been deleted successfully!`,
        "success"
      );

      localStorage.removeItem("organizationSelectedLocationId");

      emit("location-deleted", deletedLocation.id);
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
    showAddLocationModal,
    showEditLocationModal,
    locationToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,

    // Action handlers
    handleAddLocation,
    handleCloseAddLocationModal,
    handleLocationAdded,
    handleEditLocation,
    handleCloseEditLocationModal,
    handleLocationUpdated,
    handleDeleteLocation,
    confirmDelete,
    cancelDelete,
  };
}
