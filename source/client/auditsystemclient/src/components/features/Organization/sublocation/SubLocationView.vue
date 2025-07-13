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
            :sort-by="sortBy"
            :handle-location-change="handleLocationChange"
            :handle-search="handleSearch"
            :handle-sort="handleSort"
            :refresh-data="refreshData"
            @add-sublocation="handleAddSublocation"
            @edit-sublocation="handleEditSublocation"
            @delete-sublocation="handleDeleteSublocation"
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
import SubLocationTable from './SubLocationTable.vue';
import AddSublocationModal from './AddSublocationModal.vue';
import EditSublocationModal from './EditSublocationModal.vue';
import { useSubLocationTable } from './composables/useSubLocationTable';
import { useSubLocationActions } from './composables/useSubLocationActions';
import { useNotification } from '@/composables/notification/useNotification';

const props = defineProps({
    locations: {
        type: Array,
        default: () => []
    }
});

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
    currentPage,
    pageSize,
    totalCount,
    refreshData,
    handleSublocationAdded,
    handleLocationChange,
    handleSearch,
    resetToDefaultView,
    initializeData
} = useSubLocationTable(props.locations);

// Remove useSubLocationFilters since search is now handled by the table
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

// Expose the reset method to parent
// Expose the resetToDefaultView method so parent components can call it
defineExpose({
    resetToDefaultView
});
</script>

<style scoped>
.sublocation-table .table-row {
    transition: all 0.2s ease;
}
</style>