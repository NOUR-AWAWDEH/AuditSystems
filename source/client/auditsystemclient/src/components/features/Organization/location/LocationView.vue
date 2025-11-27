<template>
  <!--LocationView.vue-->
  <div class="space-y-6">
    <!-- Add Location Modal -->
    <AddLocationForm
      :is-visible="showAddLocationModal"
      :companyId="companyId"
      @close="handleCloseAddLocationModal"
      @location-added="handleLocationAdded"
    />

    <!-- Location Table with integrated selection -->
    <LocationTable
      :locations="locations"
      :is-loading="isLoading"
      :search-query="searchQuery"
      :handle-search="handleSearch"
      @edit-location="handleEditLocation"
      @delete-location="handleDeleteLocation"
      @location-selected="handleLocationSelected"
    />

    <!-- Edit Location Modal -->
    <EditLocationModal
      v-if="showEditLocationModal"
      :is-visible="showEditLocationModal"
      :location="locationToEdit"
      @close="handleCloseEditLocationModal"
      @location-updated="handleLocationUpdated"
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
import LocationTable from "./components/LocationTable.vue";
import AddLocationForm from "./components/AddLocationForm.vue";
import EditLocationModal from "./components/EditLocationModal.vue";
import ConfirmDialog from "@/components/base/ConfirmDialog.vue";
import { useLocationTable } from "./composables/useLocationTable";
import { useLocationActions } from "./composables/useLocationActions";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";

const {companyId } = useAuthStatus();

const emit = defineEmits([
  "location-selected",
  "location-added",
  "location-updated",
  "location-deleted",
]);

const { locations, isLoading, searchQuery, handleSearch, resetToDefaultView, refreshData } =
  useLocationTable();

const {
  showAddLocationModal,
  showEditLocationModal,
  locationToEdit,
  showDeleteDialog,
  deleteDialogTitle,
  deleteDialogMessage,
  itemToDelete,
  handleCloseAddLocationModal,
  handleLocationAdded,
  handleEditLocation,
  handleCloseEditLocationModal,
  handleLocationUpdated,
  handleDeleteLocation,
  confirmDelete,
  cancelDelete,
} = useLocationActions(locations, emit);

const handleLocationSelected = (locationId) => {
  emit("location-selected", locationId);
};

// Expose locations for parent component
defineExpose({
  locations,
  resetToDefaultView,
  refreshData,
});
</script>

<style scoped>
.location-table .table-row {
  transition: all 0.2s ease;
}
</style>
