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
            :audit-processes="auditProcesses"
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

// Composables
const { getSubDepartmentsByUserId, subDepartments } = useGetSubDepartmentsByUserId();
const { getSubDepartmentById } = useGetSubDepartmentById();

const { getAuditProcessesByUserId, auditProcesses } = useGetAuditProcessesByUserId();
const { getAuditProcessById } = useGetAuditProcessById();

const { getSubProcessesByUserId, subProcesses } = useGetSubProcessesByUserId();
const { getSubProcessById } = useGetSubProcessById();

const { getRiskControlMatricesByUserId, riskControlMatrices } = useGetRiskControlMatricesByUserId();
const { getRiskControlMatrixById } = useGetRiskControlMatrixById();

const { getObjectivesByUserId, objectives } = useGetObjectivesByUserId();
const { getObjectiveById } = useGetObjectiveById();

const { getRisksByUserId, risks } = useGetRisksByUserId();
const { getRiskById } = useGetRiskById();

// Computed normalized lists
const normalizedSubProcesses = computed(() => Array.isArray(subProcesses.value) ? subProcesses.value : []);
const normalizedRiskControlMatrices = computed(() => Array.isArray(riskControlMatrices.value) ? riskControlMatrices.value : []);
const normalizedObjectives = computed(() => Array.isArray(objectives.value) ? objectives.value : []);
const normalizedRisks = computed(() => Array.isArray(risks.value) ? risks.value : []);

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

// Selection handlers
const setSelectedId = (key, value) => {
  switch (key) {
    case 'subDepartment': selectedSubDepartmentId.value = value; localStorage.setItem('riskControlSelectedSubDepartmentId', value || ''); break;
    case 'auditProcess': selectedAuditProcessId.value = value; localStorage.setItem('riskControlSelectedAuditProcessId', value || ''); break;
    case 'subProcess': selectedSubProcessId.value = value; localStorage.setItem('riskControlSelectedSubProcessId', value || ''); break;
    case 'riskControlMatrix': selectedRiskControlMatrixId.value = value; localStorage.setItem('riskControlSelectedRiskControlMatrixId', value || ''); break;
    case 'objective': selectedObjectiveId.value = value; localStorage.setItem('riskControlSelectedObjectiveId', value || ''); break;
    case 'risk': selectedRiskId.value = value; localStorage.setItem('riskControlSelectedRiskId', value || ''); break;
  }
};

// Event handlers
const handleAuditProcessSelected = async (id) => {
  setSelectedId('auditProcess', id);
  if (!normalizedSubProcesses.value.length) await refreshAuditProcesses();
  activeTab.value = 'subprocesses';
};

const handleSubProcessSelected = async (id) => {
  setSelectedId('subProcess', id);
  if (!normalizedRiskControlMatrices.value.length) await refreshSubProcesses();
  activeTab.value = 'matrix';
};

const handleRiskControlMatrixSelected = async (id) => {
  setSelectedId('riskControlMatrix', id);
  if (!normalizedObjectives.value.length) await refreshRiskControlMatrices();
  activeTab.value = 'objectives';
};

const handleObjectiveSelected = async (id) => {
  setSelectedId('objective', id);
  if (!normalizedRisks.value.length) await refreshObjectives();
  activeTab.value = 'risks';
};

const handleRiskSelected = async (id) => {
  setSelectedId('risk', id);
};

// Refreshers
const refreshSubDepartments = async () => await getSubDepartmentsByUserId();
const refreshAuditProcesses = async () => await getAuditProcessesByUserId();
const refreshSubProcesses = async () => await getSubProcessesByUserId();
const refreshRiskControlMatrices = async () => await getRiskControlMatricesByUserId();
const refreshObjectives = async () => await getObjectivesByUserId();
const refreshRisks = async () => await getRisksByUserId();

// Tab navigation
const setActiveTab = (tabId) => activeTab.value = tabId;

// Handle route queries
const handleRouteQueries = async () => {
  const { subDepartmentId, auditProcessId, subProcessId, riskControlMatrixId, objectiveId, riskId } = route.query;

  if (subDepartmentId) { selectedSubDepartmentId.value = subDepartmentId; await getSubDepartmentById(subDepartmentId); }
  if (auditProcessId) { selectedAuditProcessId.value = auditProcessId; await getAuditProcessById(auditProcessId); }
  if (subProcessId) { selectedSubProcessId.value = subProcessId; await getSubProcessById(subProcessId); }
  if (riskControlMatrixId) { selectedRiskControlMatrixId.value = riskControlMatrixId; await getRiskControlMatrixById(riskControlMatrixId); }
  if (objectiveId) { selectedObjectiveId.value = objectiveId; await getObjectiveById(objectiveId); }
  if (riskId) { selectedRiskId.value = riskId; await getRiskById(riskId); }

  // Clean up URL
  if (subDepartmentId || auditProcessId || subProcessId || riskControlMatrixId || objectiveId || riskId) {
    await router.replace({ path: '/risk-control' });
  }
};

// Init
onMounted(async () => {
  await refreshSubDepartments();
  await refreshAuditProcesses();
  await handleRouteQueries();
});

watch(() => route.query, handleRouteQueries);

</script>