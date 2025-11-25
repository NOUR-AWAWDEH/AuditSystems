<template>
  <div>
    <!-- Risk Container -->
    <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
      <div class="p-3 sm:p-4 md:p-6">
        <!-- Header -->
        <div class="flex flex-col gap-4 mb-6">
          <RiskHeader />

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
                :search-placeholder="'Search risks...'"
                @update:search="handleSearch"
                class="w-full sm:w-auto"
              />
            </div>
          </div>
        </div>

        <!-- Selected Objective Info -->
        <div
          v-if="selectedObjectiveId"
          class="mb-6 p-3 sm:p-4 bg-linear-to-r from-primary-soft to-indigo-200 rounded-lg border border-primary-base"
        >
          <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="flex items-start sm:items-center gap-2 flex-1 min-w-0">
              <ObjectiveIcon size="lg" custom-class="text-blue-600" />
              <div class="flex-1 min-w-0">
                <p class="text-xs sm:text-sm text-gray-700 wrap-break-word">
                  <span>Showing Risks for objective:</span>
                  <span class="ml-1 font-semibold text-primary-shade">
                    <span v-if="isObjectiveLoading">Loading objective...</span>
                    <span v-else>{{ currentObjective?.description || "Loading..." }}</span>
                  </span>
                  <span v-if="searchQuery" class="ml-2 text-blue-600">
                    | Search: "{{ searchQuery }}"
                  </span>
                </p>
              </div>
            </div>

            <button
              @click="$emit('add-risk')"
              class="flex items-center justify-center gap-1 px-4 py-2 text-sm rounded-md bg-secondary-base hover:bg-secondary-shade text-white transition-all whitespace-nowrap"
            >
              <PlusIcon :stroke-width="3" class="w-5 h-5" />
              Add Risk
            </button>
          </div>
        </div>

        <!-- Desktop Table -->
        <div v-if="selectedObjectiveId" class="hidden lg:block rounded-lg border border-gray-200">
          <ReusableTable
            :items="transformedRisks"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="risk-table min-w-full"
          >
            <template #cell-rate="{ item }">
              <span
                :class="rateColorMap[item.rate]"
                class="inline-block px-3 py-1 rounded-md text-sm font-medium"
              >
                {{ item.rate }}
              </span>
            </template>

            <template #cell-actions="{ item }">
              <div class="flex flex-col items-start justify-start xl:flex-row gap-2 min-w-max">
                <button
                  @click.stop="$emit('edit-risk', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-risk', item)"
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
          v-if="selectedObjectiveId"
          :items="transformedRisks"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="lg:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Name
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.riskName }}
                </div>
              </div>

              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Description
                </div>
                <div class="text-gray-700 text-sm leading-snug wrap-break-word">
                  {{ item.description }}
                </div>
              </div>

              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Rate Level
                </div>
                <div
                  :class="rateColorMap[item.rate]"
                  class="inline-block px-3 py-1 rounded-md text-sm font-medium"
                >
                  {{ item.rate }}
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
                @click.stop="$emit('edit-risk', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" /> Edit
              </button>

              <button
                @click.stop="$emit('delete-risk', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" /> Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>

        <!-- No results while searching -->
        <div
          v-if="searchQuery && risks.length === 0 && !isLoading"
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
        <div v-else-if="!selectedObjectiveId" class="text-center py-16">
          <ObjectiveIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />
          <h3 class="text-xl font-semibold text-secondary-shade mb-4">
            Please select an Objective to view risks
          </h3>
          <p class="text-gray-500 mb-6">
            Choose an objective from the dropdown above to get started
          </p>
          <div v-if="!objectives || objectives.length === 0">
            <p class="text-sm text-gray-400">Please add an objective first to manage risks</p>
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
import PlusIcon from "@/components/common/icons/PlusIcon.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import RiskHeader from "./RiskHeader.vue";
import ObjectiveSelector from "../../objective/components/ObjectiveSelector.vue";
import ObjectiveIcon from "@/components/common/icons/ObjectiveIcon.vue";
import { formatDateSimple } from "@/utils/dateUtils";

const props = defineProps({
  objectives: {
    type: Array,
    default: () => [],
  },
  risks: {
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
  refreshData: {
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

defineEmits(["add-risk", "edit-risk", "delete-risk"]);

const riskColumns = [
  { key: "riskName", label: "Name" },
  { key: "description", label: "Description" },
  { key: "rate", label: "Rate Level" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedColumns = computed(() => {
  return [...riskColumns, { key: "actions", label: "Actions" }];
});

const noDataMessage = computed(() => {
  if (props.searchQuery) {
    return `No Risks found for "${props.searchQuery}".`;
  }
  return "No Risks found for this Objective.";
});

const rateLevelMap = {
  "11111111-1111-1111-1111-111111111111": "Critical",
  "22222222-2222-2222-2222-222222222222": "High",
  "33333333-3333-3333-3333-333333333333": "Medium",
  "44444444-4444-4444-4444-444444444444": "Low",
  "55555555-5555-5555-5555-555555555555": "Minimal",
};

const rateColorMap = {
  Critical: "bg-red-100 text-red-800",
  High: "bg-orange-100 text-orange-800",
  Medium: "bg-yellow-100 text-yellow-800",
  Low: "bg-green-100 text-green-800",
  Minimal: "bg-blue-100 text-blue-800",
  Unknown: "bg-gray-100 text-gray-800",
};

const transformedRisks = computed(() => {
  return props.risks.map((risk) => {
    const rate = rateLevelMap[risk.rateId] || "Unknown";
    return {
      ...risk,
      rate,
    };
  });
});
</script>

<style scoped>
.risk-table .table-row {
  transition: all 0.2s ease;
  cursor: pointer;
}

.risk-table .table-row:hover {
  background-color: #f8fafc;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.risk-table .table-row.selected {
  background-color: #e6f7ff;
  border-left: 4px solid #3b82f6;
}
</style>
