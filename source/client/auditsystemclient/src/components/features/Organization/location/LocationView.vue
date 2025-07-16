<template>
    <div class="space-y-6">
        <!-- Add Location Modal -->
        <AddLocationForm
            :is-visible="showAddLocationModal"
            @close="handleCloseAddLocationModal"
            @location-added="handleLocationAdded"
        />
        
        <!-- Location Table with integrated selection -->
        <LocationTable
            :locations="locations"
            :is-loading="isLoading"
            :search="search"
            @edit-location="handleEditLocation"
            @delete-location="handleDeleteLocation"
            @update:search="search = $event"
            @refresh="refreshData"
            @location-selected="handleLocationSelected"
        />
        
        <!-- Edit Location Modal -->
        <EditLocationModal
            v-if="showEditLocationModal"
            :is-visible="showEditLocationModal"
            :location="locationToEdit"
            @close="handleCloseEditLocationModal"
            @updated="handleLocationUpdated"
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
import LocationTable from './components/LocationTable.vue';
import AddLocationForm from './components/AddLocationForm.vue';
import EditLocationModal from './components/EditLocationModal.vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import { useLocationTable } from './composables/useLocationTable';
import { useLocationActions } from './composables/useLocationActions';
import { useNotification } from '@/composables/notification/useNotification';

const emit = defineEmits(['location-selected']);

const {
    locations,
    isLoading,
    searchQuery: search,
    refreshData
} = useLocationTable();

const { showNotification } = useNotification();

const handleLocationDeleted = () => {
    emit('location-selected', null);
};

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
    cancelDelete
} = useLocationActions(refreshData, showNotification, handleLocationDeleted);

const handleLocationSelected = (locationId) => {
    emit('location-selected', locationId);
};

// Expose locations for parent component
defineExpose({
    locations,
    refreshData
});
</script>