<template>
    <!--DepartmentView.vue-->
    <div class="space-y-6">
        <!-- Add Department Modal -->
        <AddDepartmentModal
            :is-visible="showAddDepartmentModal"
            :location-id="internalSelectedLocationId"
            @close="handleCloseAddDepartmentModal"
            @department-added="handleDepartmentAdded"
        />

        <!-- Department Management -->
        <DepartmentTable
            :locations="props.locations"
            :departments="departments"
            :is-loading="isLoading"
            :selected-location-id="internalSelectedLocationId"
            :selected-location="selectedLocation"
            :current-location="currentLocation"
            :is-location-loading="isLocationLoading"
            :search-query="searchQuery"
            :handle-location-change="handleDepartmentChange"
            :handle-search="handleSearch"
            @add-department="handleAddDepartment"
            @edit-department="handleEditDepartment"
            @delete-department="handleDeleteDepartment"
            @department-selected="handleDepartmentSelected"
        />

        <!-- Edit Department Modal -->
        <EditDepartmentModal
            v-if="showEditDepartmentModal"
            :is-visible="showEditDepartmentModal"
            :department="departmentToEdit"
            @close="handleCloseEditDepartmentModal"
            @department-updated="handleDepartmentUpdated"
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
import DepartmentTable from './components/DepartmentTable.vue';
import AddDepartmentModal from './components/AddDepartmentModal.vue';
import EditDepartmentModal from './components/EditDepartmentModal.vue';
import { useDepartmentTable } from './composables/useDepartmentTable';
import { watch } from 'vue';
import { useDepartmentModals } from './composables/useDepartmentModals';

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

// Define emit first
const emit = defineEmits([
    'department-selected',
    'department-added',
    'department-updated',
    'department-deleted',
]);

// Use composables - now with integrated backend search
const {
    departments,
    isLoading,
    selectedLocationId: internalSelectedLocationId,
    selectedLocation,
    currentLocation,
    isLocationLoading,
    searchQuery,
    handleDepartmentChange,
    handleSearch,
    resetToDefaultView,
} = useDepartmentTable(props.locations);

// Watch for prop changes and update internal state
watch(() => props.selectedLocationId, (newLocationId) => {
    if (newLocationId !== internalSelectedLocationId.value) {
        handleDepartmentChange(newLocationId);
    }
}, { immediate: true });

const {
    showAddDepartmentModal,
    showEditDepartmentModal,
    departmentToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddDepartment,
    handleCloseAddDepartmentModal,
    handleEditDepartment,
    handleCloseEditDepartmentModal,
    handleDepartmentAdded,
    handleDepartmentUpdated,
    handleDeleteDepartment,
    confirmDelete,
    cancelDelete
} = useDepartmentModals(departments, emit);

// Handle department selection and forward to parent
const handleDepartmentSelected = (departmentId) => {
    emit('department-selected', departmentId);
};

defineExpose({
    departments,
    selectedLocationId: internalSelectedLocationId,
    resetToDefaultView
});
</script>

<style scoped>
.department-table .table-row {
    transition: all 0.2s ease;
}
</style>