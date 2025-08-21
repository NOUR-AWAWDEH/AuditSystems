// RiskControl.vue
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
          <Objective
            :risk-control-matrices="riskControlMatrices"
            :selected-risk-control-matrix-id="selectedRiskControlMatrixId"
            @objective-selected="handleObjectiveSelected"
          />
        </div>

        <!-- Risks -->
        <div v-if="activeTab === 'risks'" class="p-3 sm:p-4 md:p-6">
          <RiskVue
            :objectives="objectives"
            :selected-objective-id="selectedObjectiveId"
            @risk-selected="handleRiskSelected"
          />
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
import { ref, onMounted, watch, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';

import RiskControlHeader from './components/RiskControlHeader.vue';
import AuditProcess from './auditProcess/AuditProcess.vue';
import SubProcess from './subProcess/SubProcess.vue';
import RiskControlMatrix from './riskControlMatrix/RiskControlMatrix.vue';
import Objective from './objective/ObjectiveVue.vue';
import RiskVue from './risk/RiskVue.vue';

import { useGetSubDepartmentsByUserId } from '@/composables/organization/subdepartment/useGetSubDepartmentsByUserId';
import { useGetSubDepartmentById } from '@/composables/organization/subdepartment/useGetSubDepartmentById';
import { useGetAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId';
import { useGetAuditProcessById } from '@/composables/riskControl/auditProcess/useGetAuditProcessById';
import { useGetSubProcessesByUserId } from '@/composables/riskControl/subProcess/useGetSubProcessesByUserId';
import { useGetSubProcessById } from '@/composables/riskControl/subProcess/useGetSubProcessById';
import { useGetRiskControlMatricesByUserId } from '@/composables/riskControl/riskControlMatrix/useGetRiskControlMatricesByUserId';
import { useGetRiskControlMatrixById } from '@/composables/riskControl/riskControlMatrix/useGetRiskControlMatrixById';
import { useGetObjectivesByUserId } from '@/composables/riskControl/objective/useGetObjectivesByUserId';
import { useGetObjectiveById } from '@/composables/riskControl/objective/useGetObjectiveById';
import { useGetRisksByUserId } from '@/composables/riskControl/risk/useGetRisksByUserId';
import { useGetRiskById } from '@/composables/riskControl/risk/useGetRiskById';


// Router
const route = useRoute();
const router = useRouter();

// State
const activeTab = ref('processes');
const selectedSubDepartmentId = ref(localStorage.getItem('riskControlSelectedSubDepartmentId') || null);
const selectedAuditProcessId = ref(localStorage.getItem('riskControlSelectedAuditProcessId') || null);
const selectedSubProcessId = ref(localStorage.getItem('riskControlSelectedSubProcessId') || null);
const selectedRiskControlMatrixId = ref(localStorage.getItem('riskControlSelectedRiskControlMatrixId') || null);
const selectedObjectiveId = ref(localStorage.getItem('riskControlSelectedObjectiveId') || null);
const selectedRiskId = ref(localStorage.getItem('riskControlSelectedRiskId') || null);


// Call the Composables

// Sub Departments
const { getSubDepartmentsByUserId, subDepartments } = useGetSubDepartmentsByUserId();
const { getSubDepartmentById } = useGetSubDepartmentById();

// Audit Processes
const { getAuditProcessesByUserId, auditProcesses } = useGetAuditProcessesByUserId();
const { getAuditProcessById } = useGetAuditProcessById();

// Sub Processes
const { getSubProcessesByUserId, subProcesses } = useGetSubProcessesByUserId();
const { getSubProcessById } = useGetSubProcessById();

// Risk Control Matrices
const { getRiskControlMatricesByUserId, riskControlMatrices } = useGetRiskControlMatricesByUserId();
const { getRiskControlMatrixById } = useGetRiskControlMatrixById();

// Objectives
const { getObjectivesByUserId, objectives } = useGetObjectivesByUserId();
const { getObjectiveById } = useGetObjectiveById();

// Risks
const { getRisksByUserId, risks } = useGetRisksByUserId();
const { getRiskById } = useGetRiskById();



// Computed normalized lists
const normalizedSubProcesses = computed(() => Array.isArray(subProcesses.value) ? subProcesses.value.flat() : []);
const normalizedRiskControlMatrices = computed(() => Array.isArray(riskControlMatrices.value) ? riskControlMatrices.value.flat() : []);
const normalizedObjectives = computed(() => Array.isArray(objectives.value) ? objectives.value.flat() : []);
const normalizedRisks = computed(() => Array.isArray(risks.value) ? risks.value.flat() : []);
//const normalizedExitRisks = computed(() => Array.isArray(exitRisks.value) ? exitRisks.value.flat() : []);
//const normalizedObservations = computed(() => Array.isArray(observations.value) ? observations.value.flat() : []);





const isLoading = ref(false);
const errorMessage = ref(null);

// Tabs
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

// Fetch all required data
const fetchData = async () => {
  isLoading.value = true;
  errorMessage.value = null;

  try {
    const [subDeptResult, auditResult, subProcResult, riskMatrixResult, objectiveResult, riskResult] = await Promise.all([
      getSubDepartmentsByUserId(),
      getAuditProcessesByUserId(),
      getSubProcessesByUserId(),
      getRiskControlMatricesByUserId(),
      getObjectivesByUserId(),
      getRisksByUserId(),
    ]);

    subDepartments.value = subDeptResult?.data || [];
    auditProcesses.value = auditResult?.data || [];
    subProcesses.value = subProcResult?.data || [];
    riskControlMatrices.value = riskMatrixResult?.data || [];
    objectives.value = objectiveResult?.data || [];
    risks.value = riskResult?.data || [];

  } catch (err) {
    errorMessage.value = err.message || 'Failed to fetch data';
  } finally {
    isLoading.value = false;
  }
};

// Selection handlers
const selectAuditProcess = (id) => {
  selectedAuditProcessId.value = id;
  localStorage.setItem('riskControlSelectedAuditProcessId', id || '');
};

const selectSubProcess = (id) => {
  selectedSubProcessId.value = id;
  localStorage.setItem('riskControlSelectedSubProcessId', id || '');
};

const selectRiskControlMatrix = (id) => {
  selectedRiskControlMatrixId.value = id;
  localStorage.setItem('riskControlSelectedRiskControlMatrixId', id || '');
};

const selectObjective = (id) => {
  selectedObjectiveId.value = id;
  localStorage.setItem('riskControlSelectedObjectiveId', id || '');
};

const selectRisk = (id) => {
  selectedRiskId.value = id;
  localStorage.setItem('riskControlSelectedRiskId', id || '');
};



// Event handlers
const handleAuditProcessSelected = async (id) => {
  selectAuditProcess(id);
  if (!normalizedSubProcesses.value.length) await fetchData();
  setActiveTab('subprocesses');
};

const handleSubProcessSelected = async (id) => {
  selectSubProcess(id);
  if (!normalizedRiskControlMatrices.value.length) await fetchData();
  setActiveTab('matrix');
};

const handleRiskControlMatrixSelected = async (id) => {
  selectRiskControlMatrix(id);
  if (!normalizedObjectives.value.length) await fetchData();
  setActiveTab('objectives');
};

const handleObjectiveSelected = async (id) => {
  selectObjective(id);
  if (!normalizedRisks.value.length) await fetchData();
  setActiveTab('risks');
};

const handleRiskSelected = async (id) => {
  selectRisk(id);
  //if (!normalizedControls.value.length) await fetchData();
  //setActiveTab('controls');
};



// Tab navigation
const setActiveTab = (tabId) => {
  activeTab.value = tabId;
};

// Handle route queries
const handleRouteQueries = async () => {
  const { subDepartmentId, auditProcessId, subProcessId, riskControlMatrixId, objectiveId, riskId } = route.query;

  if (subDepartmentId) {
    selectedSubDepartmentId.value = subDepartmentId;
    await getSubDepartmentById(subDepartmentId);
  }
  if (auditProcessId) {
    selectedAuditProcessId.value = auditProcessId;
    await getAuditProcessById(auditProcessId);
  }
  if (subProcessId) {
    selectedSubProcessId.value = subProcessId;
    await getSubProcessById(subProcessId);
  }
  if (riskControlMatrixId) {
    selectedRiskControlMatrixId.value = riskControlMatrixId;
    await getRiskControlMatrixById(riskControlMatrixId);
  }
  if (objectiveId) {
    selectedObjectiveId.value = objectiveId;
    await getObjectiveById(objectiveId);
  }
  if (riskId) {
    selectedRiskId.value = riskId;
    await getRiskById(riskId);
  }



  // Clean up URL
  if (subDepartmentId || auditProcessId || subProcessId || riskControlMatrixId || objectiveId || riskId) {
    await router.replace({ path: '/risk-control' });
  }
};

// Init
onMounted(async () => {
  await fetchData();
  await handleRouteQueries();
});

watch(() => route.query, handleRouteQueries);
</script>


<style scoped>
/* Existing styles remain unchanged */
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
