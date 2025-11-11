<template>
    <!--ControlDesignVue.vue-->
  <div class="space-y-6">
    <!-- Add Control Design Modal -->
    <AddControlDesignModal
      :is-visible="showAddControlDesignModal"
      :exit-control-id="internalSelectedExitControlId"
      @close="handleCloseAddControlDesignModal"
      @control-design-added="handleControlDesignAdded"
    />

    <!-- Show Control Design Table only when exit controls exist -->
    <ControlDesignTable
      v-if="filteredExitControls && filteredExitControls.length > 0"
      :exit-controls="filteredExitControls"
      :control-designs="controlDesigns"
      :is-loading="isLoading"
      :selected-exit-control-id="internalSelectedExitControlId"
      :selected-exit-control="selectedExitControl"
      :current-exit-control="currentExitControl"
      :is-exit-control-loading="isExitControlLoading"
      :search-query="searchQuery"
      :handle-exit-control-change="handleControlDesignChange"
      :handle-search="handleSearch"
      @add-control-design="handleAddControlDesign"
      @edit-control-design="handleEditControlDesign"
      @delete-control-design="handleDeleteControlDesign"
      @control-design-selected="handleControlDesignSelected"
    />

    <!-- Empty state -->
    <div v-else class="text-center py-20">
      <div class="max-w-lg mx-auto">
        <div
          class="bg-gradient-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32
          mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base"
        >
          <RiskIcon size="xxxl" custom-class="text-primary-shade" />
        </div>
        <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Control Designs Available</h3>
        <p class="text-gray-base mb-6 text-sm leading-relaxed">
          You need to create exit controls with inadequate status first before you can manage Control Designs
        </p>
        <div class="bg-white border border-tertiary-shade rounded-lg p-6">
          <div class="flex items-start space-x-3">
            <div class="flex-shrink-0">
              <svg
                class="w-6 h-6 text-tertiary-shade"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M12 9v2m0 4h.01m-6.938 
                  4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 
                  0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z"
                />
              </svg>
            </div>
            <div class="text-left">
              <h4 class="text-sm font-medium text-tertiary-shade mb-2">Getting Started</h4>
              <ol class="text-sm text-gray-base space-y-1 list-decimal list-inside">
                <li>Navigate to the Risk Control section</li>
                <li>Create or select an exit control</li>
                <li>Return here to manage Control Designs</li>
              </ol>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Edit Control Design Modal -->
    <EditControlDesignModal
      v-if="showEditControlDesignModal"
      :is-visible="showEditControlDesignModal"
      :control-design="controlDesignToEdit"
      @close="handleCloseEditControlDesignModal"
      @control-design-updated="handleControlDesignUpdated"
    />

    <!-- Confirmation Dialog -->
    <ConfirmDialog
      v-model="showDeleteDialog"
      :title="deleteDialogTitle"
      :message="deleteDialogMessage"
      :highlight-text="itemToDelete?.description || ''"
      confirm-text="Delete"
      cancel-text="Cancel"
      @confirm="confirmDelete"
      @cancel="cancelDelete"
    />
  </div>
</template>

<script setup>
import { watch, computed } from 'vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import ControlDesignTable from './components/ControlDesignTable.vue';
import AddControlDesignModal from './components/AddControlDesignModal.vue';
import EditControlDesignModal from './components/EditControlDesignModal.vue';
import { useControlDesignTable } from './composables/useControlDesignTable.js';
import { useControlDesignModals } from './composables/useControlDesignModals.js';
import RiskIcon from '@/components/common/icons/RiskIcon.vue';

const props = defineProps({
    exitControls: {
        type: Array,
        default: () => [],
    },
    selectedExitControlId: {
        type: [String, Number],
        default: null
    }
});

const emit = defineEmits([
    'control-design-selected',
    'control-design-added',
    'control-design-updated',
    'control-design-deleted'
]);

// Computed property to filter exitControls
const filteredExitControls = computed(() => {
  return props.exitControls.filter(control => control.inadequate);
});

// Composable
const {
    controlDesigns,
    isLoading,
    selectedExitControlId : internalSelectedExitControlId,
    selectedExitControl,
    currentExitControl,
    isExitControlLoading,
    searchQuery,
    handleControlDesignChange,
    handleSearch,
    resetToDefaultView,
} = useControlDesignTable(props.exitControls);

// ✅ Single watcher (merged both into one)
watch(
  () => props.selectedExitControlId,
  (newExitControlId) => {
    if (newExitControlId && !filteredExitControls.value.some(ec => ec.id === newExitControlId)) {
      // Clear invalid selection
      internalSelectedExitControlId.value = '';
      localStorage.removeItem("riskControlSelectedExitControlId");
    } else {
      internalSelectedExitControlId.value = newExitControlId || '';
    }

    // If still no selection, auto-select first valid exit control
    if (!internalSelectedExitControlId.value && filteredExitControls.value.length > 0) {
      const firstId = filteredExitControls.value[0].id;
      internalSelectedExitControlId.value = firstId;
      localStorage.setItem("riskControlSelectedExitControlId", firstId);
    }

    // Always trigger change with final ID
    handleControlDesignChange(internalSelectedExitControlId.value);
  },
  { immediate: true }
);

// Modal handling
const {
    showAddControlDesignModal,
    showEditControlDesignModal,
    controlDesignToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddControlDesign,
    handleEditControlDesign,
    handleControlDesignAdded,
    handleControlDesignUpdated,
    confirmDelete,
    handleCloseAddControlDesignModal,
    handleCloseEditControlDesignModal,
    handleDeleteControlDesign,
    cancelDelete
} = useControlDesignModals(controlDesigns, emit);

// Forward selection event
const handleControlDesignSelected = (controlDesign) => {
  emit('control-design-selected', controlDesign);
};

defineExpose({
  controlDesigns,
  selectedExitControlId: internalSelectedExitControlId,
  resetToDefaultView
});
</script>


<style scoped>
.control-design-table .table-row {
  transition: all 0.2s ease;
}
</style>