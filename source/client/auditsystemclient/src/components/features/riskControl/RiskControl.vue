<template>
  <div class="min-h-[calc(100vh-4rem)] bg-gray-50 p-2 sm:p-4 md:p-6">
    <!-- Header Section -->
    <div class="mb-4 sm:mb-6 md:mb-8">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between">
        <RiskControlHeader />
      </div>

    </div>

    <!-- Navigation Tabs -->
    <div class="mb-4 sm:mb-6">
      <div class="border-b border-gray-200">
        <nav class="-mb-px flex space-x-2 sm:space-x-4 md:space-x-8 overflow-x-auto scrollbar-hide">
          <button
            v-for="tab in tabs"
            :key="tab.id"
            @click="setActiveTab(tab.id)"
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
      <div v-if="isLoading" class="p-3 sm:p-4 md:p-6 text-center">
        <p class="text-gray-500">Loading...</p>
      </div>
      <div v-else-if="errorMessage" class="p-3 sm:p-4 md:p-6 text-center">
        <p class="text-red-500">{{ errorMessage }}</p>
      </div>
      <div v-else>
        <!-- Process Management -->
        <div v-if="activeTab === 'processes'" class="p-3 sm:p-4 md:p-6">
          <AuditProcess
            :sub-departments="subDepartments"
            :selected-sub-department-id="selectedSubDepartmentId"
            @audit-process-selected="handleAuditProcessSelected"
          />
        </div>

        
        <!-- SubProcess component -->
        <div v-if="activeTab === 'subprocesses'" class="p-3 sm:p-4 md:p-6">
          <SubProcess
            :audit-processes="auditProcesses"
            :selected-audit-process-id="selectedAuditProcessId"
            @sub-process-selected="handleSubProcessSelected"
          />
        </div>

        <!-- Risk Control Matrix -->
        <div v-if="activeTab === 'matrix'" class="p-3 sm:p-4 md:p-6">
          <RiskControlMatrix
            :sub-processes="subProcesses"
            :selected-sub-process-id="selectedSubProcessId"
            @risk-control-matrix-selected="handleRiskControlMatrixSelected"
          />
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
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useGetSubDepartmentById } from '@/composables/organization/subdepartment/useGetSubDepartmentById';
import { useGetSubDepartmentsByUserId } from '@/composables/organization/subdepartment/useGetSubDepartmentsByUserId';
import { useGetAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId';
import { useGetSubProcessesByUserId } from '@/composables/riskControl/subProcess/useGetSubProcessesByUserId';
import RiskControlHeader from './components/RiskControlHeader.vue';
import AuditProcess from './auditProcess/AuditProcess.vue';
import SubProcess from './subProcess/SubProcess.vue';
import RiskControlMatrix from './riskControlMatrix/RiskControlMatrix.vue';

// State management
const route = useRoute();
const router = useRouter();
const activeTab = ref('processes');
const selectedSubDepartmentId = ref(localStorage.getItem('riskControlSelectedSubDepartmentId') || null);
const selectedAuditProcessId = ref(localStorage.getItem('riskControlSelectedAuditProcessId') || null);
const selectedSubProcessId = ref(localStorage.getItem('riskControlSelectedSubProcessId') || null);

// Composable Destructuring
const { 
  getSubDepartmentsByUserId, 
  subDepartments, 
  isLoading: isSubDeptLoading, 
  error: subDeptError 
} = useGetSubDepartmentsByUserId();

const { 
  getAuditProcessesByUserId, 
  auditProcesses, 
  isLoading: isAuditProcLoading, 
  error: auditProcError 
} = useGetAuditProcessesByUserId();

const { 
  getSubProcessesByUserId, 
  subProcesses, 
  isLoading: isSubProcLoading, 
  error: subProcError 
} = useGetSubProcessesByUserId();

const { getSubDepartmentById } = useGetSubDepartmentById();

// Reactive loading and error states for the main component
const isLoading = ref(false);
const errorMessage = ref(null);

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

// Data fetching function
const fetchData = async () => {
  isLoading.value = true;
  errorMessage.value = null;

  try {
    const [subDeptResult, auditResult, subProcResult] = await Promise.all([
      getSubDepartmentsByUserId(),
      getAuditProcessesByUserId(),
      getSubProcessesByUserId()
    ]);

    if (!subDeptResult?.success) {
      throw new Error('Failed to fetch subdepartments');
    }

    if (!auditResult?.success || auditResult.data?.length === 0) {
      throw new Error('No audit processes found in database');
    }

    if (!subProcResult?.success) {
      throw new Error('Failed to fetch sub processes');
    }
  } catch (error) {
    errorMessage.value = error.message || 'An error occurred while fetching data';
    console.error('Data fetch error:', error);
  } finally {
    isLoading.value = false;
  }
};

// Handle selections
const selectAuditProcess = (auditProcessId) => {
  selectedAuditProcessId.value = auditProcessId;
  localStorage.setItem('riskControlSelectedAuditProcessId', auditProcessId || '');
};

const handleAuditProcessSelected = (auditProcessId) => {
  selectAuditProcess(auditProcessId);
  setActiveTab('subprocesses');
};

const handleSubProcessSelected = (subProcessId) => {
  selectedSubProcessId.value = subProcessId;
  localStorage.setItem('riskControlSelectedSubProcessId', subProcessId || '');
  setActiveTab('matrix');
};

const handleRiskControlMatrixSelected = (riskControlMatrixId) => {
  console.log('Risk control matrix selected:', riskControlMatrixId);
};

// Tab navigation
const setActiveTab = (tabId) => {
  activeTab.value = tabId;
};

// Persist selections to localStorage
watch(selectedSubDepartmentId, (newValue) => {
  localStorage.setItem('riskControlSelectedSubDepartmentId', newValue || '');
});

watch(selectedAuditProcessId, (newValue) => {
  localStorage.setItem('riskControlSelectedAuditProcessId', newValue || '');
});

watch(selectedSubProcessId, (newValue) => {
  localStorage.setItem('riskControlSelectedSubProcessId', newValue || '');
});

// Handle route query parameters
const handleRouteQueries = async () => {
  const { subDepartmentId, auditProcessId, subProcessId } = route.query;
  
  if (subDepartmentId) {
    selectedSubDepartmentId.value = subDepartmentId;
    await getSubDepartmentById(subDepartmentId);
  }
  
  if (auditProcessId) {
    selectAuditProcess(auditProcessId);
  }
  
  if (subProcessId) {
    selectedSubProcessId.value = subProcessId;
  }
  
  // Clean URL after processing
  if (subDepartmentId || auditProcessId || subProcessId) {
    await router.replace({ path: '/risk-control' });
  }
};

// Initialize
onMounted(async () => {
  await fetchData();
  await handleRouteQueries();
});

// Watch for route changes
watch(() => route.query, handleRouteQueries, { immediate: true });

</script>

<style scoped>
.min-h-\[calc\(100vh-4rem\)\] {
  min-height: calc(100vh - 4rem);
  min-height: calc(100dvh - 4rem);
}

.scrollbar-hide {
  -ms-overflow-style: none;
  scrollbar-width: none;
}

.scrollbar-hide::-webkit-scrollbar {
  display: none;
}

@media (max-width: 640px) {
  .p-2 { padding: 0.5rem; }
  .mb-4 { margin-bottom: 1rem; }
  .gap-3 { gap: 0.75rem; }
}

@media (max-width: 480px) {
  .p-2 { padding: 0.375rem; }
  .mb-4 { margin-bottom: 0.75rem; }
  .gap-3 { gap: 0.5rem; }
  .space-x-2 > :not([hidden]) ~ :not([hidden]) {
    margin-left: 0.25rem;
  }
}

@media (max-width: 360px) {
  .p-2 { padding: 0.25rem; }
  .flex-shrink-0 { min-width: fit-content; }
}
</style>