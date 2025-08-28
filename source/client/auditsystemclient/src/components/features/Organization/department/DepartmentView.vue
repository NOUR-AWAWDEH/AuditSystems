<template>
    <div class="space-y-6">
        <!-- Add Department Modal -->
        <AddDepartmentModal
            :is-visible="showAddDepartmentModal"
            :sub-location-id="internalSelectedSubLocationId"
            @close="handleCloseAddDepartmentModal"
            @department-added="handleDepartmentAdded"
        />

        <!-- Department Management -->
        <DepartmentTable
            :sub-locations="props.subLocations"
            :departments="departments"
            :is-loading="isLoading"
            :selected-sub-location-id="internalSelectedSubLocationId"
            :selected-sub-location="selectedSubLocation"
            :current-sub-location="currentSubLocation"
            :is-sub-location-loading="isSubLocationLoading"
            :search-query="searchQuery"
            :handle-sub-location-change="handleDepartmentChange"
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
import { onMounted, watch } from 'vue';
import { useDepartmentModals } from './composables/useDepartmentModals';

const props = defineProps({
    subLocations: {
        type: Array,
        default: () => []
    },
    selectedSubLocationId: {
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
    selectedSubLocationId: internalSelectedSubLocationId,
    selectedSubLocation,
    currentSubLocation,
    isSubLocationLoading,
    searchQuery,
    refreshData,
    handleDepartmentChange,
    handleSearch,
    resetToDefaultView,
} = useDepartmentTable(props.subLocations);

onMounted( async () => {
    if(internalSelectedSubLocationId) {
        await refreshData();
    }
});

// Watch for prop changes and update internal state
watch(() => props.selectedSubLocationId, (newSubLocationId) => {
    if (newSubLocationId !== internalSelectedSubLocationId.value) {
        handleDepartmentChange(newSubLocationId);
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
    selectedSubLocationId: internalSelectedSubLocationId,
    resetToDefaultView
});
</script>

<style scoped>
.department-table .table-row {
    transition: all 0.2s ease;
}
</style>
