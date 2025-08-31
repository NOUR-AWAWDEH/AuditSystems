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
            @exit-risk-selected="handleExitRiskSelected"
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
import { ref, onMounted } from 'vue';
import RiskControlHeader from './components/RiskControlHeader.vue';
import AuditProcess from './auditProcess/AuditProcess.vue';
import SubProcess from './subProcess/SubProcess.vue';
import RiskControlMatrix from './riskControlMatrix/RiskControlMatrix.vue';
import Objective from './objective/ObjectiveVue.vue';
import RiskVue from './risk/RiskVue.vue';

import { useGetSubDepartmentsByUserId } from '@/composables/organization/subdepartment/useGetSubDepartmentsByUserId';
import { useGetAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId';
import { useGetSubProcessesByUserId } from '@/composables/riskControl/subProcess/useGetSubProcessesByUserId';
import { useGetRiskControlMatricesByUserId } from '@/composables/riskControl/riskControlMatrix/useGetRiskControlMatricesByUserId';
import { useGetObjectivesByUserId } from '@/composables/riskControl/objective/useGetObjectivesByUserId';
import { useGetRisksByUserId } from '@/composables/riskControl/risk/useGetRisksByUserId';

// State
const activeTab = ref('processes');
const isLoading = ref(false);
const errorMessage = ref(null);

// Selected IDs
const selectedSubDepartmentId = ref(null);
const selectedAuditProcessId = ref(null);
const selectedSubProcessId = ref(null);
const selectedRiskControlMatrixId = ref(null);
const selectedObjectiveId = ref(null);
const selectedRiskId = ref(null);

// Composables
const { getSubDepartmentsByUserId, subDepartments } = useGetSubDepartmentsByUserId();
const { getAuditProcessesByUserId, auditProcesses } = useGetAuditProcessesByUserId();
const { getSubProcessesByUserId, subProcesses } = useGetSubProcessesByUserId();
const { getRiskControlMatricesByUserId, riskControlMatrices } = useGetRiskControlMatricesByUserId();
const { getObjectivesByUserId, objectives } = useGetObjectivesByUserId();
const { getRisksByUserId, risks } = useGetRisksByUserId();

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

// Set active tab & always refresh its data
const setActiveTab = async (tabId) => {
  activeTab.value = tabId;
  isLoading.value = true;
  errorMessage.value = null;

  try {
    switch (tabId) {
      case 'processes':
        await getSubDepartmentsByUserId();
        break;
      case 'subprocesses':
        await getAuditProcessesByUserId();
        break;
      case 'matrix':
        await getSubProcessesByUserId();
        break;
      case 'objectives':
        await getRiskControlMatricesByUserId();
        break;
      case 'risks':
        await getObjectivesByUserId();
        break;
      case 'exitrisks':
        // if you want to fetch exit risks, put call here
        break;
      case 'controls':
        // fetch controls if needed
        break;
      case 'observations':
        // fetch observations if needed
        break;
    }
  } catch (err) {
    errorMessage.value = 'Failed to load data';
  } finally {
    isLoading.value = false;
  }
};

// Handlers for selection
const handleAuditProcessSelected = (id) => {
  selectedAuditProcessId.value = id;
  setActiveTab('subprocesses');
};

const handleSubProcessSelected = (id) => {
  selectedSubProcessId.value = id;
  setActiveTab('matrix');
};

const handleRiskControlMatrixSelected = (id) => {
  selectedRiskControlMatrixId.value = id;
  setActiveTab('objectives');
};

const handleObjectiveSelected = (id) => {
  selectedObjectiveId.value = id;
  setActiveTab('risks');
};

const handleRiskSelected = (id) => {
  selectedRiskId.value = id;
  setActiveTab('exitrisks');
};

const handleExitRiskSelected = (id) => {
  selectedRiskId.value = id;
  setActiveTab('controls');
};

// Init -> load processes tab
onMounted(async () => {
  await setActiveTab('processes');
});
</script>
