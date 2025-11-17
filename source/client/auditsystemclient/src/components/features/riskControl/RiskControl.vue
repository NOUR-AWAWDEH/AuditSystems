<template>
  <!-- RiskControl.vue -->
  <div
    class="min-h-[calc(100vh-4rem)] bg-gray-50 p-2 sm:p-3 md:p-6 lg:p-8 xl:p-10 transition-all duration-300"
  >
    <!-- Header Section -->
    <div class="mb-3 sm:mb-5 md:mb-8">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-2 sm:gap-4">
        <RiskControlHeader />
      </div>
    </div>

    <!-- Navigation Tabs -->
    <div class="mb-3 sm:mb-6 border-b border-gray-200">
      <!-- Dropdown for small screens -->
      <div class="md:hidden mb-2">
        <select
          v-model="activeTab"
          @change="setActiveTab(activeTab)"
          class="w-full p-2 border border-gray-300 rounded-lg bg-white text-sm focus:outline-none focus:ring-2 focus:ring-primary-base"
        >
          <option v-for="tab in tabs" :key="tab.id" :value="tab.id">
            {{ tab.name }}
          </option>
        </select>
      </div>

      <!-- Tabs for medium+ screens -->
      <nav class="hidden md:flex flex-wrap overflow-x-auto scrollbar-hide">
        <button
          v-for="tab in tabs"
          :key="tab.id"
          @click="setActiveTab(tab.id)"
          :class="[
            'whitespace-nowrap border-b-2 font-medium shrink-0 px-2 py-2 text-sm md:text-base transition-colors duration-200',
            activeTab === tab.id
              ? 'border-primary-base text-primary-base'
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
          ]"
        >
          {{ tab.name }}
        </button>
      </nav>
    </div>

    <!-- Tab Content -->
    <section
      class="bg-white rounded-lg sm:rounded-xl md:rounded-2xl shadow-sm border border-gray-200"
    >
      <div
        v-if="isLoading"
        class="p-3 sm:p-5 md:p-8 text-center text-sm sm:text-base text-gray-500"
      >
        Loading...
      </div>

      <div
        v-else-if="errorMessage"
        class="p-3 sm:p-5 md:p-8 text-center text-sm sm:text-base text-red-500"
      >
        {{ errorMessage }}
      </div>

      <div v-else>
        <!-- Process Management -->
        <div v-if="activeTab === 'processes'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <AuditProcess
            :sub-departments="subDepartments"
            :selected-sub-department-id="selectedSubDepartmentId"
            @audit-process-selected="handleAuditProcessSelected"
          />
        </div>

        <!-- SubProcess component -->
        <div v-if="activeTab === 'subprocesses'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <SubProcess
            :audit-processes="auditProcesses"
            :selected-audit-process-id="selectedAuditProcessId"
            @sub-process-selected="handleSubProcessSelected"
          />
        </div>

        <!-- Risk Control Matrix -->
        <div v-if="activeTab === 'matrix'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <RiskControlMatrix
            :sub-processes="subProcesses"
            :selected-sub-process-id="selectedSubProcessId"
            @risk-control-matrix-selected="handleRiskControlMatrixSelected"
          />
        </div>

        <!-- Objectives -->
        <div v-if="activeTab === 'objectives'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <Objective
            :risk-control-matrices="riskControlMatrices"
            :selected-risk-control-matrix-id="selectedRiskControlMatrixId"
            v-model:selectedObjectiveId="selectedObjectiveId"
            @objective-selected="handleObjectiveSelected"
            @add-risks="handleAddRisks"
          />
        </div>

        <div v-if="activeTab === 'risks'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <RiskVue
            v-if="showAddRiskForm"
            :objectives="objectives"
            :selected-objective-id="selectedObjectiveId"
          />
        </div>

        <!-- Exit Controls -->
        <div v-if="activeTab === 'exitcontrols'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <ExitControlVue
            :objectives="objectives"
            :selected-objective-id="selectedObjectiveId"
            @exit-control-selected="handleExitControlSelected"
          />
        </div>

        <div v-if="activeTab === 'controldesigns'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <ControlDesignVue
            :exit-controls="exitControls"
            :selected-exit-control-id="selectedExitControlId"
            @control-design-selected="handleControlDesignSelected"
          />
        </div>

        <!-- Audit Programs -->
        <div v-if="activeTab === 'auditprograms'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <AuditProgramVue
            :exit-controls="exitControls"
            :selected-exit-control-id="selectedExitControlId"
            @audit-program-selected="handleAuditProgramSelected"
          />
        </div>

        <div v-if="activeTab === 'observations'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <Observation
            :audit-programs="auditPrograms"
            :selected-audit-program-id="selectedAuditProgramId"
            @observation-selected="handleObservationSelected"
          />
        </div>
        <!-- Recommendations -->
        <div
          v-if="activeTab === 'recommendations'"
          class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto"
        >
          <h2 class="text-lg font-bold text-gray-800 mb-4">Recommendations Coming Soon</h2>
          <!-- <Recommendation
            :observations="observations"
            :selected-observation-id="selectedObservationId"
          /> -->
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import RiskControlHeader from "./components/RiskControlHeader.vue";
import AuditProcess from "./auditProcess/AuditProcess.vue";
import SubProcess from "./subProcess/SubProcess.vue";
import RiskControlMatrix from "./riskControlMatrix/RiskControlMatrix.vue";
import Objective from "./objective/ObjectiveVue.vue";
import RiskVue from "./risk/RiskVue.vue";
import ExitControlVue from "./exitControl/ExitControlVue.vue";
import AuditProgramVue from "./auditProgram/AuditProgramVue.vue";
import ControlDesignVue from "./controlDesign/ControlDesignVue.vue";
import Observation from "./observation/Observation.vue";

