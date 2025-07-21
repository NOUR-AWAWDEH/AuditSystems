<template>
  <div class="min-h-[calc(100vh-4rem)] bg-gray-50 p-2 sm:p-4 md:p-6">
    <!-- Header Section -->
    <div class="mb-4 sm:mb-6 md:mb-8">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between">
        <RiskControlHeader 
          :selected-sub-department="selectedSubDepartment"
          :is-loading-sub-department="isLoadingSubDepartment"
        />
      </div>
    </div>

    <!-- Navigation Tabs -->
    <div class="mb-4 sm:mb-6">
      <div class="border-b border-gray-200">
        <nav class="-mb-px flex space-x-2 sm:space-x-4 md:space-x-8 overflow-x-auto scrollbar-hide">
          <button
            v-for="tab in tabs"
            :key="tab.id"
            @click="activeTab = tab.id"
            :class="[
              'whitespace-nowrap py-2 px-2 sm:px-3 md:px-1 border-b-2 font-medium text-xs sm:text-sm transition-colors duration-200 flex-shrink-0',
              activeTab === tab.id
                ? 'border-primary-base text-primary-base'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            ]"
          >
            {{ tab.name }}
          </button>
        </nav>
      </div>
    </div>

    <!-- Tab Content -->
    <div class="bg-white rounded-lg sm:rounded-xl shadow-sm border border-gray-200">
      <!-- Process Management -->
      <div v-if="activeTab === 'processes'" class="p-3 sm:p-4 md:p-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 sm:gap-0 mb-4 sm:mb-6">
          <h2 class="text-lg sm:text-xl font-semibold text-secondary-base">Process Management</h2>
          <button class="bg-primary-base text-white px-3 sm:px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200 text-sm sm:text-base w-full sm:w-auto">
            Add Process
          </button>
        </div>
      </div>

      <!-- Sub Process Management -->
      <div v-if="activeTab === 'subprocesses'" class="p-3 sm:p-4 md:p-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 sm:gap-0 mb-4 sm:mb-6">
          <h2 class="text-lg sm:text-xl font-semibold text-secondary-base">Sub Process Management</h2>
          <button class="bg-primary-base text-white px-3 sm:px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200 text-sm sm:text-base w-full sm:w-auto">
            Add Sub Process
          </button>
        </div>
      </div>

      <!-- Risk Control Matrix -->
      <div v-if="activeTab === 'matrix'" class="p-3 sm:p-4 md:p-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 sm:gap-0 mb-4 sm:mb-6">
          <h2 class="text-lg sm:text-xl font-semibold text-secondary-base">Risk Control Matrix</h2>
          <button class="bg-primary-base text-white px-3 sm:px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200 text-sm sm:text-base w-full sm:w-auto">
            Add Matrix Entry
          </button>
        </div>
      </div>

      <!-- Objectives -->
      <div v-if="activeTab === 'objectives'" class="p-3 sm:p-4 md:p-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 sm:gap-0 mb-4 sm:mb-6">
          <h2 class="text-lg sm:text-xl font-semibold text-secondary-base">Objectives Management</h2>
          <button class="bg-primary-base text-white px-3 sm:px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200 text-sm sm:text-base w-full sm:w-auto">
            Add Objective
          </button>
        </div>
      </div>

      <!-- Risks -->
      <div v-if="activeTab === 'risks'" class="p-3 sm:p-4 md:p-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 sm:gap-0 mb-4 sm:mb-6">
          <h2 class="text-lg sm:text-xl font-semibold text-secondary-base">Risk Management</h2>
          <button class="bg-primary-base text-white px-3 sm:px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200 text-sm sm:text-base w-full sm:w-auto">
            Add Risk
          </button>
        </div>
      </div>

      <!-- Exit Risks -->
      <div v-if="activeTab === 'exitrisks'" class="p-3 sm:p-4 md:p-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 sm:gap-0 mb-4 sm:mb-6">
          <h2 class="text-lg sm:text-xl font-semibold text-secondary-base">Exit Risk Management</h2>
          <button class="bg-primary-base text-white px-3 sm:px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200 text-sm sm:text-base w-full sm:w-auto">
            Add Exit Risk
          </button>
        </div>
      </div>

      <!-- Control Design -->
      <div v-if="activeTab === 'controls'" class="p-3 sm:p-4 md:p-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 sm:gap-0 mb-4 sm:mb-6">
          <h2 class="text-lg sm:text-xl font-semibold text-secondary-base">Control Design</h2>
          <button class="bg-primary-base text-white px-3 sm:px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200 text-sm sm:text-base w-full sm:w-auto">
            Add Control
          </button>
        </div>
      </div>

      <!-- Observations -->
      <div v-if="activeTab === 'observations'" class="p-3 sm:p-4 md:p-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 sm:gap-0 mb-4 sm:mb-6">
          <h2 class="text-lg sm:text-xl font-semibold text-secondary-base">Observations</h2>
          <button class="bg-primary-base text-white px-3 sm:px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200 text-sm sm:text-base w-full sm:w-auto">
            Add Observation
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useGetSubDepartmentById } from '@/composables/organization/subdepartment/useGetSubDepartmentById';
import RiskControlHeader from './RiskControlHeader.vue';

