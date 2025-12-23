<template>
  <!--UserView.vue-->
  <div class="space-y-6">
    <!-- AddUserForm -->
    <AddUserForm
      :is-visible="showAddUserModal"
      :company-id="companyId"
      @close="handleCloseAddUserModal"
      @user-added="handleUserAdded"
    />

    <!-- UsersTable with integrated selection -->
    <UsersTable
      :users="users"
      :is-loading="isLoading"
      :search-query="searchQuery"
      :handle-search="handleSearch"
      @edit-user="handleEditUser"
      @delete-user="handleDeleteUser"
    />

    <!-- Edit User Modal -->
    <EditUserModal
      v-if="showEditUserModal"
      :is-visible="showEditUserModal"
      :user="userToEdit"
      @close="handleCloseEditUserModal"
      @user-updated="handleUserUpdated"
    />

    <!-- Confirmation Dialog -->
    <ConfirmDialog
      v-model="showDeleteDialog"
      :title="deleteDialogTitle"
      :message="deleteDialogMessage"
      :highlight-text="itemToDelete?.firstName || ''"
      :confirm-text="'Delete'"
      :cancel-text="'Cancel'"
      @confirm="confirmDelete"
      @cancel="cancelDelete"
    />
  </div>
</template>

<script setup>
import UsersTable from "./components/UsersTable.vue";
import AddUserForm from "./components/AddUserForm.vue";
import EditUserModal from "./components/EditUserModal.vue";
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import { useUserTable } from "./composables/useUserTable";
import { useUserActions } from "./composables/useUserActions";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";

const { companyId } = useAuthStatus();

const emit = defineEmits(["user-added", "user-updated", "user-deleted"]);

const { users, isLoading, searchQuery, handleSearch, resetToDefaultView, refreshData } =
  useUserTable();

const {
  showAddUserModal,
  showEditUserModal,
  userToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleCloseAddUserModal,
  handleUserAdded,
  handleEditUser,
  handleCloseEditUserModal,
  handleUserUpdated,
  handleDeleteUser,
  confirmDelete,
  cancelDelete,
} = useUserActions(users, emit, refreshData);

// Expose users for parent component
defineExpose({
  users,
  resetToDefaultView,
  refreshData,
});
</script>
