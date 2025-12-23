<template>
  <div>
    <!-- Observation Container -->
    <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
      <div class="p-3 sm:p-4 md:p-6">
        <!-- Header -->
        <div class="flex flex-col gap-4 mb-6">
          <SubProcessHeader />

          <div class="flex flex-col sm:flex-row sm:items-center gap-4">
            <div class="w-full sm:min-w-64 sm:max-w-xs">
              <AuditProgramSelector
                :auditPrograms="auditPrograms"
                :selectedAuditProgramId="internalSelectedAuditProgramId"
                @update:selectedAuditProgram="(id) => (internalSelectedAuditProgramId = id)"
                @audit-program-changed="(id) => (internalSelectedAuditProgramId = id)"
              />
            </div>
            <div class="w-full sm:w-auto">
              <SearchFilter
                :search="searchQuery"
                :search-placeholder="'Search observations...'"
                @update:search="handleSearch"
                class="w-full sm:w-auto"
              />
            </div>
          </div>
        </div>

        <!-- Selected Audit Program Info -->
        <div
          v-if="selectedAuditProgramId"
          class="mb-6 p-3 sm:p-4 bg-linear-to-r from-primary-soft to-indigo-200 rounded-lg border border-primary-base"
        >
          <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="flex items-start sm:items-center gap-2 flex-1 min-w-0">
              <AuditProgramIcon size="lg" custom-class="text-blue-600" />
              <div class="flex-1 min-w-0">
                <p class="text-xs sm:text-sm text-gray-700 wrap-break-word">
                  <span>Showing Observations for:</span>
                  <span class="ml-1 font-semibold text-primary-shade">
                    <span v-if="isAuditProgramLoading">Loading audit program...</span>
                    <span v-else>{{ currentAuditProgram?.description || "Loading..." }}</span>
                  </span>
                  <span v-if="searchQuery" class="ml-2 text-blue-600">
                    | Search: "{{ searchQuery }}"
                  </span>
                </p>
              </div>
            </div>

            <button
              @click="$emit('add-observation')"
              class="flex items-center justify-center gap-1 px-4 py-2 text-sm rounded-md bg-secondary-base hover:bg-secondary-shade text-white transition-all whitespace-nowrap"
            >
              <PlusIcon :stroke-width="3" class="w-5 h-5" />
              Add Observation
            </button>
          </div>
        </div>

        <!-- Desktop Table -->
        <div
          v-if="selectedAuditProgramId"
          class="hidden lg:block rounded-lg border border-gray-200"
        >
          <ReusableTable
            :items="enrichedObservations"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="observation-table min-w-full"
          >
            <!-- Custom cell for IsActive -->
            <template #cell-isActive="{ item }">
              <span :class="item.isActive ? 'text-green-700 font-semibold' : 'text-red-700 font-semibold'">
                {{ item.isActive ? 'Yes' : 'No' }}
              </span>
            </template>

            <!-- Custom cell for Rate -->
            <template #cell-rate="{ item }">
              <span v-if="item.rate && item.rate.level" :class="getRateColorClasses(item.rate.level)">
                {{ item.rate.level }}
              </span>
              <span v-else class="text-gray-400 italic">Not assigned</span>
            </template>

            <template #cell-actions="{ item }">
              <div class="flex flex-col items-start justify-start xl:flex-row gap-2 min-w-max">
                <button
                  @click.stop="handleSelectObservation(item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                  :class="{ 'bg-green-200': selectedObservationId === item.id }"
                >
                  <CheckIcon
                    v-if="selectedObservationId === item.id"
                    size="md"
                    custom-class="mr-1"
                  />
                  <SelectIcon v-else size="md" custom-class="mr-1" />
                  {{ selectedObservationId === item.id ? "Selected" : "Select" }}
                </button>

                <button
                  @click.stop="$emit('edit-observation', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-observation', item)"
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
          v-if="selectedAuditProgramId"
          :items="enrichedObservations"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="lg:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Highlight
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.highlight }}
                </div>
              </div>

              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Background
                </div>
                <div class="text-gray-700 text-sm leading-snug wrap-break-word">
                  {{ item.background }}
                </div>
              </div>

              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Is Active
                </div>
                <div class="text-sm leading-snug wrap-break-word">
                  <span :class="item.isActive ? 'text-green-700 font-semibold' : 'text-red-700 font-semibold'">
                    {{ item.isActive ? 'Yes' : 'No' }}
                  </span>
                </div>
              </div>

              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Rate
                </div>
                <div class="text-sm leading-snug wrap-break-word">
                  <span v-if="item.rate && item.rate.level" :class="getRateColorClasses(item.rate.level)">
                    {{ item.rate.level }}
                  </span>
                  <span v-else class="text-gray-400 italic">Not assigned</span>
                </div>
              </div>

              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Note
                </div>
                <div class="text-gray-700 text-sm leading-snug wrap-break-word">
                  {{ item.note || 'N/A' }}
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
                @click.stop="handleSelectObservation(item)"
                class="w-42 min-h-10 inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                :class="{ 'bg-green-200': selectedObservationId === item.id }"
              >
                <CheckIcon v-if="selectedObservationId === item.id" size="md" class="mr-1" />
                <SelectIcon v-else size="md" class="mr-1" />
                {{ selectedObservationId === item.id ? "Selected" : "Select" }}
              </button>

              <button
                @click.stop="$emit('edit-observation', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" /> Edit
              </button>

              <button
                @click.stop="$emit('delete-observation', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" /> Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>

        <!-- No results while searching -->
        <div
          v-if="searchQuery && enrichedObservations.length === 0 && !isLoading"
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

        <!-- No Audit Program Selected -->
        <NoAuditProgramSelected
          v-if="!isAuditProgramLoading && auditPrograms.length === 0"
          :audit-programs="auditPrograms"
        />
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, watch, onMounted } from "vue";
import ReusableTable from "@/components/common/ReusableTable.vue";
import ReusableMobileCardList from "@/components/common/ReusableMobileCardList.vue";
import SearchFilter from "@/components/common/SearchFilter.vue";
import AuditProgramIcon from "@/components/common/icons/AuditProgramIcon.vue";
import AuditProgramSelector from "@/components/features/riskControl/auditProgram/components/AuditProgramSelector.vue";
import PlusIcon from "@/components/common/icons/PlusIcon.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import CheckIcon from "@/components/common/icons/CheckIcon.vue";
import SelectIcon from "@/components/common/icons/SelectIcon.vue";
import SubProcessHeader from "./ObservationHeader.vue";
import NoAuditProgramSelected from "./NoAuditProgramSelected.vue";
import { formatDateSimple } from "@/utils/dateUtils";
import { useSearchRates } from "@/composables/common/rate/useSearchRates";

