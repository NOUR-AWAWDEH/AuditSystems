<template>
  <!--LocationView.vue-->
  <div class="space-y-6">
    <!-- Add Location Form -->
    <AddLocationForm
      :is-visible="showAddLocationModal"
      :company-id="internalSelectedCompanyId"
      @close="handleCloseAddLocationModal"
      @location-added="handleLocationAdded"
    />

    <!-- Location Table with integrated selection -->
    <LocationTable
      v-if="props.companies && props.companies.length > 0"
      :companies="props.companies"
      :locations="locations"
      :is-loading="isLoading"
      :selected-company-id="internalSelectedCompanyId"
      :search-query="searchQuery"
      :handle-search="handleSearch"
      @edit-location="handleEditLocation"
      @delete-location="handleDeleteLocation"
      @location-selected="handleLocationSelected"
    />

    <!-- Show NoCompanyExist when no companies -->
    <NoCompanyExist v-else />

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
import { ref, watch } from "vue";
import NoCompanyExist from "@/components/features/Organization/company/components/NoCompanyExist.vue";

const props = defineProps({
  companies: {
    type: Array,
    default: () => [],
  },
  selectedCompanyId: {
    type: [String, Number],
    default: null,
  },
});

const internalSelectedCompanyId = ref(props.selectedCompanyId);

watch(() => props.selectedCompanyId, (newId) => {
  internalSelectedCompanyId.value = newId;
});

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