const route = useRoute();

// SubDepartment management
const { 
  subdepartment: selectedSubDepartment, 
  isLoading: isLoadingSubDepartment, 
  fetchSubDepartmentById 
} = useGetSubDepartmentById();

// Active tab state
const activeTab = ref('processes');

// Watch for route query changes to load subdepartment
watch(
  () => route.query.subDepartmentId,
  async (newSubDepartmentId) => {
    if (newSubDepartmentId) {
      await fetchSubDepartmentById(newSubDepartmentId);
    }
  },
  { immediate: true }
);

// Load subdepartment on component mount if ID is in query
onMounted(async () => {
  const subDepartmentId = route.query.subDepartmentId;
  if (subDepartmentId) {
    await fetchSubDepartmentById(subDepartmentId);
  }
});

// Tab configuration
const tabs = ref([
  { id: 'processes', name: 'Processes' },
  { id: 'subprocesses', name: 'Sub Processes' },
  { id: 'matrix', name: 'Risk Control Matrix' },
  { id: 'objectives', name: 'Objectives' },
  { id: 'risks', name: 'Risks' },
  { id: 'exitrisks', name: 'Exit Risks' },
  { id: 'controls', name: 'Control Design' },
  { id: 'observations', name: 'Observations' }
]);

</script>

<style scoped>
.min-h-\[calc\(100vh-4rem\)\] {
  min-height: calc(100vh - 4rem);
  min-height: calc(100dvh - 4rem);
}

/* Hide scrollbar for tab navigation */
.scrollbar-hide {
  -ms-overflow-style: none;
  scrollbar-width: none;
}

.scrollbar-hide::-webkit-scrollbar {
  display: none;
}

/* Enhanced mobile responsiveness */
@media (max-width: 640px) {
  .p-2 {
    padding: 0.5rem;
  }
  
  .mb-4 {
    margin-bottom: 1rem;
  }
  
  .gap-3 {
    gap: 0.75rem;
  }
}

@media (max-width: 480px) {
  .p-2 {
    padding: 0.375rem;
  }
  
  .mb-4 {
    margin-bottom: 0.75rem;
  }
  
  .gap-3 {
    gap: 0.5rem;
  }
  
  /* Ensure tabs don't get too cramped */
  .space-x-2 > :not([hidden]) ~ :not([hidden]) {
    margin-left: 0.25rem;
  }
}

@media (max-width: 360px) {
  .p-2 {
    padding: 0.25rem;
  }
  
  /* Very small screens - stack everything */
  .flex-shrink-0 {
    min-width: fit-content;
  }
}
</style>