<template>
  <div>
    <!-- Sub Department Container -->
    <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
      <div class="p-3 sm:p-4 md:p-6">
        <!-- Header -->
        <div class="flex flex-col gap-4 mb-6">
          <SubDepartmentHeader />

          <div class="flex flex-col sm:flex-row sm:items-center gap-4">
            <div class="w-full sm:min-w-64 sm:max-w-xs">
              <DepartmentSelector
                :departments="departments"
                :selected-department-id="internalSelectedDepartmentId"
                @location-changed="handleDepartmentChange"
              />
            </div>
            <div class="w-full sm:w-auto">
              <SearchFilter
                :search="searchQuery"
                :searchPlaceholder="'Search departments...'"
                @update:search="handleSearch"
                class="w-full sm:w-auto"
              />
            </div>
          </div>
        </div>

        <!-- Selected Department Info -->
        <div
          v-if="internalSelectedDepartmentId"
          class="mb-6 p-3 sm:p-4 bg-linear-to-r from-primary-soft to-indigo-200 rounded-lg border border-primary-base"
        >
          <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
            <div class="flex items-start sm:items-center gap-2 flex-1 min-w-0">
              <DepartmentIcon size="lg" custom-class="text-blue-600" />
              <div class="flex-1 min-w-0">
                <p class="text-xs sm:text-sm text-gray-700 wrap-break-word">
                  <span>Showing sub departments for:</span>
                  <span class="ml-1 font-semibold text-blue-800">
                    <span v-if="isDepartmentLoading">Loading...</span>
                    <span v-else>{{ currentDepartment?.name }}</span>
                  </span>
                  <span v-if="searchQuery" class="ml-2 text-blue-600">
                    | Search: "{{ searchQuery }}"
                  </span>
                </p>
              </div>
            </div>

            <button
              @click="$emit('add-sub-department')"
              class="flex items-center justify-center gap-1 px-4 py-2 text-sm rounded-md bg-secondary-base hover:bg-secondary-shade text-white transition-all whitespace-nowrap"
            >
              <PlusIcon :stroke-width="3" class="w-5 h-5" />
              Add Sub Department
            </button>
          </div>
        </div>

        <!-- Desktop Table -->
        <div v-if="internalSelectedDepartmentId" class="hidden md:block rounded-lg border border-gray-200">
          <ReusableTable
            :items="subDepartments"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="sub-department-table min-w-full"
          >
            <template #cell-actions="{ item }">
              <div class="flex flex-col 2xl:flex-row gap-2 min-w-max">
                <button
                  @click.stop="handleSelectSubDepartment(item)"
                  class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                  :class="{ 'bg-green-200': selectedSubDepartmentId === item.id }"
                >
                  <CheckIcon v-if="selectedSubDepartmentId === item.id" size="sm" custom-class="mr-1" />
                  <SelectIcon v-else size="sm" custom-class="mr-1" />
                  {{ selectedSubDepartmentId === item.id ? "Selected" : "Select" }}
                </button>

                <button
                  @click.stop="$emit('edit-sub-department', item)"
                  class="inline-flex items-center px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-sub-department', item)"
                  class="inline-flex items-center px-2 py-1 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
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
          v-if="internalSelectedDepartmentId"
          :items="subDepartments"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="md:hidden"
        >
          <template #card-content="{ item}">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">Name</div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.name }}
                </div>
              </div>

              <div>
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">Created At</div>
                <div class="text-gray-400 text-xs font-medium">{{ formatDate(item.createdAt) }}</div>
              </div>
            </div>
          </template>

          <template #card-actions="{ item }">
            <div class="flex w-full justify-start gap-2 mt-2 flex-wrap">
              <button
                @click.stop="handleSelectSubDepartment(item)"
                class="w-42 min-h-10 inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 transition-all duration-200"
                :class="{ 'bg-green-200': selectedSubDepartmentId === item.id }"
              >
                <CheckIcon v-if="selectedSubDepartmentId === item.id" size="sm" class="mr-1" />
                <SelectIcon v-else size="sm" class="mr-1" />
                {{ selectedSubDepartmentId === item.id ? "Selected" : "Select" }}
              </button>

              <button
                @click.stop="$emit('edit-sub-department', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" /> Edit
              </button>

              <button
                @click.stop="$emit('delete-sub-department', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" /> Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>

        <!-- No results while searching -->
        <div
          v-if="searchQuery && departments.length === 0 && !isLoading"
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

        <!-- No Department Selected -->
        <div v-else-if="!internalSelectedDepartmentId" class="text-center py-16">
          <DepartmentIcon size="xxxl" custom-class="text-secondary-base mx-auto mb-6" />
          <h3 class="text-xl font-semibold text-secondary-shade mb-4">Please select a Department to view sub departments</h3>
          <p class="text-gray-500 mb-6">Choose a department from the dropdown above to get started</p>
          <div v-if="!departments || departments.length === 0">
            <p class="text-sm text-gray-400">Please add a department first to manage sub departments</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import ReusableTable from '@/components/common/ReusableTable.vue';