import { useGetSubDepartmentsByUserId } from "@/composables/organization/subdepartment/useGetSubDepartmentsByUserId";
import { useGetAuditProcessesByUserId } from "@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId";
import { useGetSubProcessesByUserId } from "@/composables/riskControl/subProcess/useGetSubProcessesByUserId";
import { useGetRiskControlMatricesByUserId } from "@/composables/riskControl/riskControlMatrix/useGetRiskControlMatricesByUserId";
import { useGetObjectivesByUserId } from "@/composables/riskControl/objective/useGetObjectivesByUserId";
import { useGetExitControlsByUserId } from "@/composables/riskControl/exitControl/useGetExitControlsByUserId";
import { useGetAuditProgramsByUserId } from "@/composables/riskControl/auditProgram/useGetAuditProgramsByUserId";
import { useGetObservationsByUserId } from "@/composables/riskControl/observation/useGetObservationsByUserId";

// State
const activeTab = ref("processes");
const isLoading = ref(false);
const errorMessage = ref(null);
const showAddRiskForm = ref(false);

// Selected IDs
const selectedSubDepartmentId = ref(
  localStorage.getItem("organizationSelectedSubDepartmentId") || ""
);
const selectedAuditProcessId = ref(localStorage.getItem("riskControlSelectedAuditProcessId") || "");
const selectedSubProcessId = ref(localStorage.getItem("riskControlSelectedSubProcessId") || "");
const selectedRiskControlMatrixId = ref(
  localStorage.getItem("riskControlSelectedRiskControlMatrixId") || ""
);
const selectedObjectiveId = ref(localStorage.getItem("riskControlSelectedObjectiveId") || "");
const selectedExitControlId = ref(localStorage.getItem("riskControlSelectedExitControlId") || "");
const selectedControlDesignId = ref(
  localStorage.getItem("riskControlSelectedControlDesignId") || ""
);
const selectedAuditProgramId = ref(localStorage.getItem("riskControlSelectedAuditProgramId") || "");
const selectedObservationId = ref(localStorage.getItem("riskControlSelectedObservationId") || "");

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
  { id: "processes", name: "Processes" },
  { id: "subprocesses", name: "Sub Processes" },
  { id: "matrix", name: "Risk Control Matrix" },
  { id: "objectives", name: "Objectives" },
  { id: "risks", name: "Risks" },
  { id: "exitcontrols", name: "Exit Controls" },
  { id: "controldesigns", name: "Control Design" },
  { id: "auditprograms", name: "Audit Programs" },
  { id: "observations", name: "Observations" },
  { id: "recommendations", name: "Recommendations" },
]);

// Set active tab & always refresh its data
const setActiveTab = async (tabId) => {
  activeTab.value = tabId;
  isLoading.value = true;
  errorMessage.value = null;

  try {
    switch (tabId) {
      case "processes":
        await getSubDepartmentsByUserId();
        break;
      case "subprocesses":
        await getAuditProcessesByUserId();
        break;
      case "matrix":
        await getSubProcessesByUserId();
        break;
      case "objectives":
        await getRiskControlMatricesByUserId();
        break;
      case "risks":
        await getObjectivesByUserId();
        break;
      case "exitcontrols":
        await getObjectivesByUserId();
        break;
      case "controldesigns":
        await getExitControlsByUserId();
        break;
      case "auditprograms":
        await getExitControlsByUserId();
        break;
      case "observations":
        await getAuditProgramsByUserId();
        break;
      case "recommendations":
        await getObservationsByUserId();
        break;
    }
  } catch (err) {
    console.error("Error fetching data:", err);
    errorMessage.value = "Failed to load data";
  } finally {
    isLoading.value = false;
  }
};

// Handlers for selection
const handleAuditProcessSelected = (id) => {
  selectedAuditProcessId.value = id;
  setActiveTab("subprocesses");
};

const handleSubProcessSelected = (id) => {
  selectedSubProcessId.value = id;
  setActiveTab("matrix");
};

const handleRiskControlMatrixSelected = (id) => {
  selectedRiskControlMatrixId.value = id;
  setActiveTab("objectives");
};

const handleObjectiveSelected = (id) => {
  selectedObjectiveId.value = id;
  setActiveTab("exitcontrols");
};

const handleExitControlSelected = (exitControl) => {
  selectedExitControlId.value = exitControl.id;
  if (exitControl.inadequate) {
    setActiveTab("controldesigns");
  } else {
    setActiveTab("auditprograms");
  }
};

const handleControlDesignSelected = (controlDesign) => {
  selectedControlDesignId.value = controlDesign.id;
  setActiveTab("auditprograms");
};

const handleAuditProgramSelected = (id) => {
  selectedAuditProgramId.value = id;
  setActiveTab("observations");
};

const handleObservationSelected = (id) => {
  selectedObservationId.value = id;
  setActiveTab("recommendations");
};

const handleAddRisks = (objectiveId) => {
  selectedObjectiveId.value = objectiveId;
  activeTab.value = "risks";
  showAddRiskForm.value = true;
};

// // Init -> load processes tab
onMounted(() => {
  setActiveTab("processes");
});
</script>
