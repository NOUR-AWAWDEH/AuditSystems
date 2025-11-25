<template>
  <!--ExitControlTable.vue-->
  <div class="bg-white rounded-xl shadow-lg border border-gray-200">
    <div class="p-3 sm:p-4 md:p-6">
      <!-- Objective and Filters - Stack on mobile -->
      <div class="flex flex-col gap-4 mb-6">
        <ExitControlHeader />

        <div class="flex flex-col sm:flex-row sm:items-center gap-4">
          <div class="w-full sm:min-w-64 sm:max-w-xs">
            <ObjectiveSelector
              :objectives="objectives"
              :selected-objective-id="internalSelectedObjectiveId"
              @update:selected-objective="(id) => (internalSelectedObjectiveId = id)"
              @objective-changed="(id) => (internalSelectedObjectiveId = id)"
            />
          </div>

          <div class="w-full sm:w-auto">
            <SearchFilter
              :search="searchQuery"
              :search-placeholder="'Search exit controls...'"
              @update:search="handleSearch"
            />
          </div>
        </div>
      </div>

      <!-- Selected objective Info -->
      <div
        v-if="internalSelectedObjectiveId"
        class="mb-6 p-3 sm:p-4 bg-linear-to-r from-primary-soft to-indigo-50 rounded-lg border border-primary-base"
      >
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
          <div class="flex items-start sm:items-center gap-2 flex-1 min-w-0">
            <ObjectiveIcon size="lg" custom-class="text-blue-600" />
            <div class="flex-1 min-w-0">
              <p class="text-xs sm:text-sm text-gray-700 wrap-break-word">
                <span>Showing Exit Controls for:</span>
                <span class="ml-1 font-semibold text-primary-base">
                  <span v-if="isObjectiveLoading">Loading objective...</span>
                  <span v-else>{{ currentObjective?.description || "Loading..." }}</span>
                </span>
                <span v-if="searchQuery" class="ml-2 text-primary-base">
                  | Search: "{{ searchQuery }}"
                </span>
              </p>
            </div>
          </div>

          <button
            @click="$emit('add-exit-control')"
            class="flex items-center justify-center gap-1 px-4 py-2 text-sm rounded-md bg-secondary-base hover:bg-secondary-shade text-white transition-all whitespace-nowrap"
          >
            <PlusIcon :stroke-width="3" />
            Add Exit Control
          </button>
        </div>
      </div>

      <!-- Desktop Table -->
      <div
        v-if="internalSelectedObjectiveId"
        class="hidden lg:block rounded-lg border border-gray-200"
      >
        <ReusableTable
          :items="exitControls"
          :columns="enhancedColumns"
          :is-loading="isLoading"
          :actions="[]"
          :no-data-message="noDataMessage"
          class="exit-control-table min-w-full"
        >
          <template #cell-inadequate="{ item }">
            <span
              :class="inadequateColorMap[item.inadequate]"
              class="flex flex-col items-center-safe justify-center-safe lg:px-2 2xl:flex-row gap-2 py-1 max-w-27 rounded-md font-bold"
            >
              {{ inadequateLabelMap[item.inadequate] }}
            </span>
          </template>

          <template #cell-actions="{ item }">
            <div class="flex flex-col justify-start items-start 2xl:flex-row gap-2 min-w-max">
              <button
                @click.stop="handleSelectExitControl(item)"
                class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                :class="{ 'bg-green-200': selectedExitControlId === item.id }"
              >
                <CheckIcon v-if="selectedExitControlId === item.id" size="md" custom-class="mr-1" />
                <SelectIcon v-else size="md" custom-class="mr-1" />
                {{ selectedExitControlId === item.id ? "Selected" : "Select" }}
              </button>

              <button
                @click.stop="$emit('edit-exit-control', item)"
                class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-green-200 transition-all duration-200"
              >
                <EditIcon size="sm" custom-class="mr-1" />
                Edit
              </button>

              <button
                @click.stop="$emit('delete-exit-control', item)"
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
        v-if="internalSelectedObjectiveId"
        :items="exitControls"
        :showNumber="true"
        :isLoading="isLoading"
        :noDataMessage="noDataMessage"
        class="lg:hidden"
      >
        <template #card-content="{ item }">
          <div class="space-y-4">
            <div class="border-b border-gray-100 pb-3">
              <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                {{ exitControlColumns[0].label }}
              </div>
              <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                {{ item.description }}
              </div>
            </div>

            <div class="border-b border-gray-100 pb-3">
              <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                {{ exitControlColumns[1].label }}
              </div>
              <div
                :class="inadequateColorMap[item.inadequate]"
                class="inline-block px-3 py-1 rounded-md text-sm font-medium"
              >
                {{ inadequateLabelMap[item.inadequate] }}
              </div>
            </div>

            <div class="border-b border-gray-100 pb-3">
              <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                {{ exitControlColumns[2].label }}
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
              @click.stop="handleSelectExitControl(item)"
              class="w-42 min-h-10 inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
              :class="{ 'bg-green-200': selectedExitControlId === item.id }"
            >
              <CheckIcon v-if="selectedExitControlId === item.id" size="md" class="mr-1" />
              <SelectIcon v-else size="md" class="mr-1" />
              {{ selectedExitControlId === item.id ? "Selected" : "Select" }}
            </button>

            <button
              @click.stop="$emit('edit-exit-control', item)"
              class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
            >
              <EditIcon size="sm" class="mr-1" /> Edit
            </button>

            <button
              @click.stop="$emit('delete-exit-control', item)"
              class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
            >
              <DeleteIcon size="sm" class="mr-1" /> Delete
            </button>
          </div>
        </template>
      </ReusableMobileCardList>

      <!-- No results while searching -->
      <div
        v-if="searchQuery && exitControls.length === 0 && !isLoading"
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

      <!-- No Objective Selected -->
      <div v-else-if="!internalSelectedObjectiveId" class="text-center py-16">
        <ObjectiveIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />
        <h3 class="text-xl font-semibold text-secondary-shade mb-4">
          Please select an Objective to view exit controls
        </h3>
        <p class="text-gray-500 mb-6">Choose an objective from the dropdown above to get started</p>
        <div v-if="!objectives || objectives.length === 0">
          <p class="text-sm text-gray-400">Please add an objective first to manage exit controls</p>
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
import ObjectiveSelector from "@/components/features/riskControl/objective/components/ObjectiveSelector.vue";
import ObjectiveIcon from "@/components/common/icons/ObjectiveIcon.vue";
import PlusIcon from "@/components/common/icons/PlusIcon.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import CheckIcon from "@/components/common/icons/CheckIcon.vue";
import SelectIcon from "@/components/common/icons/SelectIcon.vue";
import ExitControlHeader from "./ExitControlHeader.vue";
import { formatDateSimple } from "@/utils/dateUtils";

