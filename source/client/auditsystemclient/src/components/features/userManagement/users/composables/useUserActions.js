import { ref } from "vue";
import { useDeleteUserById } from "@/composables/userManagement/users/useDeleteUserById";
import { useNotification } from "@/composables/notification/useNotification";

export function useUserActions(users, emit, refreshData) {
  const { deleteUser } = useDeleteUserById();
  const { showNotification } = useNotification();

  // Modal states
  const showAddUserModal = ref(false);
  const showEditUserModal = ref(false);
  const userToEdit = ref(null);
  const showDeleteDialog = ref(false);
  const deleteDialogTitle = ref("");
  const deleteDialogMessage = ref("");
  const itemToDelete = ref(null);

  // Add user actions
  const handleAddUser = () => {
    showAddUserModal.value = true;
  };

  const handleCloseAddUserModal = () => {
    showAddUserModal.value = false;
  };

  const handleUserAdded = async (newUser) => {
    // Refresh data from server to get the latest user list immediately
    if (refreshData) {
      await refreshData();
    } else {
      // Fallback: add to local array if refreshData is not provided
      users.value.push(newUser);
    }
    emit("user-added", newUser);
    handleCloseAddUserModal();
  };

  // User actions
  const handleEditUser = (user) => {
    userToEdit.value = user;
    showEditUserModal.value = true;
  };

  const handleCloseEditUserModal = () => {
    showEditUserModal.value = false;
    userToEdit.value = null;
  };

  const handleUserUpdated = async (updatedUser) => {
    try {
      // Find and replace the updated user in the local list
      const index = users.value.findIndex((user) => user.id === updatedUser.id);

      if (index !== -1) {
        users.value[index] = { ...users.value[index], ...updatedUser };
      }

      emit("user-updated", updatedUser);

      showNotification(
        `User "${updatedUser.name}" has been updated successfully!`,
        "success"
      );

      handleCloseEditUserModal();
    } catch (error) {
      console.error("Error updating user:", error);
      showNotification(`Failed to update user.`, "error");
    }
  };

  const handleDeleteUser = (user) => {
    itemToDelete.value = user;
    deleteDialogTitle.value = "Delete User";
    deleteDialogMessage.value = `Are you sure you want to delete "${user.FirstName} ${user.LastName}"? This action cannot be undone.`;
    showDeleteDialog.value = true;
  };

  // Delete confirmation
  const confirmDelete = async () => {
    if (!itemToDelete.value) return;

    const deletedUser = { ...itemToDelete.value };
    const result = await deleteUser(deletedUser.id);

    if (result.success) {
      // Remove the deleted user from the local list
      const index = users.value.findIndex((user) => user.id === deletedUser.id);
      if (index !== -1) {
        users.value.splice(index, 1); // <-- reactive change
      }

      showNotification(
        `User "${deletedUser.FirstName} ${deletedUser.LastName}" has been deleted successfully!`,
        "success"
      );

      emit("user-deleted", deletedUser.id);
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
    showAddUserModal,
    showEditUserModal,
    userToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,

    // Action handlers
    handleAddUser,
    handleCloseAddUserModal,
    handleUserAdded,
    handleEditUser,
    handleCloseEditUserModal,
    handleUserUpdated,
    handleDeleteUser,
    confirmDelete,
    cancelDelete,
  };
}