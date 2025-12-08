<template>
  <!-- Add Job Title Form -->
  <AddJobTitleForm
    :is-visible="showAddJobTitleModal"
    :company-id="companyId"
    @close="handleCloseAddJobTitleModal"
    @job-title-added="handleJobTitleAdded"
  />

  <!-- Job Title Table -->
  <JobTitleTable
    :job-titles="jobTitles"
    :is-loading="isLoading"
    :search-query="searchQuery"
    :handle-search="handleSearch"
    @edit-job-title="handleEditJobTitle"
    @delete-job-title="handleDeleteJobTitle"
  />

  <!-- Edit Job Title Modal -->
  <EditJobTitleModal
    v-if="showEditJobTitleModal"
    :is-visible="showEditJobTitleModal"
    :job-title="jobTitleToEdit"
    :company-id="companyId"
    @close="handleCloseEditJobTitleModal"
    @job-title-updated="handleJobTitleUpdated"
  />

  <!-- Confirmation Dialog -->
  <ConfirmDialog
    v-model="showDeleteDialog"
    :title="deleteDialogTitle"
    :message="deleteDialogMessage"
    :highlight-text="itemToDelete?.name || ''"
    :confirm-text="'Delete'"
    :cancel-text="'Cancel'"
    @confirm="confirmDelete"
    @cancel="cancelDelete"
  />
</template>

<script setup>
import JobTitleTable from "./components/JobTitleTable.vue";
import AddJobTitleForm from "./components/AddJobTitleForm.vue";
import EditJobTitleModal from "./components/EditJobTitleModal.vue";
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import { useJobTitleTable } from "./composables/useJobTitleTable";
import { useJobTitleActions } from "./composables/useJobTitleActions";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";

const { companyId } = useAuthStatus();

const emit = defineEmits(["job-title-added", "job-title-updated", "job-title-deleted"]);

const { jobTitles, isLoading, searchQuery, handleSearch, resetToDefaultView, refreshData } =
  useJobTitleTable();

const {
  showAddJobTitleModal,
  showEditJobTitleModal,
  jobTitleToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleCloseAddJobTitleModal,
  handleJobTitleAdded,
  handleEditJobTitle,
  handleCloseEditJobTitleModal,
  handleJobTitleUpdated,
  handleDeleteJobTitle,
  confirmDelete,
  cancelDelete,
} = useJobTitleActions(jobTitles, emit);

// Expose job-titles for parent component
defineExpose({
  jobTitles,
  resetToDefaultView,
  refreshData,
});
</script>
