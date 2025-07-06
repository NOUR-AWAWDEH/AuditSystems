<template>
    <div class="space-y-6">
        <!-- Sublocation Management -->
        <SubLocationTable
            :locations="locations"
            :sublocations="filteredSublocations"
            :is-loading="isLoading"
            :selected-location-id="selectedLocationId"
            :selected-location="selectedLocation"
            :search="searchQuery"
            :sort="sortBy"
            @update:selectedLocationId="handleLocationChange"
            @update:search="searchQuery = $event"
            @update:sort="sortBy = $event"
            @refresh="refreshData"
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
import { computed } from 'vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import SubLocationTable from './SubLocationTable.vue';
import AddSublocationModal from './AddSublocationModal.vue';
import EditSublocationModal from './EditSublocationModal.vue';
import { useSubLocationTable } from './composables/useSubLocationTable';
import { useSubLocationFilters } from './composables/useSubLocationFilters';
import { useSubLocationActions } from './composables/useSubLocationActions';
import { useNotification } from '@/composables/notification/useNotification';

const props = defineProps({
    locations: {
        type: Array,
        default: () => []
    }
});

const { showNotification } = useNotification();

// Use composables
const {
    sublocations,
    isLoading,
    selectedLocationId,
    selectedLocation,
    refreshData,
    handleLocationChange,
    handleSublocationAdded
} = useSubLocationTable(props.locations);

const {
    searchQuery,
    sortBy,
    filteredSublocations
} = useSubLocationFilters(sublocations);

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
</script>

<style scoped>
.sublocation-table .table-row {
    transition: all 0.2s ease;
}
</style>