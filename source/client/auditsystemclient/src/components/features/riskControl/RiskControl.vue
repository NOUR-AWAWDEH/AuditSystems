<template>
  <!-- RiskControl.vue -->
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
            v-model:selectedObjectiveId="selectedObjectiveId"
            @objective-selected="handleObjectiveSelected"
            @add-risks="handleAddRisks"
          />
        </div>

        <div v-if="activeTab === 'risks'" class="p-3 sm:p-4 md:p-6">
          <RiskVue
            v-if="showAddRiskForm"
            :objectives="objectives"
            :selected-objective-id="selectedObjectiveId"
          />
        </div>

        <!-- Exit Controls -->
        <div v-if="activeTab === 'exitcontrols'" class="p-3 sm:p-4 md:p-6">
          <ExitControlVue 
            :objectives="objectives"
            :selected-objective-id="selectedObjectiveId"
            @exit-control-selected="handleExitControlSelected"
          />
        </div>
        

        <div v-if="activeTab === 'controldesigns'" class="p-3 sm:p-4 md:p-6">
         <ControlDesignVue
            :exit-controls="exitControls"
            :selected-exit-control-id="selectedExitControlId"
            @control-design-selected="handleControlDesignSelected"
         />
        </div>
        
        <!-- Audit Programs -->
        <div v-if="activeTab === 'auditprograms'" class="p-3 sm:p-4 md:p-6">
          <AuditProgramVue
            :exit-controls="exitControls"
            :selected-exit-control-id="selectedExitControlId"
            @audit-program-selected="handleAuditProgramSelected"
          />
        </div>

        <div v-if="activeTab === 'observations'" class="p-3 sm:p-4 md:p-6">
          <Observation
            :audit-programs="auditPrograms"
            :selected-audit-program-id="selectedAuditProgramId"
            @observation-selected="handleObservationSelected"
          />
        </div>
        <!-- Recommendations -->
        <div v-if="activeTab === 'recommendations'" class="p-3 sm:p-4 md:p-6">
         <h2 class="text-lg font-bold text-gray-800 mb-4">Recommendations Comming Soon</h2>
          <!-- <Recommendation
            :observations="observations"
            :selected-observation-id="selectedObservationId"
          /> -->
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
import ExitControlVue from './exitControl/ExitControlVue.vue';
import AuditProgramVue from './auditProgram/AuditProgramVue.vue';
import ControlDesignVue from './controlDesign/ControlDesignVue.vue';
import Observation from './observation/Observation.vue';

import { useGetSubDepartmentsByUserId } from '@/composables/organization/subdepartment/useGetSubDepartmentsByUserId';
import { useGetAuditProcessesByUserId } from '@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId';
import { useGetSubProcessesByUserId } from '@/composables/riskControl/subProcess/useGetSubProcessesByUserId';
import { useGetRiskControlMatricesByUserId } from '@/composables/riskControl/riskControlMatrix/useGetRiskControlMatricesByUserId';
import { useGetObjectivesByUserId } from '@/composables/riskControl/objective/useGetObjectivesByUserId';
import { useGetExitControlsByUserId } from '@/composables/riskControl/exitControl/useGetExitControlsByUserId';
import { useGetAuditProgramsByUserId } from '@/composables/riskControl/auditProgram/useGetAuditProgramsByUserId';
import { useGetObservationsByUserId } from '@/composables/riskControl/observation/useGetObservationsByUserId';

// State
const activeTab = ref('processes');
const isLoading = ref(false);
const errorMessage = ref(null);
const showAddRiskForm = ref(false);

// Selected IDs
const selectedSubDepartmentId = ref(localStorage.getItem("organizationSelectedSubDepartmentId") || '');
const selectedAuditProcessId = ref(localStorage.getItem("riskControlSelectedAuditProcessId") || '');
const selectedSubProcessId = ref(localStorage.getItem("riskControlSelectedSubProcessId") || '');
const selectedRiskControlMatrixId = ref(localStorage.getItem("riskControlSelectedRiskControlMatrixId") || '');
const selectedObjectiveId = ref(localStorage.getItem("riskControlSelectedObjectiveId") || '');
const selectedExitControlId = ref(localStorage.getItem("riskControlSelectedExitControlId") || '');
const selectedControlDesignId = ref(localStorage.getItem("riskControlSelectedControlDesignId") || '');
const selectedAuditProgramId = ref(localStorage.getItem("riskControlSelectedAuditProgramId") || '');
const selectedObservationId = ref(localStorage.getItem("riskControlSelectedObservationId") || '');


// Composables
const { getSubDepartmentsByUserId, subDepartments } = useGetSubDepartmentsByUserId();
const { getAuditProcessesByUserId, auditProcesses } = useGetAuditProcessesByUserId();
const { getSubProcessesByUserId, subProcesses } = useGetSubProcessesByUserId();
const { getRiskControlMatricesByUserId, riskControlMatrices } = useGetRiskControlMatricesByUserId();
const { getObjectivesByUserId, objectives } = useGetObjectivesByUserId();
const { getExitControlsByUserId, exitControls } = useGetExitControlsByUserId();
const { getAuditProgramsByUserId, auditPrograms } = useGetAuditProgramsByUserId();
const { getObservationsByUserId, observations } = useGetObservationsByUserId();

// Tabs
const tabs = ref([
  { id: 'processes', name: 'Processes' },
  { id: 'subprocesses', name: 'Sub Processes' },
  { id: 'matrix', name: 'Risk Control Matrix' },
  { id: 'objectives', name: 'Objectives' },
  { id: 'risks', name: 'Risks' },
  { id: 'exitcontrols', name: 'Exit Controls' },
  { id: 'controldesigns', name: 'Control Design' },
  { id: 'auditprograms', name: 'Audit Programs' },
  { id: 'observations', name: 'Observations' },
  { id: 'recommendations', name: 'Recommendations' }
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
      case 'exitcontrols':
        await getObjectivesByUserId();
        break;
      case 'controldesigns':
        await getExitControlsByUserId();
        break;
      case 'auditprograms':
        await getExitControlsByUserId(); 
        break;
      case 'observations':
        await getAuditProgramsByUserId();
        break;
      case 'recommendations':
        await getObservationsByUserId();
        break;
    }

  } catch (err) {
    console.error('Error fetching data:', err);
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
  setActiveTab('exitcontrols');
};

const handleExitControlSelected = (exitControl) => { 
  selectedExitControlId.value = exitControl.id;
  if (exitControl.inadequate) { 
    setActiveTab('controldesigns');
  } else {
    setActiveTab('auditprograms');
  }
};

const handleControlDesignSelected = (controlDesign) => {
  selectedControlDesignId.value = controlDesign.id;
  setActiveTab('auditprograms');
};

const handleAuditProgramSelected = (id) => {
  selectedAuditProgramId.value = id;
  setActiveTab('observations');
};

const handleObservationSelected = (id) => {
  selectedObservationId.value = id;
  setActiveTab('recommendations');
};

const handleAddRisks = (objectiveId) => {
  selectedObjectiveId.value = objectiveId;
  activeTab.value = 'risks';
  showAddRiskForm.value = true;
};

// // Init -> load processes tab
onMounted(() => {
  setActiveTab('processes');
});

</script>