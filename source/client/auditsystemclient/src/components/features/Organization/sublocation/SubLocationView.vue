<template>
    <div class="space-y-6">
        <!-- Sublocation Management -->
        <SubLocationTable
            :locations="locations"
            :sublocations="sublocations"
            :is-loading="isLoading"
            :selected-location-id="selectedLocationId"
            :selected-location="selectedLocation"
            :current-location="currentLocation"
            :is-location-loading="isLocationLoading"
            :search-query="searchQuery"
            :handle-location-change="handleLocationChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-sublocation="handleAddSublocation"
            @edit-sublocation="handleEditSublocation"
            @delete-sublocation="handleDeleteSublocation"
            @sublocation-selected="handleSublocationSelected"
        />

        <!-- Add Sublocation Modal -->
        <AddSublocationModal
            :is-visible="showAddSublocationModal"
            :location-id="selectedLocationId"
            @close="handleCloseAddSublocationModal"
            @sublocation-added="handleSublocationAdded"
        />

        <!-- Edit Sublocation Modal -->
        <EditSublocationModal
            v-if="showEditSublocationModal"
            :is-visible="showEditSublocationModal"
            :sublocation="sublocationToEdit"
            @close="handleCloseEditSublocationModal"
            @updated="handleSublocationUpdated"
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
import AddSublocationModal from './components/AddSublocationModal.vue';
import EditSublocationModal from './components/EditSublocationModal.vue';
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
const emit = defineEmits(['sublocation-selected']);

const { showNotification } = useNotification();

// Use composables - now with integrated backend search
const {
    sublocations,
    isLoading,
    selectedLocationId,
    selectedLocation,
    currentLocation,
    isLocationLoading,
    searchQuery,
    refreshData,
    handleSublocationAdded,
    handleLocationChange,
    handleSearch,
    resetToDefaultView,
} = useSubLocationTable(props.locations);

const {
    showAddSublocationModal,
    showEditSublocationModal,
    sublocationToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddSublocation,
    handleCloseAddSublocationModal,
    handleEditSublocation,
    handleCloseEditSublocationModal,
    handleSublocationUpdated,
    handleDeleteSublocation,
    confirmDelete,
    cancelDelete
} = useSubLocationActions(refreshData, showNotification);

// Handle sublocation selection and forward to parent
const handleSublocationSelected = (sublocationId) => {
    emit('sublocation-selected', sublocationId);
};

// In the script setup section, add:
defineExpose({
    sublocations,
    selectedLocationId, // Expose selected location ID
    resetToDefaultView
});
</script>

<style scoped>
.sublocation-table .table-row {
    transition: all 0.2s ease;
}
</style>