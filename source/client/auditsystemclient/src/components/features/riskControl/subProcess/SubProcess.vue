<template>
    <div class="space-y-6">
        <!-- Add SubProcess Modal -->
        <AddSubProcessModal
            :is-visible="showAddSubProcessModal"
            :audit-process-id="internalSelectedAuditProcessId"
            @close="handleCloseAddSubProcessModal"
            @sub-process-added="handleSubProcessAdded"
        />

        <!-- Show SubProcessTable only when audit processes exist -->
        <SubProcessTable
            v-if="hasAuditProcesses"
            :audit-processes="auditProcessesForComposable"
            :sub-processes="subProcesses"
            :is-loading="isLoading"
            :selected-audit-process-id="internalSelectedAuditProcessId"
            :selected-audit-process="selectedAuditProcess"
            :current-audit-process="currentAuditProcess"
            :is-audit-process-loading="isAuditProcessLoading"
            :search-query="searchQuery"
            :handle-audit-process-change="handleSubProcessChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-sub-process="handleAddSubProcess"
            @edit-sub-process="handleEditSubProcess"
            @delete-sub-process="handleDeleteSubProcess"
            @sub-process-selected="handleSubProcessSelected"
        />

        <!-- Show big department icon when no audit processes exist -->
        <div v-else class="text-center py-20">
            <div class="max-w-lg mx-auto">
                <div class="bg-gradient-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base">
                    <DepartmentIcon size="xxxl" custom-class="text-primary-shade" />
                </div>
                <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Processes Available</h3>
                <p class="text-gray-base mb-6 text-sm leading-relaxed">You need to create processes first before you can manage sub processes</p>
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
                                <li>Navigate to the Risk Control section</li>
                                <li>Create or select an audit process</li>
                                <li>Return here to manage sub processes</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Edit Sub Process Modal -->
        <EditSubProcessModal
            v-if="showEditSubProcessModal"
            :is-visible="showEditSubProcessModal"
            :sub-process="subProcessToEdit"
            @close="handleCloseEditSubProcessModal"
            @updated="handleSubProcessUpdated"
        />

        <!-- Confirmation Dialog -->
        <ConfirmDialog
            v-model="showDeleteDialog"
            :title="deleteDialogTitle"
            :message="deleteDialogMessage"
            :highlight-text="itemToDelete?.particular || ''"
            :confirm-text="'Delete'"
            :cancel-text="'Cancel'"
            @confirm="confirmDelete"
            @cancel="cancelDelete"
        />
    </div>
</template>

<script setup>
import { watch, onMounted, computed } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import SubProcessTable from './components/SubProcessTable.vue';
import AddSubProcessModal from './components/AddSubProcessModal.vue';
import EditSubProcessModal from './components/EditSubProcessModal.vue';
import { useSubProcessTable } from './composables/useSubProcessTable';
import { useSubProcessModals } from './composables/useSubProcessModals';
import DepartmentIcon from '@/components/common/icons/DepartmentIcon.vue';

const props = defineProps({
    auditProcesses: {
        type: Array,
        default: () => []
    },
    selectedAuditProcessId: {
        type: [String, Number],
        default: null
    }
});

const hasAuditProcesses = computed(() => {
    const processes = props.auditProcesses;
    
    // Handle direct arrays
    if (Array.isArray(processes)) {
        return processes.length > 0;
    }
    
    // Handle reactive ref objects that contain an auditProcesses array
    if (processes && typeof processes === 'object') {
        // Check if it has a nested auditProcesses array (from composable)
        if (processes.auditProcesses && Array.isArray(processes.auditProcesses)) {
            return processes.auditProcesses.length > 0;
        }
        
        // Check if it's a reactive ref with value property
        if ('value' in processes) {
            const value = processes.value;
            if (Array.isArray(value)) {
                return value.length > 0;
            }
            // Check nested auditProcesses in value
            if (value && value.auditProcesses && Array.isArray(value.auditProcesses)) {
                return value.auditProcesses.length > 0;
            }
        }
    }
    
    return false;
});


// Define emit first
const emit = defineEmits(['sub-process-selected']);

const { showNotification } = useNotification();

// Use composables - now with integrated backend search
// Pass the reactive ref or array properly
const auditProcessesForComposable = computed(() => {
    const processes = props.auditProcesses;
    
    // Handle direct arrays
    if (Array.isArray(processes)) {
        return processes;
    }
    
    // Handle object with nested auditProcesses array
    if (processes && typeof processes === 'object') {
        // Check for nested auditProcesses array (from composable)
        if (processes.auditProcesses && Array.isArray(processes.auditProcesses)) {
            return processes.auditProcesses;
        }
        
        // Check if it's a reactive ref with value property
        if ('value' in processes) {
            const value = processes.value;
            if (Array.isArray(value)) {
                return value;
            }
            // Check nested auditProcesses in value
            if (value && value.auditProcesses && Array.isArray(value.auditProcesses)) {
                return value.auditProcesses;
            }
        }
    }
    
    // Fallback to empty array
    return [];
});

const {
    subProcesses,
    isLoading,
    selectedAuditProcessId: internalSelectedAuditProcessId,
    selectedAuditProcess,
    currentAuditProcess,
    isAuditProcessLoading,
    searchQuery,
    refreshData,
    handleSubProcessAdded,
    handleSubProcessChange,
    handleSearch,
    resetToDefaultView,
} = useSubProcessTable(auditProcessesForComposable);

// Add onMounted to refresh data on page load
onMounted(async () => {
    // If there's a selected audit process, refresh the data
    if (internalSelectedAuditProcessId.value) {
        await refreshData();
    }
});

// Watch for prop changes and update internal state
watch(() => props.selectedAuditProcessId, (newAuditProcessId) => {
    if (newAuditProcessId !== internalSelectedAuditProcessId.value) {
        handleSubProcessChange(newAuditProcessId);
    }
}, { immediate: true });

// Watch for auditProcesses prop changes
watch(() => props.auditProcesses, (newProcesses) => {
    // Watch for changes in audit processes
}, { immediate: true, deep: true });

// Use modal composable for dialog management
const {
    showAddSubProcessModal,
    showEditSubProcessModal,
    subProcessToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddSubProcess,
    handleCloseAddSubProcessModal,
    handleEditSubProcess,
    handleCloseEditSubProcessModal,
    handleSubProcessUpdated,
    handleDeleteSubProcess,
    confirmDelete,
    cancelDelete
} = useSubProcessModals(refreshData, showNotification);

// Handle sub process selection and forward to parent
const handleSubProcessSelected = (subProcessId) => {
    emit('sub-process-selected', subProcessId);
};

defineExpose({
    subProcesses,
    selectedAuditProcessId: internalSelectedAuditProcessId,
    resetToDefaultView
});
</script>

<style scoped>
.sub-process-table .table-row {
    transition: all 0.2s ease;
}
</style>
