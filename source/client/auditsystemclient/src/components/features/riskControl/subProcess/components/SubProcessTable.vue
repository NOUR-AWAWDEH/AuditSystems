<template>
  <!--SubProcessTable.vue-->
  <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
    <div class="p-3 sm:p-4 md:p-6">
      <div class="flex flex-col gap-4 mb-6">
        <!-- Header Section -->
        <SubProcessHeader />
        <div class="flex flex-col sm:flex-row sm:items-center gap-4">
          <div class="w-full sm:min-w-64 sm:max-w-xs">
            <AuditProcessSelector
              :auditProcesses="auditProcesses"
              :selectedAuditProcessId="internalSelectedAuditProcessId"
              @update:selectedAuditProcess="(id) => (internalSelectedAuditProcessId = id)"
              @audit-process-changed="(id) => (internalSelectedAuditProcessId = id)"
            />
          </div>
          <div class="w-full sm:w-auto">
            <SearchFilter
              :search="searchQuery"
              :search-placeholder="'Search sub processes...'"
              @update:search="handleSearch"
              class="w-full sm:w-auto"
            />
          </div>
        </div>

        <!-- Selected AuditProcess Info section -->
        <div
          v-if="internalSelectedAuditProcessId"
          class="mb-6 p-3 sm:p-4 bg-linear-to-r from-primary-soft to-indigo-200 rounded-lg border border-primary-base"
        >
          <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="flex items-start sm:items-center gap-2 flex-1 min-w-0">
              <DepartmentIcon size="lg" custom-class="text-blue-600 mr-2" />

              <div class="flex-1 min-w-0">
                <p class="text-xs sm:text-sm text-gray-700 wrap-break-word">
                  <span>Showing Sub Processes for:</span>
                  <span class="ml-1 font-semibold text-primary-shade">
                    <span v-if="isAuditProcessLoading">Loading audit process...</span>
                    <span v-else>{{ currentAuditProcess?.processName || "Loading..." }}</span>
                  </span>
                  <span v-if="searchQuery" class="ml-2 text-blue-600">
                    | Search: "{{ searchQuery }}"
                  </span>
                </p>
              </div>
            </div>

            <button
              @click="$emit('add-sub-process')"
              class="flex items-center justify-center gap-1 px-4 py-2 text-sm rounded-md bg-secondary-base hover:bg-secondary-shade text-white transition-all whitespace-nowrap"
            >
              <PlusIcon :stroke-width="3" class="w-5 h-5" />
              Add Sub Process
            </button>
          </div>
        </div>

        <!-- Table Container - Show when AuditProcess is Selected -->
        <div
          v-if="internalSelectedAuditProcessId"
          class="hidden lg:block rounded-lg border border-gray-200"
        >
          <ReusableTable
            :items="subProcesses"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="sub-process-table min-w-full"
          >
            <template #cell-actions="{ item }">
              <div class="flex flex-col justify-start items-start xl:flex-row gap-2 min-w-max">
                <button
                  @click.stop="handleSelectSubProcess(item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                  :class="{ 'bg-green-200': selectedSubProcessId === item.id }"
                >
                  <CheckIcon
                    v-if="selectedSubProcessId === item.id"
                    size="md"
                    custom-class="mr-1"
                  />
                  <SelectIcon v-else size="md" custom-class="mr-1" />
                  {{ selectedSubProcessId === item.id ? "Selected" : "Select" }}
                </button>

                <button
                  @click.stop="$emit('edit-sub-process', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-primary-shade bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-sub-process', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
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
          v-if="internalSelectedAuditProcessId"
          :items="subProcesses"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="lg:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Particular
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.particular }}
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
                @click.stop="handleSelectSubProcess(item)"
                class="w-42 min-h-10 inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                :class="{ 'bg-green-200': selectedSubProcessId === item.id }"
              >
                <CheckIcon v-if="selectedSubProcessId === item.id" size="md" class="mr-1" />
                <SelectIcon v-else size="md" class="mr-1" />
                {{ selectedSubProcessId === item.id ? "Selected" : "Select" }}
              </button>

              <button
                @click.stop="$emit('edit-sub-process', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" />
                Edit
              </button>

              <button
                @click.stop="$emit('delete-sub-process', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" /> Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>

        <!-- Clear Search Button (shown when no results and searching) -->
        <div
          v-if="searchQuery && subProcesses.length === 0 && !isLoading"
          class="text-center py-4 border-t border-gray-200"
        >
          <button
            @click="handleSearch('')"
            class="inline-flex items-center px-4 py-2 text-sm font-medium text-blue-600 bg-blue-50 rounded-md hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-all duration-200"
          >
            <CloseIcon size="sm" stroke-width="3" custom-class="mr-1" />
            Clear Search
          </button>
        </div>

        <!-- No audit-process Selected -->
        <div v-else-if="!internalSelectedAuditProcessId" class="text-center py-16">
          <ProcessIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />
          <h3 class="text-xl font-semibold text-secondary-shade mb-4">
            Please select an audit-process to view Audit Processes
          </h3>
          <p class="text-gray-500 mb-6">
            Choose an audit-process from the dropdown above to get started
          </p>
          <div v-if="!subDepartments || subDepartments.length === 0">
            <p class="text-sm text-gray-400">
              Please add a Audit Process first to manage Sub Processes
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
import SearchFilter from "@/components/common/SearchFilter.vue";
import AuditProcessSelector from "@/components/features/riskControl/auditProcess/components/AuditProcessSelector.vue";
import DepartmentIcon from "@/components/common/icons/DepartmentIcon.vue";
import PlusIcon from "@/components/common/icons/PlusIcon.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import CheckIcon from "@/components/common/icons/CheckIcon.vue";
import SelectIcon from "@/components/common/icons/SelectIcon.vue";
import ProcessIcon from "@/components/common/icons/ProcessIcon.vue";
import SubProcessHeader from "./SubProcessHeader.vue";
import { formatDateSimple } from "@/utils/dateUtils.js";

