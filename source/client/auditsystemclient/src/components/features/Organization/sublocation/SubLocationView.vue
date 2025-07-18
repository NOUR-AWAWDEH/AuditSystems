<template>
    <div class="space-y-6">
        <!-- SubLocation Management -->
        <SubLocationTable
            :locations="locations"
            :sub-locations="subLocations"
            :is-loading="isLoading"
            :selected-location-id="selectedLocationId"
            :selected-location="selectedLocation"
            :current-location="currentLocation"
            :is-location-loading="isLocationLoading"
            :search-query="searchQuery"
            :handle-location-change="handleLocationChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-sub-location="handleAddSubLocation"
            @edit-sub-location="handleEditSubLocation"
            @delete-sub-location="handleDeleteSubLocation"
            @sub-location-selected="handleSubLocationSelected"
        />

        <!-- Add SubLocation Modal -->
        <AddSubLocationModal
            :is-visible="showAddSubLocationModal"
            :location-id="selectedLocationId"
            @close="handleCloseAddSubLocationModal"
            @sub-location-added="handleSubLocationAdded"
        />

        <!-- Edit Sublocation Modal -->
        <EditSubLocationModal
            v-if="showEditSubLocationModal"
            :is-visible="showEditSubLocationModal"
            :sub-location="subLocationToEdit"
            @close="handleCloseEditSubLocationModal"
            @updated="handleSubLocationUpdated"
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
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import SubLocationTable from './components/SubLocationTable.vue';
import AddSubLocationModal from './components/AddSublocationModal.vue';
import EditSubLocationModal from './components/EditSublocationModal.vue';
import { useSubLocationTable } from './composables/useSubLocationTable';
import { useSubLocationActions } from './composables/useSubLocationActions';
import { useNotification } from '@/composables/notification/useNotification';

const props = defineProps({
    locations: {
        type: Array,
        default: () => []
    }
});

// Define emit to forward sublocation selection to parent
const emit = defineEmits(['sub-location-selected']);

const { showNotification } = useNotification();

// Use composables - now with integrated backend search
const {
    subLocations,
    isLoading,
    selectedLocationId,
    selectedLocation,
    currentLocation,
    isLocationLoading,
    searchQuery,
    refreshData,
    handleSubLocationAdded,
    handleLocationChange,
    handleSearch,
    resetToDefaultView,
} = useSubLocationTable(props.locations);

const {
    showAddSubLocationModal,
    showEditSubLocationModal,
    subLocationToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddSubLocation,
    handleCloseAddSubLocationModal,
    handleEditSubLocation,
    handleCloseEditSubLocationModal,
    handleSubLocationUpdated,
    handleDeleteSubLocation,
    confirmDelete,
    cancelDelete
} = useSubLocationActions(refreshData, showNotification);

// Handle SubLocation selection and forward to parent
const handleSubLocationSelected = (subLocationId) => {
    emit('sub-location-selected', subLocationId);
};

defineExpose({
    subLocations,
    selectedLocationId,
    resetToDefaultView
});
</script>

<style scoped>
.sub-location-table .table-row {
    transition: all 0.2s ease;
}
</style>