<template>
    <div class="space-y-6">
        <!-- Department Management -->
        <DepartmentTable
            :sublocations="sublocations"
            :departments="departments"
            :is-loading="isLoading"
            :selected-sublocation-id="selectedSubLocationId"
            :selected-sublocation="selectedSubLocation"
            :current-sublocation="currentSubLocation"
            :is-sublocation-loading="isSubLocationLoading"
            :search-query="searchQuery"
            :handle-sublocation-change="handleSubLocationChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-department="handleAddDepartment"
            @edit-department="handleEditDepartment"
            @delete-department="handleDeleteDepartment"
            @department-selected="handleDepartmentSelected"
        />

        <!-- Add Department Modal -->
        <AddDepartmentModal
            :is-visible="showAddDepartmentModal"
            :sub-location-id="selectedSubLocationId"
            @close="handleCloseAddDepartmentModal"
            @department-added="handleDepartmentAdded"
        />

        <!-- Edit Department Modal -->
        <EditDepartmentModal
            v-if="showEditDepartmentModal"
            :is-visible="showEditDepartmentModal"
            :department="departmentToEdit"
            @close="handleCloseEditDepartmentModal"
            @updated="handleDepartmentUpdated"
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
import { useDepartmentActions } from './composables/useDepartmentActions';
import { useNotification } from '@/composables/notification/useNotification';
import { watch } from 'vue';

const props = defineProps({
    sublocations: {
        type: Array,
        default: () => []
    },
    selectedSublocationId: {
        type: String,
        default: null
    }
});

// Define emit first
const emit = defineEmits(['department-selected']);

const { showNotification } = useNotification();

// Use composables - now with integrated backend search
const {
    departments,
    isLoading,
    selectedSubLocationId,
    selectedSubLocation,
    currentSubLocation,
    isSubLocationLoading,
    searchQuery,
    refreshData,
    handleDepartmentAdded,
    handleSubLocationChange,
    handleSearch,
    resetToDefaultView,
} = useDepartmentTable(props.sublocations);

// Watch for changes in the prop and update the composable's state
watch(() => props.selectedSublocationId, (newSublocationId) => {
    if (newSublocationId !== selectedSubLocationId.value) {
        handleSubLocationChange(newSublocationId);
    }
}, { immediate: true });

// Handle department selection and forward to parent
const handleDepartmentSelected = (departmentId) => {
    emit('department-selected', departmentId);
};

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
    handleDepartmentUpdated,
    handleDeleteDepartment,
    confirmDelete,
    cancelDelete
} = useDepartmentActions(refreshData, showNotification);

defineExpose({
    resetToDefaultView
});
</script>

<style scoped>
.department-table .table-row {
    transition: all 0.2s ease;
}
</style>