const props = defineProps({
  auditProcesses: {
    type: Array,
    default: () => [],
  },
  subProcesses: {
    type: Array,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  selectedAuditProcessId: {
    type: String,
    default: "",
  },
  selectedAuditProcess: {
    type: Object,
    default: () => null,
  },
  currentAuditProcess: {
    type: Object,
    default: () => null,
  },
  isAuditProcessLoading: {
    type: Boolean,
    default: false,
  },
  searchQuery: {
    type: String,
    default: "",
  },
  handleAuditProcessChange: {
    type: Function,
    default: () => {},
  },
  handleSearch: {
    type: Function,
    default: () => {},
  },
  refreshData: {
    type: Function,
    default: () => {},
  },
});

const internalSelectedAuditProcessId = ref(props.selectedAuditProcessId);

watch(
  () => props.selectedAuditProcessId,
  (newVal) => {
    internalSelectedAuditProcessId.value = newVal;
  }
);

watch(internalSelectedAuditProcessId, (newVal) => {
  props.handleAuditProcessChange(newVal);
});

const emit = defineEmits([
  "add-sub-process",
  "edit-sub-process",
  "delete-sub-process",
  "sub-process-selected",
]);

// Sub Process selection state
const selectedSubProcessId = ref(null);

// Handle sub process selection from table row or button
const handleSelectSubProcess = (subProcess) => {
  selectedSubProcessId.value = subProcess.id;
  emit("sub-process-selected", subProcess.id);
};

const subProcessColumns = [
  { key: "particular", label: "Particular" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedColumns = computed(() => {
  return [...subProcessColumns, { key: "actions", label: "Actions" }];
});

const noDataMessage = computed(() => {
  if (props.searchQuery) {
    return `No sub processes found for "${props.searchQuery}".`;
  }
  return "No sub processes found for this audit process.";
});
</script>

<style scoped>
.sub-process-table .table-row {
  transition: all 0.2s ease;
  cursor: pointer;
}

.sub-process-table .table-row:hover {
  background-color: #f8fafc;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.sub-process-table .table-row.selected {
  background-color: #e6f7ff;
  border-left: 4px solid #3b82f6;
}
</style>
