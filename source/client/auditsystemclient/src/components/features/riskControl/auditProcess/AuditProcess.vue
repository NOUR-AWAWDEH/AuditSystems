// AuditProcess.vue

<template>
    <div class="space-y-6">
        <!-- Add Audit Process Modal -->
        <AddAuditProcessModal
            :is-visible="showAddAuditProcessModal"
            :sub-department-id="internalSelectedSubDepartmentId"
            @close="handleCloseAddAuditProcessModal"
            @audit-process-added="handleAuditProcessAdded"
        />

        <!-- Show AuditProcessTable only when sub-departments exist -->
        <AuditProcessTable
            v-if="props.subDepartments && props.subDepartments.length > 0"
            :sub-departments="props.subDepartments"
            :audit-processes="computedAuditProcesses"
            :is-loading="isLoading"
            :selected-sub-department-id="internalSelectedSubDepartmentId"
            :selected-sub-department="selectedSubDepartment"
            :current-sub-department="currentSubDepartment"
            :is-sub-department-loading="isSubDepartmentLoading"
            :search-query="searchQuery"
            :handle-sub-department-change="handleAuditProcessChange"
            :handle-search="handleSearch"
            @add-audit-process="handleAddAuditProcess"
            @edit-audit-process="handleEditAuditProcess"
            @delete-audit-process="handleDeleteAuditProcess"
            @audit-process-selected="handleAuditProcessSelected"
        />

        <!-- Show big department icon when no sub-departments exist -->
        <div v-else class="text-center py-20">
            <div class="max-w-lg mx-auto">
                <div class="bg-gradient-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base">
                    <DepartmentIcon size="xxxl" custom-class="text-primary-shade" />
                </div>
                <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Sub-Departments Available</h3>
                <p class="text-gray-base mb-6 text-sm leading-relaxed">You need to create sub-departments first before you can manage audit processes</p>
                <div class="bg-white border border-tertiary-shade rounded-lg p-6">
                    <div class="flex items-start space-x-3">
                        <div class="flex-shrink-0">
                            <svg class="w-6 h-6 text-tertiary-shade" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z"></path>
                            </svg>
                        </div>
                        <div class="text-left">
                            <h4 class="text-sm font-medium text-tertiary-shade mb-2">Getting Started</h4>
                            <ol class="text-sm text-gray-base space-y-1 list-decimal list-inside">
                                <li>Navigate to the Organization section</li>
                                <li>Create or select a location</li>
                                <li>Add sub-locations and departments</li>
                                <li>Create sub-departments</li>
                                <li>Return here to manage audit processes</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Edit Audit Process Modal -->
        <EditAuditProcessModal
            v-if="showEditAuditProcessModal"
            :is-visible="showEditAuditProcessModal"
            :audit-process="auditProcessToEdit"
            @close="handleCloseEditAuditProcessModal"
            @audit-process-updated="handleAuditProcessUpdated"
        />

        <!-- Confirmation Dialog -->
        <ConfirmDialog
            v-model="showDeleteDialog"
            :title="deleteDialogTitle"
            :message="deleteDialogMessage"
            :highlight-text="itemToDelete?.processName || ''"
            :confirm-text="'Delete'"
            :cancel-text="'Cancel'"
            @confirm="confirmDelete"
            @cancel="cancelDelete"
        />
    </div>
</template>

<script setup>
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import AuditProcessTable from './components/AuditProcessTable.vue';
import AddAuditProcessModal from './components/AddAuditProcessModal.vue';
import EditAuditProcessModal from './components/EditAuditProcessModal.vue';
import { useAuditProcessTable } from './composables/useAuditProcessTable';
import { useAuditProcessModals } from './composables/useAuditProcessModals';
import DepartmentIcon from '@/components/common/icons/DepartmentIcon.vue';
import { onMounted } from 'vue';

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
const emit = defineEmits([
    'audit-process-selected',
    'audit-process-added',
    'audit-process-updated',
    'audit-process-deleted'
]);



// Use composables - now with integrated backend search
const {
    isLoading,
    computedAuditProcesses,
    selectedSubDepartmentId: internalSelectedSubDepartmentId,
    selectedSubDepartment,
    currentSubDepartment,
    isSubDepartmentLoading,
    searchQuery,
    handleAuditProcessChange,
    handleSearch,
    resetToDefaultView,
} = useAuditProcessTable(props.subDepartments);

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
    handleEditAuditProcess,
    handleDeleteAuditProcess,
    handleCloseAddAuditProcessModal,
    handleAuditProcessAdded,
    handleAuditProcessUpdated,
    confirmDelete,
    handleCloseEditAuditProcessModal,
    cancelDelete
} = useAuditProcessModals(computedAuditProcesses, emit); // ← Pass computedAuditProcesses instead

// Handle audit process selection and forward to parent
const handleAuditProcessSelected = (auditProcessId) => {
    emit('audit-process-selected', auditProcessId);
};

defineExpose({
    auditProcesses: computedAuditProcesses, // ← Use computed here too
    selectedSubDepartmentId: internalSelectedSubDepartmentId,
    resetToDefaultView
});
</script>
