<template>
    <div class="flex min-h-screen bg-gray-50">
        <!-- Main Content with full height -->
        <div class="flex-1 min-h-screen p-3 sm:p-4 md:p-6">
            <LocationHeader />
            <LocationFilters 
                v-model:search="searchQuery"
                v-model:sort="sortBy"
                @refresh="refreshData"
            />
            <AddLocationForm 
                @location-added="handleLocationAdded"
            />
            <LocationTable 
                :locations="filteredLocations"
                :sublocations="sublocations"
                :is-loading="isLoading || isSublocationsLoading"
                @edit-location="handleEditLocation"
                @delete-location="handleDeleteLocation"
                @add-sublocation="handleAddSublocation"
                @edit-sublocation="handleEditSublocation"
                @delete-sublocation="handleDeleteSublocation"
            />
        </div>
        
        <!-- Notification Component -->
        <BaseNotification />
        
        <!-- Edit Location Modal -->
        <EditLocationModal
            :is-visible="showEditModal"
            :location="locationToEdit"
            @close="handleCloseEditModal"
            @updated="handleLocationUpdated"
        />
        
        <!-- Add Sublocation Modal -->
        <AddSublocationModal
            :is-visible="showAddSublocationModal"
            :location-id="selectedLocationId"
            @close="handleCloseAddSublocationModal"
            @sublocation-added="handleSublocationAdded"
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
import { ref, computed } from 'vue';
import BaseNotification from '@/components/base/BaseNotification.vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import LocationHeader from './components/LocationHeader.vue';
import LocationFilters from './components/LocationFilters.vue';
import AddLocationForm from './components/AddLocationForm.vue';
import LocationTable from './components/LocationTable.vue';
import EditLocationModal from './components/EditLocationModal.vue';
import AddSublocationModal from '../sublocation/AddSublocationModal.vue';

// Import new composables
import { useLocationTable } from './composables/useLocationTable';
import { useLocationFilters } from './composables/useLocationFilters';
import { useLocationActions } from './composables/useLocationActions';
import { useNotification } from '@/composables/notification/useNotification';

const { showNotification } = useNotification();

// Use the new composables
const {
    locations,
    sublocations,
    isLoading,
    isSublocationsLoading,
    refreshData,
    handleLocationAdded,
    handleSublocationAdded
} = useLocationTable();

const {
    searchQuery,
    sortBy,
    filteredLocations
} = useLocationFilters(locations, sublocations);

const {
    // Modal states
    showEditModal,
    locationToEdit,
    showAddSublocationModal,
    selectedLocationId,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    
    // Action handlers
    handleEditLocation,
    handleCloseEditModal,
    handleLocationUpdated,
    handleDeleteLocation,
    handleAddSublocation,
    handleCloseAddSublocationModal,
    handleEditSublocation,
    handleDeleteSublocation,
    confirmDelete,
    cancelDelete
} = useLocationActions(refreshData, showNotification);
</script>

<style scoped>
.min-h-screen {
    min-height: 100vh;
    min-height: 100dvh;
}

@media (max-width: 640px) {
    .p-3 {
        padding: 0.75rem;
    }
}

@media (max-width: 480px) {
    .p-3 {
        padding: 0.5rem;
    }
}
</style>