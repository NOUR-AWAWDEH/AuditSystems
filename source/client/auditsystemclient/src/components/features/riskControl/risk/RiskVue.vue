<template>
  <!-- RiskVue.vue -->
    <div class="space-y-6">
        <!-- Add Risk Modal -->
        <AddRiskModal
            :is-visible="showAddRiskModal"
            :objective-id="internalSelectedObjectiveId"
            @close="handleCloseAddRiskModal"
            @risk-added="handleRiskAdded"
        />

        <!-- Show RiskTable only when objectives exist -->
        <RiskTable
            v-if="props.objectives && props.objectives.length > 0"
            :objectives="props.objectives"
            :risks="risks"
            :is-loading="isLoading"
            :selected-objective-id="internalSelectedObjectiveId"
            :selected-objective="selectedObjective"
            :current-objective="currentObjective"
            :is-objective-loading="isObjectiveLoading"
            :search-query="searchQuery"
            :handle-objective-change="handleRiskChange"
            :handle-search="handleSearch"
            :refresh-data="refreshData"
            @add-risk="handleAddRisk"
            @edit-risk="handleEditRisk"
            @delete-risk="handleDeleteRisk"
            @risk-selected="handleRiskSelected"
        />

        <!-- Show big Objective icon when no Objective  exist -->
        <div v-else class="text-center py-20">
            <div class="max-w-lg mx-auto">
                <div class="bg-gradient-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base">
                    <ObjectiveIcon size="xxxl" custom-class="text-primary-shade" />
                </div>
                <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Objectives Available</h3>
                <p class="text-gray-base mb-6 text-sm leading-relaxed">You need to create objectives first before you can manage risks</p>
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
                                <li>Create or select an objective</li>
                                <li>Return here to manage risks</li>
                            </ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Edit Risk Modal -->
        <EditRiskModal
            v-if="showEditRiskModal"
            :is-visible="showEditRiskModal"
            :risk="riskToEdit"
            @close="handleCloseEditRiskModal"
            @updated="handleRiskUpdated"
        />

        <!-- Confirmation Dialog -->
        <ConfirmDialog
            v-model="showDeleteDialog"
            :title="deleteDialogTitle"
            :message="deleteDialogMessage"
            :highlight-text="itemToDelete?.nameRisk || ''"
            :confirm-text="'Delete'"
            :cancel-text="'Cancel'"
            @confirm="confirmDelete"
            @cancel="cancelDelete"
        />
    </div>
</template>

<script setup>
import { watch, onMounted, ref  } from 'vue';
import { useNotification } from '@/composables/notification/useNotification';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import RiskTable from './components/RiskTable.vue';
import AddRiskModal from './components/AddRiskModal.vue';
import EditRiskModal from './components/EditRiskModal.vue';
import { useRiskTable } from './composables/useRiskTable';
import { useRiskModals } from './composables/useRiskModals';
import ObjectiveIcon from '@/components/common/icons/ObjectiveIcon.vue';

const props = defineProps({
    objectives: {
        type: Array,
        default: () => [],
    },
    selectedObjectiveId: {
        type: [String, Number],
        default: null
    }
});



// Define emit first
const emit = defineEmits(['risk-selected']);

const { showNotification } = useNotification();

// Use composables - now with integrated backend search

const {
    risks,
    isLoading,
    selectedObjectiveId : internalSelectedObjectiveId,
    selectedObjective,
    currentObjective,
    isObjectiveLoading,
    searchQuery,
    refreshData,
    handleRiskAdded,
    handleRiskChange,
    handleSearch,
    resetToDefaultView,
} = useRiskTable(props.objectives);

// Local ref to manage selectedObjectiveId
const localSelectedObjectiveId = ref(props.selectedObjectiveId);

// Add onMounted to refresh data on page load
onMounted(async () => {
    if (localSelectedObjectiveId.value) {
        await refreshData();
    }
});

// Watch for prop changes and update internal state
watch(
    () => props.selectedObjectiveId,
    async (newObjectiveId) => {
        localSelectedObjectiveId.value = newObjectiveId;
        if (newObjectiveId !== internalSelectedObjectiveId.value) {
            await handleRiskChange(newObjectiveId);
        }
    },
    { immediate: true }
);

// Watch for objectives prop changes
watch(
    () => props.objectives,
    async (newObjectives) => {
        if (newObjectives.length > 0 && localSelectedObjectiveId.value) {
            await refreshData();
        }
    },
    { immediate: true, deep: true }
);

// Use modal composable for dialog management
const {
    showAddRiskModal,
    showEditRiskModal,
    riskToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddRisk,
    handleCloseAddRiskModal,
    handleEditRisk,
    handleCloseEditRiskModal,
    handleRiskUpdated,
    handleDeleteRisk,
    confirmDelete,
    cancelDelete
} = useRiskModals(refreshData, showNotification);


// Handle risk selection and forward to parent
const handleRiskSelected = (riskId) => {
    emit('risk-selected', riskId);
};

defineExpose({
    risks,
    selectedObjectiveId: localSelectedObjectiveId,
    resetToDefaultView
});
</script>

<style scoped>
.risk-table .table-row {
    transition: all 0.2s ease;
}
</style>