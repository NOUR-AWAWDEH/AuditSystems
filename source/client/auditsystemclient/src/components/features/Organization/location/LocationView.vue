<template>
    <div class="flex min-h-[calc(100vh-4rem)] bg-gray-50">
        <!-- Main Content with full height -->
        <div class="flex-1 min-h-[calc(100vh-4rem)] p-3 sm:p-4 md:p-6">
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
                    :locations="locations"
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
                <SubLocationView ref="subLocationViewRef" :locations="locations" />
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
import LocationHeader from './components/LocationHeader.vue';
import LocationTable from './components/LocationTable.vue';
import AddLocationForm from './components/AddLocationForm.vue';
import EditLocationModal from './components/EditLocationModal.vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import SubLocationView from '@/components/features/Organization/sublocation/SubLocationView.vue';
import { useLocationTable } from './composables/useLocationTable';
import { useLocationActions } from './composables/useLocationActions';
import { useNotification } from '@/composables/notification/useNotification';
import { ref } from 'vue';

// Use composables
// In the script setup section, replace useLocationFilters with direct table management
const {
    locations,
    isLoading,
    searchQuery: search,
    sortBy: sort,
    refreshData
} = useLocationTable();

const { showNotification } = useNotification();

// Create a ref to access the SubLocationView component
const subLocationViewRef = ref(null);

// Callback function to handle location deletion
const handleLocationDeleted = (deletedLocationId) => {
    // Reset the sublocation view when a location is deleted
    if (subLocationViewRef.value && subLocationViewRef.value.resetToDefaultView) {
        subLocationViewRef.value.resetToDefaultView();
    }
};

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
} = useLocationActions(refreshData, showNotification, handleLocationDeleted);
</script>

<style scoped>
.min-h-\[calc\(100vh-4rem\)\] {
    min-height: calc(100vh - 4rem);
    min-height: calc(100dvh - 4rem);
}

@media (max-width: 640px) {
    .p-3 {
        padding: 0.75rem;
    }
    
    /* Ensure proper spacing on mobile */
    .space-y-6 > * + * {
        margin-top: 1rem;
    }
    
    .mt-8 {
        margin-top: 1.5rem;
    }
}

@media (max-width: 480px) {
    .p-3 {
        padding: 0.5rem;
    }
    
    /* Tighter spacing on very small screens */
    .space-y-6 > * + * {
        margin-top: 0.75rem;
    }
    
    .mt-8 {
        margin-top: 1rem;
    }
}
</style>