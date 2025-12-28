<template>
  <div>
    <!-- Recommendation Container -->
    <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
      <div class="p-3 sm:p-4 md:p-6">
        <!-- Header -->
        <div class="flex flex-col gap-4 mb-6">
          <RecommendationHeader />

          <div class="flex flex-col sm:flex-row sm:items-center gap-4">
            <div class="w-full sm:min-w-64 sm:max-w-xs">
              <ObservationSelector
                :observations="observations"
                :selected-observation-id="internalSelectedObservationId"
                @update:selected-observation="(id) => (internalSelectedObservationId = id)"
                @observation-changed="(id) => (internalSelectedObservationId = id)"
              />
            </div>
            <div class="w-full sm:w-auto">
              <SearchFilter
                :search="searchQuery"
                :search-placeholder="'Search Recommendations...'"
                @update:search="handleSearch"
                class="w-full sm:w-auto"
              />
            </div>
          </div>
        </div>

        <!-- Selected Observation Info -->
        <div
          v-if="selectedObservationId"
          class="mb-6 p-3 sm:p-4 bg-linear-to-r from-primary-soft to-indigo-200 rounded-lg border border-primary-base"
        >
          <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="flex items-start sm:items-center gap-2 flex-1 min-w-0">
              <ObservationIcon size="lg" custom-class="text-blue-600" />
              <div class="flex-1 min-w-0">
                <p class="text-xs sm:text-sm text-gray-700 wrap-break-word">
                  <span>Showing Recommendations for Observation:</span>
                  <span class="ml-1 font-semibold text-primary-shade">
                    <span v-if="isObservationLoading">Loading Observation...</span>
                    <span v-else>{{ currentObservation?.highlight || "Loading..." }}</span>
                  </span>
                  <span v-if="searchQuery" class="ml-2 text-blue-600">
                    | Search: "{{ searchQuery }}"
                  </span>
                </p>
              </div>
            </div>

            <button
              @click="$emit('add-recommendation')"
              class="flex items-center justify-center gap-1 px-4 py-2 text-sm rounded-md bg-secondary-base hover:bg-secondary-shade text-white transition-all whitespace-nowrap"
            >
              <PlusIcon :stroke-width="3" class="w-5 h-5" />
              Add Recommendation
            </button>
          </div>
        </div>

        <!-- Desktop Table -->
        <div v-if="selectedObservationId" class="hidden lg:block rounded-lg border border-gray-200">
          <ReusableTable
            :items="enhancedRecommendations"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="recommendation-table min-w-full"
          >
            <template #cell-actions="{ item }">
              <div class="flex flex-col items-start justify-start xl:flex-row gap-2 min-w-max">
                <button
                  @click.stop="$emit('edit-recommendation', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-recommendation', item)"
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
          v-if="selectedObservationId"
          :items="enhancedRecommendations"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="lg:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  {{ enhancedColumns[0].label }}
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.title }}
                </div>
              </div>

              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  {{ enhancedColumns[1].label }}
                </div>
                <div class="text-gray-700 text-sm leading-snug wrap-break-word">
                  {{ item.description }}
                </div>
              </div>

              <div>
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  {{ enhancedColumns[2].label }}
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
                @click.stop="$emit('edit-recommendation', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" /> Edit
              </button>

              <button
                @click.stop="$emit('delete-recommendation', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" /> Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>

        <!-- No results while searching -->
        <div
          v-if="searchQuery && recommendations.length === 0 && !isLoading"
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

        <!-- No Observation Selected -->
        <div v-else-if="!selectedObservationId" class="text-center py-16">
          <ObservationIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />
          <h3 class="text-xl font-semibold text-secondary-shade mb-4">
            Please select an Observation to view Recommendations
          </h3>
          <p class="text-gray-500 mb-6">
            Choose an Observation from the dropdown above to get started
          </p>
          <div v-if="!observations || observations.length === 0">
            <p class="text-sm text-gray-400">Please add an Observation first to manage Recommendations</p>
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
import RecommendationHeader from "./RecommendationHeader.vue";
import ObservationSelector from "@/components/features/riskControl/observation/components/ObservationSelector.vue";
import ObservationIcon from "@/components/common/icons/ObservationIcon.vue";
import { formatDateSimple } from "@/utils/dateUtils";

const props = defineProps({
  observations: {
    type: Array,
    default: () => [],
  },
  recommendations: {
    type: Array,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  selectedObservationId: {
    type: [String, Number],
    default: null,
  },
  selectedObservation: {
    type: Object,
    default: () => null,
  },
  currentObservation: {
    type: Object,
    default: () => null,
  },
  isObservationLoading: {
    type: Boolean,
    default: false,
  },
  searchQuery: {
    type: String,
    default: "",
  },
  handleObservationChange: {
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

const internalSelectedObservationId = ref(props.selectedObservationId);

watch(
  () => props.selectedObservationId,
  (newVal) => {
    internalSelectedObservationId.value = newVal;
  }
);

watch(internalSelectedObservationId, (newVal) => {
  props.handleObservationChange(newVal);
});

defineEmits(["add-recommendation", "edit-recommendation", "delete-recommendation"]);

const recommendationColumns = [
  { key: "title", label: "Title" },
  { key: "description", label: "Description" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedRecommendations = computed(() => {
  return props.recommendations.map((recommendation) => {
    return {
      ...recommendation,
    };
  });
});

const enhancedColumns = computed(() => {
  return [...recommendationColumns, { key: "actions", label: "Actions" }];
});

const noDataMessage = computed(() => {
  if (props.searchQuery) {
    return `No Recommendations found for "${props.searchQuery}".`;
  }
  return "No Recommendations found for this Observation.";
});
</script>

<style scoped>
.recommendation-table .table-row {
  transition: all 0.2s ease;
  cursor: pointer;
}

.recommendation-table .table-row:hover {
  background-color: #f8fafc;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.recommendation-table .table-row.selected {
  background-color: #e6f7ff;
  border-left: 4px solid #3b82f6;
}
</style>