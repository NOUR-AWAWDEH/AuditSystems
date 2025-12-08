<template>
  <!--JobTitleTable.vue-->
  <div>
    <!-- Job Title Container -->
    <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
      <div class="p-3 sm:p-4 md:p-6">
        <!-- Header with title and Filters -->
        <div class="flex flex-col gap-4 mb-6">
          <JobTitleHeader />

          <div class="flex flex-col sm:flex-row sm:items-end gap-3">
              <SearchFilter
                :search="searchQuery"
                :searchPlaceholder="'Search job titles...'"
                @update:search="handleSearch"
                class="w-full sm:w-auto md:w-1/2"
              />
          </div>
        </div>

        <div class="hidden lg:block rounded-lg border border-gray-200">
          <ReusableTable
            :items="jobTitles"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="job-title-table min-w-full"
          >
            <template #cell-actions="{ item }">
              <div class="flex flex-col items-start justify-start xl:flex-row gap-2 min-w-max">

                <button
                  @click.stop="$emit('edit-job-title', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-job-title', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <DeleteIcon size="sm" custom-class="mr-1" />
                  <span>Delete</span>
                </button>
              </div>
            </template>
          </ReusableTable>
        </div>

        <!-- Mobile Card View -->
        <ReusableMobileCardList
          :items="jobTitles"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="lg:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  {{ jobTitleColumns[0].label }}
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.name }}
                </div>
              </div>

              <div>
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  {{ jobTitleColumns[1].label }}
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
                @click.stop="$emit('edit-job-title', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" /> Edit
              </button>

              <button
                @click.stop="$emit('delete-job-title', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" />
                Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>
      </div>

        <!-- Loading -->
        <div v-if="isLoading" class="text-center py-8">
          <div
            class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-primary-base">
          </div>
        <p class="mt-2 text-gray-600 text-sm">Loading data...</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed} from "vue";
import JobTitleHeader from "./JobTitleHeader.vue"
import ReusableTable from "@/components/common/ReusableTable.vue";
import ReusableMobileCardList from "@/components/common/ReusableMobileCardList.vue";
import SearchFilter from "@/components/common/SearchFilter.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";
import { formatDateSimple } from "@/utils/dateUtils";

const props = defineProps({
  jobTitles: {
    type: Array,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  searchQuery: {
    type: String,
    default: "",
  },
  handleSearch: {
    type: Function,
    default: () => {},
  },
});

defineEmits(["add-job-title", "edit-job-title", "delete-job-title"]);

const jobTitleColumns = [
  { key: "name", label: "Job Title Name" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedColumns = computed(() => {
  return [...jobTitleColumns, { key: "actions", label: "Actions" }];
});

const noDataMessage = computed(() =>
 props.searchQuery
    ? `No Locations found for "${props.searchQuery}".`
    : "No Locations found yet. Please add a location."
);

</script>

<style scoped>
.job-title-table .table-row {
  transition: all 0.2s ease;
  cursor: pointer;
}

.job-title-table .table-row:hover {
  background-color: #f8fafc;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.job-title-table .table-row.selected {
  background-color: #eff6ff;
  border-left: 4px solid #3b82f6;
}

@media (max-width: 767px) {
  .job-title-table .table-row:hover {
    transform: none;
    box-shadow: none;
  }
}
</style>
