<template>
    <div class="space-y-6">
        <!-- Add SubLocation Modal -->
        <AddSubLocationModal
            :is-visible="showAddSubLocationModal"
            :location-id="selectedLocationId"
            @close="handleCloseAddSubLocationModal"
            @sub-location-added="handleSubLocationAdded"
        />

        
        <!-- SubLocation Management -->
        <SubLocationTable
            v-if="props.locations && props.locations.length > 0 "
            :locations="props.locations"
            :sub-locations="subLocations"
            :is-loading="isLoading"
            :selected-location-id="internalSelectedLocationId"
            :selected-location="selectedLocation"
            :current-location="currentLocation"
            :is-location-loading="isLocationLoading"
            :search-query="searchQuery"
            :handle-location-change="handleSubLocationChange"
            :handle-search="handleSearch"
            @add-sub-location="handleAddSubLocation"
            @edit-sub-location="handleEditSubLocation"
            @delete-sub-location="handleDeleteSubLocation"
            @sub-location-selected="handleSubLocationSelected"
        />



        <!-- Edit Sub location Modal -->
        <EditSubLocationModal
            v-if="showEditSubLocationModal"
            :is-visible="showEditSubLocationModal"
            :sub-location="subLocationToEdit"
            @close="handleCloseEditSubLocationModal"
            @sub-location-updated="handleSubLocationUpdated"
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
import { watch, onMounted } from 'vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import SubLocationTable from './components/SubLocationTable.vue';
import AddSubLocationModal from './components/AddSubLocationModal.vue';
import EditSubLocationModal from './components/EditSubLocationModal.vue';
import { useSubLocationTable } from './composables/useSubLocationTable';
import { useSubLocationModals } from './composables/useSubLocationModals';

const props = defineProps({
    locations: {
        type: Array,
        default: () => []
    },
    selectedLocationId: {
        type: [String, Number],
        default: null
    }
});

// Define emit to forward subLocation selection to parent
const emit = defineEmits([
    'sub-location-selected',
    'sub-location-added',
    'sub-location-updated',
    'sub-location-deleted'
]);


// Use composables - now with integrated backend search
const {
    subLocations,
    isLoading,
    selectedLocationId: internalSelectedLocationId,
    selectedLocation,
    currentLocation,
    isLocationLoading,
    searchQuery,
    refreshData,
    handleSubLocationChange,
    handleSearch,
    resetToDefaultView,
} = useSubLocationTable(props.locations);


onMounted( async () => {
    if(internalSelectedLocationId.value) {
      await refreshData();
    }
});


// Watch for prop changes and update internal state
watch(() => props.selectedLocationId, (newLocationId) => {
    if (newLocationId !== internalSelectedLocationId.value) {
        handleSubLocationChange(newLocationId);
    }
}, { immediate: true });

// Use modal composable for dialog management
const {
    showAddSubLocationModal,
    showEditSubLocationModal,
    subLocationToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleSubLocationAdded,
    handleAddSubLocation,
    handleCloseAddSubLocationModal,
    handleEditSubLocation,
    handleCloseEditSubLocationModal,
    handleSubLocationUpdated,
    handleDeleteSubLocation,
    confirmDelete,
    cancelDelete
} = useSubLocationModals(subLocations, emit);

// Handle SubLocation selection and forward to parent
const handleSubLocationSelected = (subLocationId) => {
    emit('sub-location-selected', subLocationId);
};

defineExpose({
    subLocations,
    selectedLocationId: internalSelectedLocationId,
    resetToDefaultView
});
</script>

<style scoped>
.sub-location-table .table-row {
    transition: all 0.2s ease;
}
</style>
