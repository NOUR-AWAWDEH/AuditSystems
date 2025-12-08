<template>
  <!-- JobTitle.vue -->
  <div>
    <JobTitleHeader />

    <!-- Add Job Title Form -->
    <AddJobTitle
      :company-id="companyId"
      @jobtitle-added="handleJobTitleAdded"
    />

    <!-- Job Title Table -->
    <JobTitleTable
      :job-titles="jobTitles"
      :is-loading="isLoading"
      :search-query="searchQuery"
      @search="handleSearch"
      @edit-job-title="handleEditJobTitle"
      @delete-job-title="handleDeleteJobTitle"
    />

    <!-- Edit Job Title Modal -->
    <EditJobTitle
      v-if="showEditJobTitleModal"
      :is-visible="showEditJobTitleModal"
      :job-title="jobTitleToEdit"
      @close="handleCloseEditJobTitleModal"
      @jobtitle-updated="handleJobTitleUpdated"
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
  </div>
</template>

<script setup>
import JobTitleTable from "./components/JobTitleTable.vue";
import AddJobTitle from "./components/AddJobTitle.vue";
import EditJobTitle from "./components/EditJobTitle.vue";
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import JobTitleHeader from "./components/JobTitleHeader.vue";
import { useJobTitleTable } from "./composables/useJobTitleTable";
import { useJobTitleActions } from "./composables/useJobTitleActions";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";

const {companyId } = useAuthStatus();

const emit = defineEmits([
  "job-title-added",
  "job-title-updated",
  "job-title-deleted"
]);

const { jobTitles, isLoading, searchQuery, handleSearch, resetToDefaultView, refreshData } = useJobTitleTable();

const {
  showEditJobTitleModal,
  jobTitleToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
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
