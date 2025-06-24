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
            <LocationTable 
                :locations="locations"
                :is-loading="isLoading"
                @edit-location="handleEditLocation"
                @delete-location="handleDeleteLocation"
            />
        </div>
        
        <!-- Notification Component -->
        <BaseNotification />
        
        <!-- Confirmation Dialog -->
        <ConfirmDialog
            v-model="showDeleteDialog"
            :title="'Delete Location'"
            :message="deleteDialogMessage"
            :highlight-text="locationToDelete?.name || ''"
            :confirm-text="'Delete'"
            :cancel-text="'Cancel'"
            @confirm="confirmDelete"
            @cancel="cancelDelete"
        />
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import SideBar from '@/components/common/SideBar.vue';
import BaseNotification from '@/components/base/BaseNotification.vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import LocationHeader from './components/LocationHeader.vue';
import AddLocationForm from './components/AddLocationForm.vue';
import LocationTable from './components/LocationTable.vue';
import { useNotification } from '@/composables/notification/useNotification';
import { useGetLocations } from '@/composables/organization/location/useGetLocations';

const showDeleteDialog = ref(false);
const deleteDialogMessage = ref('');
const locationToDelete = ref(null);

const { showNotification } = useNotification();
const { isLoading, locations, fetchLocations, addLocation, removeLocation } = useGetLocations();

const handleLocationAdded = (newLocation) => {
    addLocation({
        id: newLocation.id || Date.now(),
        ...newLocation,
        createdAt: new Date().toISOString()
    });
    showNotification(`Location "${newLocation.name}" has been added successfully!`, 'success');
};

const handleEditLocation = (location) => {
    console.log('Edit location:', location);
    showNotification('Edit functionality coming soon!', 'info');
    // TODO: Implement edit functionality
};

const handleDeleteLocation = (location) => {
    locationToDelete.value = location;
    deleteDialogMessage.value = `Are you sure you want to delete "${location.name}"? This action cannot be undone.`;
    showDeleteDialog.value = true;
};

const confirmDelete = () => {
    if (locationToDelete.value) {
        removeLocation(locationToDelete.value.id);
        showNotification(`Location "${locationToDelete.value.name}" has been deleted successfully!`, 'success');
        locationToDelete.value = null;
    }
};

const cancelDelete = () => {
    locationToDelete.value = null;
};

onMounted(() => {
    fetchLocations();
});
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