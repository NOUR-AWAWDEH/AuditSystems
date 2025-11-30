import { ref } from "vue";
import { useDeleteSubDepartmentById } from "@/composables/organization/subdepartment/useDeleteSubDepartmentById";
import { useNotification } from "@/composables/notification/useNotification";

export function useSubDepartmentModals(subDepartments, emit) {
  const { deleteSubDepartment } = useDeleteSubDepartmentById();
  const { showNotification } = useNotification();

  const showAddSubDepartmentModal = ref(false);
  const showEditSubDepartmentModal = ref(false);
  const subDepartmentToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  const handleAddSubDepartment = () => {
    showAddSubDepartmentModal.value = true;
  };

  const handleCloseAddSubDepartmentModal = () => {
    showAddSubDepartmentModal.value = false;
  };

  const handleSubDepartmentAdded = async (newSubDepartment) => {
    try {
      subDepartments.value.push(newSubDepartment);
      emit("sub-department-added", newSubDepartment);
      handleCloseAddSubDepartmentModal();
    } catch (error) {
      console.error("Error in handleSubDepartmentAdded:", error);
      showNotification("Failed to refresh data after adding sub-department", "error");
    }
  };

  const handleEditSubDepartment = (subDepartment) => {
    subDepartmentToEdit.value = subDepartment;
    showEditSubDepartmentModal.value = true;
  };

  const handleCloseEditSubDepartmentModal = () => {
    showEditSubDepartmentModal.value = false;
    subDepartmentToEdit.value = null;
  };

  const handleSubDepartmentUpdated = async (updatedSubDepartment) => {
    try {
      // Find and replace the updated process in the local list
      const index = subDepartments.value.findIndex((p) => p.id === updatedSubDepartment.id);

      if (index !== -1) {
        subDepartments.value[index] = { ...subDepartments.value[index], ...updatedSubDepartment };
      }

      emit("sub-department-updated", updatedSubDepartment);

      showNotification(
        `Sub-Department "${updatedSubDepartment.name}" has been updated successfully!`,
        "success"
      );

      handleCloseEditSubDepartmentModal();
    } catch (error) {
      console.error("Error in handleSubDepartmentUpdated:", error);
      showNotification("Failed to update sub-department", "error");
    }
  };

  const handleDeleteSubDepartment = (subDepartment) => {
    itemToDelete.value = subDepartment;
    deleteDialogTitle.value = "Delete Sub-Department";
    deleteDialogMessage.value = `Are you sure you want to delete the sub-department "${subDepartment.name}"?`;
    showDeleteDialog.value = true;
  };

  const confirmDelete = async () => {
    if (!itemToDelete.value) return;

    const deletedSubDepartment = { ...itemToDelete.value };
    const result = await deleteSubDepartment(deletedSubDepartment.id);

    if (result.success) {
      // Remove the deleted audit process from the local list
      const index = subDepartments.value.findIndex((p) => p.id === deletedSubDepartment.id);
      if (index !== -1) {
        subDepartments.value.splice(index, 1); // <-- reactive change
      }

      showNotification(
        `Sub-Department "${deletedSubDepartment.name}" has been deleted successfully!`,
        "success"
      );

      localStorage.removeItem("organizationSelectedSubDepartmentId");

      emit("sub-department-deleted", deletedSubDepartment.id);
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
    showAddSubDepartmentModal,
    showEditSubDepartmentModal,
    subDepartmentToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddSubDepartment,
    handleCloseAddSubDepartmentModal,
    handleSubDepartmentAdded,
    handleEditSubDepartment,
    handleCloseEditSubDepartmentModal,
    handleSubDepartmentUpdated,
    handleDeleteSubDepartment,
    confirmDelete,
    cancelDelete,
  };
}
