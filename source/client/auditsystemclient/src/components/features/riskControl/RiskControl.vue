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
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useGetSubDepartmentById } from '@/composables/organization/subdepartment/useGetSubDepartmentById';
import RiskControlHeader from './components/RiskControlHeader.vue';
import AuditProcess from './auditProcess/AuditProcess.vue';
import SubProcess from './subProcess/SubProcess.vue';
import RiskControlMatrix from './riskControlMatrix/RiskControlMatrix.vue';
import { useGetSubDepartmentsByUserId } from '@/composables/organization/subdepartment/useGetSubDepartmentsByUserId';
import { useGetAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId';
import { useGetSubProcessesByUserId } from '@/composables/riskControl/subProcess/useGetSubProcessesByUserId';

const route = useRoute();
const router = useRouter();

// Composable For audit processes
const { 
  getAuditProcessesByUserId
} = useGetAuditProcessesByUserId();

// Composable For sub processes
const { 
  getSubProcessesByUserId
} = useGetSubProcessesByUserId();

// Composable For sub department
const { 
  getSubDepartmentById 
} = useGetSubDepartmentById();

const { 
  subdepartments: allSubDepartments,
  getSubDepartmentsByUserId 
} = useGetSubDepartmentsByUserId();


// Active tab state
const activeTab = ref('processes');

// Props for AuditProcess component
const subDepartments = ref([]);

// Props for SubProcess component
const auditProcesses = ref([]);

// Props for RiskControlMatrix component
const subProcesses = ref([]);

// Initialize from localStorage
const selectedSubDepartmentId = ref(localStorage.getItem('riskControlSelectedSubDepartmentId') || null);

// Props for SubProcess component - use the reactive ref directly from composable
// Initialize from localStorage
const selectedAuditProcessId = ref(localStorage.getItem('riskControlSelectedAuditProcessId') || null);

// Props for RiskControlMatrix component
// Initialize from localStorage
const selectedSubProcessId = ref(localStorage.getItem('riskControlSelectedSubProcessId') || null);

// Watch for selectedSubDepartmentId changes and persist to localStorage
watch(selectedSubDepartmentId, (newValue) => {
  if (newValue) {
    localStorage.setItem('riskControlSelectedSubDepartmentId', newValue);
  } else {
    localStorage.removeItem('riskControlSelectedSubDepartmentId');
  }
});

// Watch for selectedAuditProcessId changes and persist to localStorage
watch(selectedAuditProcessId, (newValue) => {
  if (newValue) {
    localStorage.setItem('riskControlSelectedAuditProcessId', newValue);
  } else {
    localStorage.removeItem('riskControlSelectedAuditProcessId');
  }
});

// Watch for selectedSubProcessId changes and persist to localStorage
watch(selectedSubProcessId, (newValue) => {
  if (newValue) {
    localStorage.setItem('riskControlSelectedSubProcessId', newValue);
  } else {
    localStorage.removeItem('riskControlSelectedSubProcessId');
  }
});

// Function to fetch all subdepartments for the user
const fetchAllSubDepartments = async () => {
  try {
    const result = await getSubDepartmentsByUserId();
    if (result?.success) {
      subDepartments.value = allSubDepartments.value; // Now contains ALL subdepartments
    }
  } catch (error) {
    console.error('Error fetching subdepartments:', error);
  }
};

// Function to fetch all audit processes for the user
const fetchAllAuditProcesses = async () => {
  try {
    const result = await getAuditProcessesByUserId();
    if (result?.success) {
      auditProcesses.value = result.auditProcesses;
    }
  } catch (error) {
    console.error('Error fetching audit processes:', error);
  }
};

// Function to fetch all sub processes for the user
const fetchAllSubProcesses = async () => {
  try {
    const result = await getSubProcessesByUserId();
    if (result?.success) {
      subProcesses.value = result.subProcesses;
    }

  } catch (error) {
    console.error('Error fetching sub processes:', error);
  }
};

// Watch for route query changes to load subdepartment
watch(
  () => route.query.subDepartmentId,
  async (newSubDepartmentId) => {
    if (newSubDepartmentId) {
      selectedSubDepartmentId.value = newSubDepartmentId;
      await fetchSubDepartmentById(newSubDepartmentId);
      
      // Fetch all subdepartments instead of just the selected one
      await fetchAllSubDepartments();
      
      // Redirect to clean URL after processing the subDepartmentId
      router.replace({ path: '/risk-control' });
    }
  },
  { immediate: true }
);

// Watch for route query changes to load audit process
watch(
  () => route.query.auditProcessId,
  async (newAuditProcessId) => {
    if (newAuditProcessId) {
      selectedAuditProcessId.value = newAuditProcessId;
      
      // Fetch all audit processes
      await fetchAllAuditProcesses();
      
      // Redirect to clean URL after processing the auditProcessId
      router.replace({ path: '/risk-control' });
    }
  },
  { immediate: true }
);

// Watch for route query changes to load sub process
watch(
  () => route.query.subProcessId,
  async (newSubProcessId) => {
    if (newSubProcessId) {
      selectedSubProcessId.value = newSubProcessId;
      
      // Fetch all sub processes
      await fetchAllSubProcesses();
      
      // Redirect to clean URL after processing the subProcessId
      router.replace({ path: '/risk-control' });
    }
  },
  { immediate: true }
);

// Load subdepartment on component mount
onMounted(async () => {
  // Always fetch all subdepartments first
  await fetchAllSubDepartments();
  
  // Always fetch all audit processes
  await fetchAllAuditProcesses();
  
  // Always fetch all sub processes
  await fetchAllSubProcesses();
  
  const subDepartmentId = route.query.subDepartmentId || selectedSubDepartmentId.value;
  if (subDepartmentId) {
    selectedSubDepartmentId.value = subDepartmentId;
    await getSubDepartmentById(subDepartmentId);
    
    // Only redirect if we came from a query parameter
    if (route.query.subDepartmentId) {
      router.replace({ path: '/risk-control' });
    }
  }
  
  const auditProcessId = route.query.auditProcessId || selectedAuditProcessId.value;
  if (auditProcessId) {
    selectedAuditProcessId.value = auditProcessId;
    
    // Only redirect if we came from a query parameter
    if (route.query.auditProcessId) {
      router.replace({ path: '/risk-control' });
    }
  }
  
  const subProcessId = route.query.subProcessId || selectedSubProcessId.value;
  if (subProcessId) {
    selectedSubProcessId.value = subProcessId;
    
    // Only redirect if we came from a query parameter
    if (route.query.subProcessId) {
      router.replace({ path: '/risk-control' });
    }
  }
});

// Handle audit process selection
const handleAuditProcessSelected = (auditProcessId) => {
  selectedAuditProcessId.value = auditProcessId;
  // Switch to sub processes tab when an audit process is selected
  activeTab.value = 'subprocesses';
};

// Handle sub process selection
const handleSubProcessSelected = (subProcessId) => {
  selectedSubProcessId.value = subProcessId;
  // Switch to risk control matrix tab when a sub process is selected
  activeTab.value = 'matrix';
};

// Handle risk control matrix selection
const handleRiskControlMatrixSelected = (riskControlMatrixId) => {
  console.log('Risk control matrix selected:', riskControlMatrixId);
  // Handle risk control matrix selection logic here
};

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
