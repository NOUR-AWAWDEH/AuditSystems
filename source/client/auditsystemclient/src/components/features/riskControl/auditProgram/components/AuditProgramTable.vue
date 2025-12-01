<template>
  <div>
    <!-- Audit Program Container -->
    <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
      <div class="p-3 sm:p-4 md:p-6">
        <!-- Header -->
        <div class="flex flex-col gap-4 mb-6">
          <AuditProgramHeader />

          <div class="flex flex-col sm:flex-row sm:items-center gap-4">
            <div class="w-full sm:min-w-64 sm:max-w-xs">
              <ExitControlSelector
                :exit-controls="exitControls"
                label="Select Exit Control (Adequate)"
                :selected-exit-control-id="internalSelectedExitControlId"
                @update:selected-exit-control="(id) => (internalSelectedExitControlId = id)"
                @exit-control-changed="(id) => (internalSelectedExitControlId = id)"
              />
            </div>
            <div class="w-full sm:w-auto">
              <SearchFilter
                :search="searchQuery"
                :search-placeholder="'Search Audit Programs...'"
                @update:search="handleSearch"
                class="w-full sm:w-auto"
              />
            </div>
          </div>
        </div>

        <!-- Selected Exit Control Info -->
        <div
          v-if="selectedExitControlId"
          class="mb-6 p-3 sm:p-4 bg-linear-to-r from-primary-soft to-indigo-200 rounded-lg border border-primary-base"
        >
          <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="flex items-start sm:items-center gap-2 flex-1 min-w-0">
              <ExitControlIcon size="lg" custom-class="text-blue-600" />
              <div class="flex-1 min-w-0">
                <p class="text-xs sm:text-sm text-gray-700 wrap-break-word">
                  <span>Showing Audit Programs for:</span>
                  <span class="ml-1 font-semibold text-primary-shade">
                    <span v-if="isExitControlLoading">Loading exit control...</span>
                    <span v-else>{{ currentExitControl?.description || "Loading..." }}</span>
                  </span>
                  <span v-if="searchQuery" class="ml-2 text-blue-600">
                    | Search: "{{ searchQuery }}"
                  </span>
                </p>
              </div>
            </div>

            <button
              @click="$emit('add-audit-program')"
              class="flex items-center justify-center gap-1 px-4 py-2 text-sm rounded-md bg-secondary-base hover:bg-secondary-shade text-white transition-all whitespace-nowrap"
            >
              <PlusIcon :stroke-width="3" class="w-5 h-5" />
              Add Audit Program
            </button>
          </div>
        </div>

        <!-- Desktop Table -->
        <div v-if="selectedExitControlId" class="hidden lg:block rounded-lg border border-gray-200">
          <ReusableTable
            :items="auditPrograms"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="audit-program-table min-w-full"
          >
            <template #cell-actions="{ item }">
              <div class="flex flex-col items-start justify-start xl:flex-row gap-2 min-w-max">
                <button
                  @click.stop="handleSelectAuditProgram(item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                  :class="{ 'bg-green-200': selectedAuditProgramId === item.id }"
                >
                  <CheckIcon
                    v-if="selectedAuditProgramId === item.id"
                    size="md"
                    custom-class="mr-1"
                  />
                  <SelectIcon v-else size="md" custom-class="mr-1" />
                  {{ selectedAuditProgramId === item.id ? "Selected" : "Select" }}
                </button>

                <button
                  @click.stop="$emit('edit-audit-program', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-audit-program', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <DeleteIcon size="sm" custom-class="mr-1" />
                  Delete
                </button>
              </div>
            </template>
          </ReusableTable>
        </div>

        <!-- Mobile Card View -->
        <ReusableMobileCardList
          v-if="selectedExitControlId"
          :items="auditPrograms"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="lg:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Description
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.description }}
                </div>
              </div>

              <div>
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Created At
                </div>
                <div class="text-gray-400 text-xs font-medium">
                  {{ formatDateSimple(item.createdAt) }}
                </div>
              </div>
            </div>
          </template>

          <template #card-actions="{ item }">
            <div class="flex w-full justify-start gap-2 mt-2 flex-wrap">
              <button
                @click.stop="handleSelectAuditProgram(item)"
                class="w-42 min-h-10 inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                :class="{ 'bg-green-200': selectedAuditProgramId === item.id }"
              >
                <CheckIcon v-if="selectedAuditProgramId === item.id" size="md" class="mr-1" />
                <SelectIcon v-else size="md" class="mr-1" />
                {{ selectedAuditProgramId === item.id ? "Selected" : "Select" }}
              </button>

              <button
                @click.stop="$emit('edit-audit-program', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" /> Edit
              </button>

              <button
                @click.stop="$emit('delete-audit-program', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" /> Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>

        <!-- No results while searching -->
        <div
          v-if="searchQuery && auditPrograms.length === 0 && !isLoading"
          class="text-center py-4 border-t border-gray-200"
        >
          <button
            @click="handleSearch('')"
            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 transition-all duration-200"
          >
            <CloseIcon size="sm" stroke-width="3" class="mr-1" />
            Clear Search
          </button>
        </div>

        <!-- No Exit Control Selected -->
        <div v-else-if="!selectedExitControlId" class="text-center py-16">
          <ExitControlIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />
          <h3 class="text-xl font-semibold text-secondary-shade mb-4">
            Please select an Exit Control to view audit programs
          </h3>
          <p class="text-gray-500 mb-6">
            Choose an exit control from the dropdown above to get started
          </p>
          <div v-if="!exitControls || exitControls.length === 0">
            <p class="text-sm text-gray-400">
              Please add an exit control first to manage audit programs
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, watch } from "vue";
import ReusableTable from "@/components/common/ReusableTable.vue";
import ReusableMobileCardList from "@/components/common/ReusableMobileCardList.vue";
import AuditProgramHeader from "./AuditProgramHeader.vue";
import SearchFilter from "@/components/common/SearchFilter.vue";
import ExitControlIcon from "@/components/common/icons/RiskIcon.vue";
import PlusIcon from "@/components/common/icons/PlusIcon.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import CheckIcon from "@/components/common/icons/CheckIcon.vue";
import SelectIcon from "@/components/common/icons/SelectIcon.vue";
import ExitControlSelector from "../../exitControl/components/ExitControlSelector.vue";
import { formatDateSimple } from "@/utils/dateUtils";

