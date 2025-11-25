<template>
  <!--AuditProcessTable.vue-->
  <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
    <div class="p-3 sm:p-4 md:p-6">
      <div class="flex flex-col gap-4 mb-6">
        <!-- Header Section -->
        <AuditProcessHeader />

        <div class="flex flex-col sm:flex-row sm:items-center gap-4">
          <div class="w-full sm:min-w-64 sm:max-w-xs">
            <SubDepartmentSelector
              :sub-departments="subDepartments"
              :selected-sub-department-id="internalSelectedSubDepartmentId"
              @update:selected-sub-department="(id) => (internalSelectedSubDepartmentId = id)"
              @sub-department-changed="(id) => (internalSelectedSubDepartmentId = id)"
            />
          </div>
          <div class="w-full sm:w-auto">
            <SearchFilter
              :search="searchQuery"
              :search-placeholder="'Search  processes...'"
              @update:search="handleSearch"
              class="w-full sm:w-auto"
            />
          </div>
        </div>

        <!-- Selected Sub-Department Info section -->
        <div
          v-if="internalSelectedSubDepartmentId"
          class="mb-6 p-3 sm:p-4 bg-linear-to-r from-primary-soft to-indigo-200 rounded-lg border border-primary-base"
        >
          <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="flex items-start sm:items-center gap-2 flex-1 min-w-0">
              <DepartmentIcon size="lg" custom-class="text-blue-600" />

              <div class="flex-1 min-w-0">
                <p class="text-xs sm:text-sm text-gray-700 wrap-break-word">
                  <span>Showing Audit Processes for:</span>
                  <span class="ml-1 font-semibold text-primary-shade">
                    <span v-if="isSubDepartmentLoading">Loading sub-department...</span>
                    <span v-else>{{ currentSubDepartment?.name || "Loading..." }}</span>
                  </span>
                  <span v-if="searchQuery" class="ml-2 text-blue-600">
                    | Search: "{{ searchQuery }}"
                  </span>
                </p>
              </div>
            </div>

            <button
              @click="$emit('add-audit-process')"
              class="flex items-center justify-center gap-1 px-4 py-2 text-sm rounded-md bg-secondary-base hover:bg-secondary-shade text-white transition-all whitespace-nowrap"
            >
              <PlusIcon :stroke-width="3" class="w-5 h-5" />
              Add Process
            </button>
          </div>
        </div>

        <!-- Desktop Table -->
        <div v-if="internalSelectedSubDepartmentId"
          class="hidden md:block rounded-lg border border-gray-200"
        >
          <ReusableTable
            :items="auditProcesses"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="audit-process-table min-w-full"
          >
            <template #cell-actions="{ item }">
              <div class="flex flex-col items-start justify-start xl:flex-row gap-2 min-w-max">
                <button
                  @click.stop="handleSelectAuditProcess(item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                  :class="{ 'bg-green-200': selectedAuditProcessId === item.id }"
                >
                  <CheckIcon v-if="selectedAuditProcessId === item.id" size="md" custom-class="mr-1" />
                  <SelectIcon v-else size="md" custom-class="mr-1" />
                  {{ selectedAuditProcessId === item.id ? "Selected" : "Select" }}
                </button>

               <button
                  @click.stop="$emit('edit-audit-process', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-primary-shade bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-audit-process', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200">
                  <DeleteIcon size="sm" custom-class="mr-1" />
                  Delete
                </button>
              </div>
            </template>
          </ReusableTable>
        </div>

        <!-- Mobile Card View -->
        <ReusableMobileCardList
          v-if="internalSelectedSubDepartmentId"
          :items="auditProcesses"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="md:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Process Name
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.processName }}
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
                @click.stop="handleSelectAuditProcess(item)"
                class="w-42 min-h-10 inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                :class="{ 'bg-green-200': selectedAuditProcessId === item.id }"
              >
                <CheckIcon v-if="selectedAuditProcessId === item.id" size="sm" class="mr-1" />
                <SelectIcon v-else size="sm" class="mr-1" />
                {{ selectedAuditProcessId === item.id ? "Selected" : "Select" }}
              </button>

              <button
                @click.stop="$emit('edit-audit-process', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" /> Edit
              </button>

              <button
                @click.stop="$emit('delete-audit-process', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" /> Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>

        <!-- Clear Search Button (shown when no results and searching) -->
        <div
          v-if="searchQuery && auditProcesses.length === 0 && !isLoading"
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

        <!-- No sub-department Selected -->
        <div v-else-if="!internalSelectedSubDepartmentId" class="text-center py-16">
          <DepartmentIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />
          <h3 class="text-xl font-semibold text-secondary-shade mb-4">Please select a sub-department to view Audit Processes</h3>
          <p class="text-gray-500 mb-6">Choose a sub-department from the dropdown above to get started</p>
          <div v-if="!subDepartments || subDepartments.length === 0">
            <p class="text-sm text-gray-400">Please add a sub-department first to manage Audit Processes</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, watch } from "vue";
import ReusableTable from "@/components/common/ReusableTable.vue";
import SearchFilter from "@/components/common/SearchFilter.vue";
import SubDepartmentSelector from "@/components/features/Organization/subdepartment/components/SubDepartmentSelector.vue";
import DepartmentIcon from "@/components/common/icons/DepartmentIcon.vue";
import PlusIcon from "@/components/common/icons/PlusIcon.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import CheckIcon from "@/components/common/icons/CheckIcon.vue";
import SelectIcon from "@/components/common/icons/SelectIcon.vue";
import AuditProcessHeader from "./AuditProcessHeader.vue";
import ReusableMobileCardList from "@/components/common/ReusableMobileCardList.vue";
import { formatDateSimple } from "@/utils/dateUtils.js";

const props = defineProps({
  subDepartments: Array,
  auditProcesses: Array,
  isLoading: Boolean,
  selectedSubDepartmentId: String,
  selectedSubDepartment: Object,
  currentSubDepartment: Object,
  isSubDepartmentLoading: Boolean,
  searchQuery: String,
  handleSubDepartmentChange: Function,
  handleSearch: Function,
});

const emit = defineEmits([
  "add-audit-process",
  "edit-audit-process",
  "delete-audit-process",
  "audit-process-selected",
]);

// Local ref for sub-department selection
const internalSelectedSubDepartmentId = ref(props.selectedSubDepartmentId);

// Sync internal state with parent prop
watch(
  () => props.selectedSubDepartmentId,
  (newVal) => {
    internalSelectedSubDepartmentId.value = newVal;
  }
);

// Emit change when internal selection changes
watch(
  internalSelectedSubDepartmentId,
  (newVal) => {
    props.handleSubDepartmentChange(newVal);
  },
  { immediate: true }
);

// Audit Process selection state
const selectedAuditProcessId = ref(null);

const handleSelectAuditProcess = (auditProcess) => {
  selectedAuditProcessId.value = auditProcess.id;
  emit("audit-process-selected", auditProcess.id);
};

// Table columns
const auditProcessColumns = [
  { key: "processName", label: "Name" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedColumns = computed(() => [
  ...auditProcessColumns,
  { key: "actions", label: "Actions" },
]);

// Message for empty data
const noDataMessage = computed(() => {
  return props.searchQuery
    ? `No processes found for "${props.searchQuery}".`
    : "No processes found for this sub department.";
});
</script>

<style scoped>
.audit-process-table .table-row {
  transition: all 0.2s ease;
  cursor: pointer;
}

.audit-process-table .table-row:hover {
  background-color: #f8fafc;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.audit-process-table .table-row.selected {
  background-color: #e6f7ff;
  border-left: 4px solid #3b82f6;
}
</style>