import ReusableMobileCardList from '@/components/common/ReusableMobileCardList.vue';
import SearchFilter from '@/components/common/SearchFilter.vue';
import DepartmentSelector from '@/components/features/Organization/department/components/DepartmentSelector.vue';
import SubDepartmentHeader from './SubDepartmentHeader.vue';
import DepartmentIcon from '@/components/common/icons/DepartmentIcon.vue';
import PlusIcon from '@/components/common/icons/PlusIcon.vue';
import EditIcon from '@/components/common/icons/EditIcon.vue';
import DeleteIcon from '@/components/common/icons/DeleteIcon.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import CheckIcon from '@/components/common/icons/CheckIcon.vue';
import SelectIcon from '@/components/common/icons/SelectIcon.vue';

// Initialize router at the top level
const router = useRouter();

const props = defineProps({
    departments: Array,
    subDepartments: Array,
    isLoading: Boolean,
    selectedDepartmentId: String,
    selectedDepartment: Object,
    currentDepartment: Object,
    isDepartmentLoading: Boolean,
    searchQuery: String,
    handleDepartmentChange: Function,
    handleSearch: Function
});

const emit = defineEmits([
    'add-sub-department',
    'edit-sub-department',
    'delete-sub-department',
    'sub-department-selected'
]);

const internalSelectedDepartmentId = ref(props.selectedDepartmentId);

watch(() => props.selectedDepartmentId, (newVal) => {
    internalSelectedDepartmentId.value = newVal;
});

watch(() => internalSelectedDepartmentId.value, (newVal) => {
    props.handleDepartmentChange(newVal);
}, { immediate: true });

// SubDepartment selection state
const selectedSubDepartmentId = ref(null);

// Handle subdepartment selection from table row or button
const handleSelectSubDepartment = (subDepartment) => {
    selectedSubDepartmentId.value = subDepartment.id;
    emit('sub-department-selected', subDepartment.id);

    // Navigate to RiskControl with selected subdepartment
    router.push({
        path: '/risk-control',
    });
};


const subDepartmentColumns = [
    { key: 'name', label: 'Name' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...subDepartmentColumns, { key: 'actions', label: 'Actions' }];
});

const noDataMessage = computed(() => {
    if (props.searchQuery) {
        return `No sub departments found for "${props.searchQuery}".`;
    }
    return 'No sub departments found for this department.';
});


const formatDate = (dateStr) => {
  if (!dateStr) return "—";
  const date = new Date(dateStr);
  return isNaN(date.getTime()) ? "Invalid Date" : date.toLocaleDateString();
};

</script>

<style scoped>
.sub-department-table .table-row {
    transition: all 0.2s ease;
    cursor: pointer;
}

.sub-department-table .table-row:hover {
    background-color: #f8fafc;
    transform: translateY(-1px);
    box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.sub-department-table .table-row.selected {
    background-color: #eff6ff;
    border-left: 4px solid #3b82f6;
}
</style>
