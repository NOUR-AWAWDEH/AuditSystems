<template>
    <div class="space-y-6">
        <!-- Department and SubDepartment Management -->
        <SubDepartmentTable
            :departments="departments"
            :subdepartments="subdepartments"
            :is-loading="isLoading"
            :selected-department-id="selectedDepartmentId"
            :selected-department="selectedDepartment" 
            :current-department="currentDepartment"
            :is-department-loading="isDepartmentLoading"
            :search-query="searchQuery"
            :handle-department-change="handleDepartmentChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-subdepartment="handleAddSubdepartment"
            @edit-subdepartment="handleEditSubdepartment"
            @delete-subdepartment="handleDeleteSubdepartment"
            @subdepartment-selected="handleSubDepartmentSelected"
            @department-selected="handleDepartmentSelected"
        />

        <!-- Add SubDepartment Modal -->
        <AddSubDepartmentModal
            :is-visible="showAddSubdepartmentModal"
            :department-id="selectedDepartmentId"
            @close="handleCloseAddSubdepartmentModal"
            @subdepartment-added="handleSubdepartment"
        />

        <!-- Edit SubDepartment Modal -->
        <EditSubDepartmentModal
            v-if="showEditSubdepartmentModal"
            :is-visible="showEditSubdepartmentModal"
            :subdepartment="subdepartmentToEdit"
            @close="handleCloseEditSubdepartmentModal"
            @updated="handleSubdepartmentUpdated"
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
// Base Components
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';

// Feature Components
import SubDepartmentTable from './components/SubDepartmentTable.vue';
import AddSubDepartmentModal from './components/AddSubDepartmentModal.vue'
import EditSubDepartmentModal from './components/EditSubDepartmentModal.vue';

// Composables
import { useSubDepartmentTable } from './composables/useSubDepartmentTable';
import { useSubDepartmentActions } from './composables/useSubDepartmentActions';
import { useNotification } from '@/composables/notification/useNotification';

const props = defineProps({
    departments: {
        type: Array,
        default: () => []
    }
});

// Define emits for both department and subdepartment selections
const emit = defineEmits(['subdepartment-selected', 'department-selected']);

const { showNotification } = useNotification();

// Use composables with integrated department and subdepartment handling
const {
    subdepartments,
    isLoading,
    selectedDepartmentId,
    selectedDepartment,
    currentDepartment,
    isDepartmentLoading,
    searchQuery,
    refreshData,
    handleSubDepartmentAdded,
    handleDepartmentChange,
    handleSearch,
    resetToDefaultView,
} = useSubDepartmentTable(props.departments);

const {
    showAddSubdepartmentModal,
    showEditSubdepartmentModal,
    subdepartmentToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddSubdepartment,
    handleCloseAddSubdepartmentModal,
    handleSubdepartment,
    handleEditSubdepartment,
    handleCloseEditSubdepartmentModal,
    handleSubdepartmentUpdated,
    handleDeleteSubdepartment,
    confirmDelete,
    cancelDelete
} = useSubDepartmentActions(refreshData, showNotification);

// Handle department selection
const handleDepartmentSelected = (departmentId) => {
    emit('department-selected', departmentId);
};

// Handle subdepartment selection
const handleSubDepartmentSelected = (subdepartmentId) => {
    emit('subdepartment-selected', subdepartmentId);
};

// Expose necessary properties and methods
defineExpose({
    subdepartments,
    selectedDepartmentId,
    resetToDefaultView
});
</script>

<style scoped>
.subdepartment-table .table-row {
    transition: all 0.2s ease;
}
</style>