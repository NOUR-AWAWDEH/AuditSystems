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
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import SideBar from '@/components/common/SideBar.vue';
import BaseNotification from '@/components/base/BaseNotification.vue';
import LocationHeader from './components/LocationHeader.vue';
import AddLocationForm from './components/AddLocationForm.vue';
import LocationTable from './components/LocationTable.vue';
import { useNotification } from '@/composables/notification/useNotification';

const isLoading = ref(false);
const locations = ref([]);

const { showNotification } = useNotification();

const handleLocationAdded = (newLocation) => {
    locations.value.unshift({
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
    const confirmed = confirm(`Are you sure you want to delete "${location.name}"? This action cannot be undone.`);
    
    if (confirmed) {
        locations.value = locations.value.filter(loc => loc.id !== location.id);
        showNotification(`Location "${location.name}" has been deleted successfully!`, 'success');
    }
};

onMounted(() => {
    // Load locations from API or storage
    // loadLocations();
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