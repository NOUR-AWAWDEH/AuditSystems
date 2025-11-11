<template>
    <!--SubDepartmentView.vue-->
    <div class="space-y-6">
        <!-- Add SubDepartment Modal -->
        <AddSubDepartmentModal
            :is-visible="showAddSubDepartmentModal"
            :department-id="selectedDepartmentId"
            @close="handleCloseAddSubDepartmentModal"
            @sub-department-added="handleSubDepartmentAdded"
        />

        <SubDepartmentTable
            :departments="props.departments"
            :sub-departments="subDepartments"
            :is-loading="isLoading"
            :selected-department-id="internalSelectedDepartmentId"
            :selected-department="selectedDepartment"
            :current-department="currentDepartment"
            :is-department-loading="isDepartmentLoading"
            :search-query="searchQuery"
            :handle-department-change="handleSubDepartmentChange"
            :handle-search="handleSearch"
            @add-sub-department="handleAddSubDepartment"
            @edit-sub-department="handleEditSubDepartment"
            @delete-sub-department="handleDeleteSubDepartment"
            @sub-department-selected="handleSubDepartmentSelected"
        />

        <!-- Edit SubDepartment Modal -->
        <EditSubDepartmentModal
            v-if="showEditSubDepartmentModal"
            :is-visible="showEditSubDepartmentModal"
            :sub-department="subDepartmentToEdit"
            @close="handleCloseEditSubDepartmentModal"
            @sub-department-updated="handleSubDepartmentUpdated"
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
import { useSubDepartmentModals } from './composables/useSubDepartmentModals';
import { watch} from 'vue';

const props = defineProps({
    departments: {
        type: Array,
        default: () => []
    },
    selectedDepartmentId: {
        type: [String, Number],
        default: null
    }
});

const emit = defineEmits([
    'sub-department-selected',
    'sub-department-added',
    'sub-department-updated',
    'sub-department-deleted',
]);

// Use composables
const {
    subDepartments,
    isLoading,
    selectedDepartmentId: internalSelectedDepartmentId,
    selectedDepartment,
    currentDepartment,
    isDepartmentLoading,
    searchQuery,
    handleSubDepartmentChange,
    handleSearch,
    resetToDefaultView,
} = useSubDepartmentTable(props.departments);


// Watch for prop changes and update internal state
watch(() => props.selectedDepartmentId, (newDepartmentId) => {
    if (newDepartmentId !== internalSelectedDepartmentId.value) {
        handleSubDepartmentChange(newDepartmentId);
    }
}, { immediate: true });

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
    handleSubDepartmentAdded,
    handleSubDepartmentUpdated,
    handleDeleteSubDepartment,
    confirmDelete,
    cancelDelete
} = useSubDepartmentModals(subDepartments, emit);

// Handle subdepartment selection and forward to parent
const handleSubDepartmentSelected = (subdepartmentId) => {
    emit('sub-department-selected', subdepartmentId);
};

// Expose necessary properties and methods
defineExpose({
    subDepartments,
    selectedDepartmentId: internalSelectedDepartmentId,
    resetToDefaultView
});
</script>