const props = defineProps({
  auditPrograms: {
    type: Array,
    default: () => [],
  },
  observations: {
    type: Array,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  selectedAuditProgramId: {
    type: String,
    default: "",
  },
  selectedAuditProgram: {
    type: Object,
    default: () => null,
  },
  currentAuditProgram: {
    type: Object,
    default: () => null,
  },
  isAuditProgramLoading: {
    type: Boolean,
    default: false,
  },
  searchQuery: {
    type: String,
    default: "",
  },
  handleAuditProgramChange: {
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

const internalSelectedAuditProgramId = ref(props.selectedAuditProgramId);
const { searchResults: rates, searchRates } = useSearchRates();

// Load rates on mount
onMounted(async () => {
  try {
    await searchRates({ searchTerm: '', pageNumber: 1, pageSize: 100 });
  } catch (err) {
    console.error('Error loading rates:', err);
  }
});

// Enrich observations with rate data
const enrichedObservations = computed(() => {
  if (!rates.value || rates.value.length === 0) {
    return props.observations;
  }

  return props.observations.map(observation => {
    if (observation.rate) {
      // Already has rate data
      return observation;
    }

    // Find matching rate by rateId
    const matchingRate = rates.value.find(rate => rate.id === observation.rateId);
    
    return {
      ...observation,
      rate: matchingRate ? { id: matchingRate.id, level: matchingRate.level } : null
    };
  });
});

watch(
  () => props.selectedAuditProgramId,
  (newVal) => {
    internalSelectedAuditProgramId.value = newVal;
  }
);

watch(internalSelectedAuditProgramId, (newVal) => {
  props.handleAuditProgramChange(newVal);
});

const emit = defineEmits([
  "add-observation",
  "edit-observation",
  "delete-observation",
  "observation-selected",
]);

// Observation selection state
const selectedObservationId = ref(null);

// Handle observation selection from table row or button
const handleSelectObservation = (observation) => {
  selectedObservationId.value = observation.id;
  emit("observation-selected", observation.id);
};

const observationColumns = [
  { key: "highlight", label: "Highlight" },
  { key: "background", label: "Background" },
  { key: "isActive", label: "Is Active" },
  { key: "rate", label: "Rate" },
  { key: "note", label: "Note" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedColumns = computed(() => {
  return [...observationColumns, { key: "actions", label: "Actions" }];
});

const noDataMessage = computed(() => {
  if (props.searchQuery) {
    return `No observations found for "${props.searchQuery}".`;
  }
  return "No observations found for this audit program.";
});

// Rate color mapping based on level
const getRateColorClasses = (level) => {
  if (!level) {
    return 'px-2 py-1 rounded-md font-semibold text-xs bg-gray-100 text-gray-800 border border-gray-200';
  }
  const levelStr = String(level).toLowerCase();
  const colorMap = {
    critical: 'px-2 py-1 rounded-md font-semibold text-xs bg-red-100 text-red-800 border border-red-200',
    high: 'px-2 py-1 rounded-md font-semibold text-xs bg-orange-100 text-orange-800 border border-orange-200',
    medium: 'px-2 py-1 rounded-md font-semibold text-xs bg-yellow-100 text-yellow-800 border border-yellow-200',
    low: 'px-2 py-1 rounded-md font-semibold text-xs bg-blue-100 text-blue-800 border border-blue-200',
    minimal: 'px-2 py-1 rounded-md font-semibold text-xs bg-green-100 text-green-800 border border-green-200'
  };
  return colorMap[levelStr] || 'px-2 py-1 rounded-md font-semibold text-xs bg-gray-100 text-gray-800 border border-gray-200';
};
</script>