const props = defineProps({
  exitControls: {
    type: Array,
    default: () => [],
  },
  auditPrograms: {
    type: Array,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  selectedExitControlId: {
    type: [String, Number],
    default: null,
  },
  selectedExitControl: {
    type: Object,
    default: () => null,
  },
  currentExitControl: {
    type: Object,
    default: () => null,
  },
  isExitControlLoading: {
    type: Boolean,
    default: false,
  },
  searchQuery: {
    type: String,
    default: "",
  },
  handleExitControlChange: {
    type: Function,
    default: () => {},
  },
  handleSearch: {
    type: Function,
    default: () => {},
  },
});

const internalSelectedExitControlId = ref(props.selectedExitControlId);

watch(
  () => props.selectedExitControlId,
  (newVal) => {
    internalSelectedExitControlId.value = newVal;
  }
);

watch(internalSelectedExitControlId, (newVal) => {
  props.handleExitControlChange(newVal);
});

const emit = defineEmits([
  "add-audit-program",
  "edit-audit-program",
  "delete-audit-program",
  "audit-program-selected",
]);

// Audit program selection state
const selectedAuditProgramId = ref(null);

// Handle audit program selection from table row or button
const handleSelectAuditProgram = (auditProgram) => {
  selectedAuditProgramId.value = auditProgram.id;
  emit("audit-program-selected", auditProgram.id);
};

const auditProgramColumns = [
  { key: "description", label: "Description" },
  { key: "actionOwner", label: "Action Owner" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedColumns = computed(() => {
  return [...auditProgramColumns, { key: "actions", label: "Actions" }];
});

const noDataMessage = computed(() => {
  if (props.searchQuery) {
    return `No Audit Programs found for "${props.searchQuery}".`;
  }
  return "No Audit Programs found for this Exit Control.";
});
</script>