const props = defineProps({
  objectives: {
    type: Array,
    default: () => [],
  },
  exitControls: {
    type: Array,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  selectedObjectiveId: {
    type: [String, Number],
    default: null,
  },
  selectedObjective: {
    type: Object,
    default: () => null,
  },
  currentObjective: {
    type: Object,
    default: () => null,
  },
  isObjectiveLoading: {
    type: Boolean,
    default: false,
  },
  searchQuery: {
    type: String,
    default: "",
  },
  handleObjectiveChange: {
    type: Function,
    default: () => {},
  },
  handleSearch: {
    type: Function,
    default: () => {},
  },
});

const internalSelectedObjectiveId = ref(props.selectedObjectiveId);

watch(
  () => props.selectedObjectiveId,
  (newVal) => {
    internalSelectedObjectiveId.value = newVal;
  }
);

watch(internalSelectedObjectiveId, (newVal) => {
  props.handleObjectiveChange(newVal);
});

const emit = defineEmits([
  "add-exit-control",
  "edit-exit-control",
  "delete-exit-control",
  "exit-control-selected",
]);

// Exit control selection state
const selectedExitControlId = ref(null);

// Handle exit control selection from table row or button
const handleSelectExitControl = (exitControl) => {
  selectedExitControlId.value = exitControl.id;
  emit("exit-control-selected", exitControl);
};

const exitControlColumns = [
  { key: "description", label: "Description" },
  { key: "inadequate", label: "Inadequate" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedColumns = computed(() => {
  return [...exitControlColumns, { key: "actions", label: "Actions" }];
});

const noDataMessage = computed(() => {
  if (props.searchQuery) {
    return `No Exit Controls found for "${props.searchQuery}".`;
  }
  return "No Exit Controls found for this Objective.";
});

const inadequateColorMap = {
  true: "bg-red-100 text-red-800 no-wrap", // Bad → Inadequate
  false: "bg-green-100 text-green-800 no-wrap", // Good → Adequate
};

const inadequateLabelMap = {
  true: "Inadequate",
  false: "Adequate",
};
</script>

<style scoped>
.exit-control-table .table-row {
  transition: all 0.2s ease;
  cursor: pointer;
}

.exit-control-table .table-row:hover {
  background-color: #f8fafc;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.exit-control-table .table-row.selected {
  background-color: #e6f7ff;
  border-left: 4px solid #3b82f6;
}
</style>
