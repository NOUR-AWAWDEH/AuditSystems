<template>
    <div class="space-y-6">
        <!-- Department and SubDepartment Management -->
        <SubDepartmentTable
            :departments="departments"
            :sub-departments="subDepartments"
            :is-loading="isLoading"
            :selected-department-id="selectedDepartmentId"
            :selected-department="selectedDepartment" 
            :current-department="currentDepartment"
            :is-department-loading="isDepartmentLoading"
            :search-query="searchQuery"
            :handle-department-change="handleDepartmentChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-sub-department="handleAddSubDepartment"
            @edit-sub-department="handleEditSubDepartment"
            @delete-sub-department="handleDeleteSubDepartment"
            @sub-department-selected="handleSubDepartmentSelected"
        />

        <!-- Add SubDepartment Modal -->
        <AddSubDepartmentModal
            :is-visible="showAddSubDepartmentModal"
            :department-id="selectedDepartmentId"
            @close="handleCloseAddSubDepartmentModal"
            @sub-department-added="handleSubDepartmentAdded"
        />

        <!-- Edit SubDepartment Modal -->
        <EditSubDepartmentModal
            v-if="showEditSubDepartmentModal"
            :is-visible="showEditSubDepartmentModal"
            :sub-department="subDepartmentToEdit"
            @close="handleCloseEditSubDepartmentModal"
            @updated="handleSubDepartmentUpdated"
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
import SubDepartmentTable from './components/SubDepartmentTable.vue';
import AddSubDepartmentModal from './components/AddSubDepartmentModal.vue'
import EditSubDepartmentModal from './components/EditSubDepartmentModal.vue';
import { useSubDepartmentTable } from './composables/useSubDepartmentTable';
import { useSubDepartmentActions } from './composables/useSubDepartmentActions';
import { useNotification } from '@/composables/notification/useNotification';

const props = defineProps({
    departments: {
        type: Array,
        default: () => []
    }
});

const emit = defineEmits(['sub-department-selected']);

const { showNotification } = useNotification();

// Use composables
const {
    subDepartments,
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
    showAddSubDepartmentModal,
    showEditSubDepartmentModal,
    subDepartmentToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddSubDepartment,
    handleCloseAddSubDepartmentModal,
    handleEditSubDepartment,
    handleCloseEditSubDepartmentModal,
    handleSubDepartmentUpdated,
    handleDeleteSubDepartment,
    confirmDelete,
    cancelDelete
} = useSubDepartmentActions(refreshData, showNotification);

// Handle subdepartment selection and forward to parent
const handleSubDepartmentSelected = (subdepartmentId) => {
    emit('sub-department-selected', subdepartmentId);
};

// Expose necessary properties and methods
defineExpose({
    subDepartments,
    selectedDepartmentId,
    resetToDefaultView
});
</script>

<style scoped>
.subdepartment-table .table-row {
    transition: all 0.2s ease;
}
</style>