<template>
    <div class="flex min-h-screen bg-gray-50">
        <!-- Sidebar - Full Height -->
        <div class="flex-shrink-0">
            <SideBar class="min-h-screen" />
        </div>
        
        <!-- Main Content with full height -->
        <div class="flex-1 min-h-screen p-3 sm:p-4 md:p-6">
            <!-- Location Management Section -->
            <div class="space-y-6">
                <!-- Location Header -->
                <LocationHeader @add-location="handleAddLocation" />
            
                 <!-- Add Location Modal -->
                <AddLocationForm
                    :is-visible="showAddLocationModal"
                    @close="handleCloseAddLocationModal"
                    @location-added="handleLocationAdded"
                />
                <!-- Location Table with integrated filters -->
                <LocationTable
                    :locations="filteredLocations"
                    :is-loading="isLoading"
                    :search="search"
                    :sort="sort"
                    @edit-location="handleEditLocation"
                    @delete-location="handleDeleteLocation"
                    @update:search="search = $event"
                    @update:sort="sort = $event"
                    @refresh="refreshData"
                />
            </div>
            
            <!-- Sublocation Management Section -->
            <div class="mt-8">
                <SubLocationView :locations="locations" />
            </div>
        </div>
        
       
        
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
import SideBar from '@/components/common/SideBar.vue';
import LocationHeader from './components/LocationHeader.vue';
import LocationFilters from './components/LocationFilters.vue';
import LocationTable from './components/LocationTable.vue';
import AddLocationForm from './components/AddLocationForm.vue';
import EditLocationModal from './components/EditLocationModal.vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import SubLocationView from '@/components/features/Organization/sublocation/SubLocationView.vue';
import { useLocationTable } from './composables/useLocationTable';
import { useLocationFilters } from './composables/useLocationFilters';
import { useLocationActions } from './composables/useLocationActions';
import { useNotification } from '@/composables/notification/useNotification';

// Use composables
const {
    locations,
    isLoading,
    refreshData
} = useLocationTable();

const {
    search,
    sort,
    filteredLocations
} = useLocationFilters(locations);

const { showNotification } = useNotification();

const {
    showAddLocationModal,
    showEditLocationModal,
    locationToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddLocation,
    handleCloseAddLocationModal,
    handleLocationAdded,
    handleEditLocation,
    handleCloseEditLocationModal,
    handleLocationUpdated,
    handleDeleteLocation,
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