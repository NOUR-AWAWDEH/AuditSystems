<template>
    <div class="space-y-6">
        <!-- Add Audit Process Modal -->
        <AddAuditProcessModal
            :is-visible="showAddAuditProcessModal"
            :sub-department-id="internalSelectedSubDepartmentId"
            @close="handleCloseAddAuditProcessModal"
            @audit-process-added="handleAuditProcessAdded"
        />

        <!-- Audit Process Management -->
        <AuditProcessTable
            :sub-departments="props.subDepartments"
            :audit-processes="auditProcesses"
            :is-loading="isLoading"
            :selected-sub-department-id="internalSelectedSubDepartmentId"
            :selected-sub-department="selectedSubDepartment"
            :current-sub-department="currentSubDepartment"
            :is-sub-department-loading="isSubDepartmentLoading"
            :search-query="searchQuery"
            :handle-sub-department-change="handleAuditProcessChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-audit-process="handleAddAuditProcess"
            @edit-audit-process="handleEditAuditProcess"
            @delete-audit-process="handleDeleteAuditProcess"
            @audit-process-selected="handleAuditProcessSelected"
        />
        <!-- Edit Audit Process Modal -->
        <EditAuditProcessModal
            v-if="showEditAuditProcessModal"
            :is-visible="showEditAuditProcessModal"
            :audit-process="auditProcessToEdit"
            @close="handleCloseEditAuditProcessModal"
            @updated="handleAuditProcessUpdated"
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
import { useNotification } from '@/composables/notification/useNotification';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import AuditProcessTable from './components/AuditProcessTable.vue';
import AddAuditProcessModal from './components/AddAuditProcessModal.vue';
import EditAuditProcessModal from './components/EditAuditProcessModal.vue';
import { useAuditProcessTable } from './composables/useAuditProcessTable';
import { useAuditProcessActions } from './composables/useAuditProcessActions';
import { useAuditProcessModals } from './composables/useAuditProcessModals';

const props = defineProps({
    subDepartments: {
        type: Array,
        default: () => []
    },
    selectedSubDepartmentId: {
        type: [String, Number],
        default: null
    }
});

// Define emit first
const emit = defineEmits(['audit-process-selected']);

const { showNotification } = useNotification();

// Use composables - now with integrated backend search
const {
    auditProcesses,
    isLoading,
    selectedSubDepartmentId: internalSelectedSubDepartmentId,
    selectedSubDepartment,
    currentSubDepartment,
    isSubDepartmentLoading,
    searchQuery,
    refreshData,
    handleAuditProcessAdded,
    handleAuditProcessChange,
    handleSearch,
    resetToDefaultView,
} = useAuditProcessTable(props.subDepartments);

// Add onMounted to refresh data on page load
onMounted(async () => {
    // If there's a selected sub-department, refresh the data
    if (internalSelectedSubDepartmentId.value) {
        await refreshData();
    }
});

// Watch for prop changes and update internal state
watch(() => props.selectedSubDepartmentId, (newSubDepartmentId) => {
    if (newSubDepartmentId !== internalSelectedSubDepartmentId.value) {
        handleAuditProcessChange(newSubDepartmentId); // Changed from handleSubDepartmentChange
    }
}, { immediate: true });

// Watch for subDepartments prop changes
watch(() => props.subDepartments, () => {
    // The composable should handle this automatically, but we can add logging here if needed
}, { immediate: true, deep: true });

// Use modal composable for dialog management
const {
    showAddAuditProcessModal,
    showEditAuditProcessModal,
    auditProcessToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddAuditProcess,
    handleCloseAddAuditProcessModal,
    handleEditAuditProcess,
    handleCloseEditAuditProcessModal,
    handleAuditProcessUpdated,
    handleDeleteAuditProcess,
    confirmDelete,
    cancelDelete
} = useAuditProcessModals(refreshData, showNotification);

// Remove the incorrect destructuring from useAuditProcessActions
// Keep only the actions that are actually returned by useAuditProcessActions

// Handle audit process selection and forward to parent
const handleAuditProcessSelected = (auditProcessId) => {
    emit('audit-process-selected', auditProcessId);
};

defineExpose({
    auditProcesses,
    selectedSubDepartmentId: internalSelectedSubDepartmentId,
    resetToDefaultView
});
</script>

<style scoped>
.audit-process-table .table-row {
    transition: all 0.2s ease;
}
</style>
