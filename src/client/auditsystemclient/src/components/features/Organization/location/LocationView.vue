<template>
    <div class="flex min-h-screen bg-gray-50">
        <!-- Sidebar - Full Height -->
        <div class="flex-shrink-0">
            <SideBar class="min-h-screen" />
        </div>
        
        <!-- Main Content with full height -->
        <div class="flex-1 min-h-screen p-3 sm:p-4 md:p-6">
            <LocationHeader />
            <AddLocationForm 
                @location-added="handleLocationAdded"
            />
            <LocationHierarchyTable 
                :locations="locations"
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
import { ref, onMounted, onBeforeUnmount } from 'vue';
import SideBar from '@/components/common/SideBar.vue';
import BaseNotification from '@/components/base/BaseNotification.vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import LocationHeader from './components/LocationHeader.vue';
import AddLocationForm from './components/AddLocationForm.vue';
import LocationHierarchyTable from './components/LocationHierarchyTable.vue';
import EditLocationModal from './components/EditLocationModal.vue';
import AddSublocationModal from '../sublocation/AddSublocationModal.vue';
import { useNotification } from '@/composables/notification/useNotification';
import { useGetLocations } from '@/composables/organization/location/useGetLocationsByUserId';
import { useDeleteLocation } from '@/composables/organization/location/useDeleteLocationById';
import { useGetSublocations } from '@/composables/organization/sublocation/useGetSublocations';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

const showDeleteDialog = ref(false);
const deleteDialogMessage = ref('');
const deleteDialogTitle = ref('');
const itemToDelete = ref(null);
const deleteType = ref(''); // 'location' or 'sublocation'

// Edit modal state
const showEditModal = ref(false);
const locationToEdit = ref(null);

// Add sublocation modal state
const showAddSublocationModal = ref(false);
const selectedLocationId = ref(null);

const { showNotification } = useNotification();
const { isLoading, locations, fetchLocations, removeLocation } = useGetLocations();
const { isLoading: isSublocationsLoading, sublocations, fetchSublocations } = useGetSublocations();
const { deleteLocation } = useDeleteLocation();
const { initCheck } = useAuthStatus();

// Function to refresh data
const refreshData = async () => {
  await initCheck();
  await fetchLocations();
  await fetchSublocations();
};

// Handle page visibility change to refresh data when user returns to tab
const handleVisibilityChange = () => {
  if (!document.hidden) {
    refreshData();
  }
};

// Handle focus event to refresh data when user returns to window
const handleFocus = () => {
  refreshData();
};

onMounted(async () => {
  await refreshData();
  
  // Add event listeners for page visibility and focus
  document.addEventListener('visibilitychange', handleVisibilityChange);
  window.addEventListener('focus', handleFocus);
});

onBeforeUnmount(() => {
  // Clean up event listeners
  document.removeEventListener('visibilitychange', handleVisibilityChange);
  window.removeEventListener('focus', handleFocus);
});

const handleLocationAdded = async (newLocation) => {
  await fetchLocations();
  // Show success notification
  showNotification(`Location "${newLocation.name}" has been added successfully!`, 'success');
};

const handleEditLocation = (location) => {
  locationToEdit.value = location;
  showEditModal.value = true;
};

const handleCloseEditModal = () => {
  showEditModal.value = false;
  locationToEdit.value = null;
};

const handleLocationUpdated = async (updatedLocation) => {
  // Refresh the locations list to get the updated data
  await fetchLocations();
  
  // Show success notification
  showNotification(`Location "${updatedLocation.name}" has been updated successfully!`, 'success');
  
  // Close the modal
  handleCloseEditModal();
};

const handleDeleteLocation = (location) => {
  itemToDelete.value = location;
  deleteType.value = 'location';
  deleteDialogTitle.value = 'Delete Location';
  deleteDialogMessage.value = `Are you sure you want to delete "${location.name}"? This action cannot be undone.`;
  showDeleteDialog.value = true;
};

// Sublocation handlers
const handleAddSublocation = (locationId) => {
  selectedLocationId.value = locationId;
  showAddSublocationModal.value = true;
};

const handleCloseAddSublocationModal = () => {
  showAddSublocationModal.value = false;
  selectedLocationId.value = null;
};

const handleSublocationAdded = async (newSublocation) => {
  await fetchSublocations();
  showNotification(`Sublocation "${newSublocation.name}" has been added successfully!`, 'success');
  handleCloseAddSublocationModal();
};

const handleEditSublocation = () => {
  // TODO: Implement edit sublocation functionality
  showNotification('Edit sublocation functionality coming soon!', 'info');
};

const handleDeleteSublocation = (sublocation) => {
  itemToDelete.value = sublocation;
  deleteType.value = 'sublocation';
  deleteDialogTitle.value = 'Delete Sublocation';
  deleteDialogMessage.value = `Are you sure you want to delete "${sublocation.name}"? This action cannot be undone.`;
  showDeleteDialog.value = true;
};

const confirmDelete = async () => {
  if (itemToDelete.value) {
    if (deleteType.value === 'location') {
      const result = await deleteLocation(itemToDelete.value.id, itemToDelete.value.name);
      
      if (result.success) {
        showNotification(`Location "${itemToDelete.value.name}" has been deleted successfully!`, 'success');
        removeLocation(itemToDelete.value.id);
      }
    } else if (deleteType.value === 'sublocation') {
      // TODO: Implement delete sublocation functionality
      showNotification('Delete sublocation functionality coming soon!', 'info');
    }
    
    itemToDelete.value = null;
    deleteType.value = '';
    showDeleteDialog.value = false;
  }
};

const cancelDelete = () => {
  itemToDelete.value = null;
  deleteType.value = '';
};
</script>

<style scoped>
/* Ensure full height layout */
.min-h-screen {
    min-height: 100vh;
    min-height: 100dvh; /* Dynamic viewport height for mobile browsers */
}

/* Enhanced mobile responsiveness */
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