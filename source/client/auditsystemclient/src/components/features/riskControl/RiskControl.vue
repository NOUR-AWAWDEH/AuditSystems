<template>
  <!-- RiskControl.vue -->
  <div
    class="min-h-[calc(100vh)] bg-gray-50 p-2 sm:p-3 md:p-6 lg:p-8 xl:p-10 transition-all duration-300"
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
            'whitespace-nowrap border-b-2 font-medium shrink-0 px-2 xl:px-5 py-2 text-sm md:text-base transition-colors duration-200',
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
            @audit-process-deleted="handleAuditProcessDeleted"
          />
        </div>

        <!-- SubProcess component Removed -->

        <!-- Risk Control Matrix -->
        <div v-if="activeTab === 'matrix'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <RiskControlMatrix
            :audit-process-id="selectedAuditProcessId"
            @risk-control-matrix-selected="handleRiskControlMatrixSelected"
            @risk-control-matrix-deleted="handleRiskControlMatrixDeleted"
          />
        </div>

        <!-- Objectives -->
        <div v-if="activeTab === 'objectives'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <Objective
            :risk-control-matrices="riskControlMatrices"
            :selected-risk-control-matrix-id="selectedRiskControlMatrixId"
            v-model:selectedObjectiveId="selectedObjectiveId"
            @objective-selected="handleObjectiveSelected"
            @objective-deleted="handleObjectiveDeleted"
            @add-risks="handleAddRisks"
          />
        </div>

        <div v-if="activeTab === 'risks'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <RiskVue
            :objectives="objectives"
            :selected-objective-id="selectedObjectiveId"
            @risk-deleted="handleRiskDeleted"
          />
        </div>

        <!-- Exit Controls -->
        <div v-if="activeTab === 'exitcontrols'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <ExitControlVue
            :objectives="objectives"
            :selected-objective-id="selectedObjectiveId"
            @exit-control-selected="handleExitControlSelected"
            @exit-control-deleted="handleExitControlDeleted"
          />
        </div>

        <div v-if="activeTab === 'controldesigns'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <ControlDesignVue
            :exit-controls="exitControls"
            :selected-exit-control-id="selectedExitControlId"
            @control-design-selected="handleControlDesignSelected"
            @exit-control-deleted="handleControlDesignDeleted"
          />
        </div>

        <!-- Audit Programs -->
        <div v-if="activeTab === 'auditprograms'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <AuditProgramVue
            :exit-controls="exitControls"
            :selected-exit-control-id="selectedExitControlId"
            @audit-program-selected="handleAuditProgramSelected"
            @audit-program-deleted="handleAuditProgramDeleted"
          />
        </div>

        <div v-if="activeTab === 'observations'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <ObservationVue
            :audit-programs="auditPrograms"
            :selected-audit-program-id="selectedAuditProgramId"
            @observation-selected="handleObservationSelected"
            @observation-deleted="handleObservationDeleted"
          />
        </div>
        <!-- Recommendations -->
        <div
          v-if="activeTab === 'recommendations'"
          class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto"
        >
          <RecommendationVue
            :observations="observations"
            :selected-observation-id="selectedObservationId"
            @recommendation-deleted="handleRecommendationDeleted"
          />
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";

import RiskControlHeader from "./components/RiskControlHeader.vue";
import AuditProcess from "./auditProcess/AuditProcess.vue";
import RiskControlMatrix from "./riskControlMatrix/RiskControlMatrix.vue";
import Objective from "./objective/ObjectiveVue.vue";
import RiskVue from "./risk/RiskVue.vue";
import ExitControlVue from "./exitControl/ExitControlVue.vue";
import AuditProgramVue from "./auditProgram/AuditProgramVue.vue";
import ControlDesignVue from "./controlDesign/ControlDesignVue.vue";
import ObservationVue from "./observation/ObservationVue.vue";
import RecommendationVue from "./recommendation/RecommendationVue.vue";

import { useGetSubDepartmentsByUserId } from "@/composables/organization/subdepartment/useGetSubDepartmentsByUserId";
import { useGetAuditProcessesByUserId } from "@/composables/riskControl/auditProcess/useGetAuditProcessesByUserId";
import { useGetRiskControlMatricesByUserId } from "@/composables/riskControl/riskControlMatrix/useGetRiskControlMatricesByUserId";
import { useGetObjectivesByUserId } from "@/composables/riskControl/objective/useGetObjectivesByUserId";
import { useGetExitControlsByUserId } from "@/composables/riskControl/exitControl/useGetExitControlsByUserId";
import { useGetAuditProgramsByUserId } from "@/composables/riskControl/auditProgram/useGetAuditProgramsByUserId";
import { useGetObservationsByUserId } from "@/composables/riskControl/observation/useGetObservationsByUserId";

const route = useRoute();
const router = useRouter();

// State
const activeTab = ref("processes");

// Selected IDs
const selectedSubDepartmentId = ref(localStorage.getItem("organizationSelectedSubDepartmentId") || null);
const selectedAuditProcessId = ref(localStorage.getItem("riskControlSelectedAuditProcessId") || null);
const selectedRiskControlMatrixId = ref(localStorage.getItem("riskControlSelectedRiskControlMatrixId") || null);
const selectedObjectiveId = ref(localStorage.getItem("riskControlSelectedObjectiveId") || null);
const selectedExitControlId = ref(localStorage.getItem("riskControlSelectedExitControlId") || null);
const selectedControlDesignId = ref(localStorage.getItem("riskControlSelectedControlDesignId") || null);
const selectedAuditProgramId = ref(localStorage.getItem("riskControlSelectedAuditProgramId") || null);
const selectedObservationId = ref(localStorage.getItem("riskControlSelectedObservationId") || null);

// Composables
const { getSubDepartmentsByUserId, subDepartments } = useGetSubDepartmentsByUserId();
const { getAuditProcessesByUserId, auditProcesses } = useGetAuditProcessesByUserId();
const { getRiskControlMatricesByUserId, riskControlMatrices } = useGetRiskControlMatricesByUserId();
const { getObjectivesByUserId, objectives } = useGetObjectivesByUserId();
const { getExitControlsByUserId, exitControls } = useGetExitControlsByUserId();
const { getAuditProgramsByUserId, auditPrograms } = useGetAuditProgramsByUserId();
const { getObservationsByUserId, observations } = useGetObservationsByUserId();

const isLoading = ref(false);
const errorMessage = ref(null);

// Tabs
const tabs = ref([
  { id: "processes", name: "Processes" },
  { id: "matrix", name: "Risk Control Matrix" },
  { id: "objectives", name: "Objectives" },
  { id: "risks", name: "Risks" },
  { id: "exitcontrols", name: "Exit Controls" },
  { id: "controldesigns", name: "Control Design" },
  { id: "auditprograms", name: "Audit Programs" },
  { id: "observations", name: "Observations" },
  { id: "recommendations", name: "Recommendations" },
]);

const storageKeys = {
  processes: "riskControlSelectedAuditProcessId",
  matrix: "riskControlSelectedRiskControlMatrixId",
  objectives: "riskControlSelectedObjectiveId",
  exitcontrols: "riskControlSelectedExitControlId",
  controldesigns: "riskControlSelectedControlDesignId",
  auditprograms: "riskControlSelectedAuditProgramId",
  observations: "riskControlSelectedObservationId",
};

const selectionOrder = [
  "processes",
  "matrix",
  "objectives",
  "exitcontrols",
  "controldesigns",
  "auditprograms",
  "observations"
];

const clearSelectionCascade = (fromKey) => {
  const start = selectionOrder.indexOf(fromKey);
  if (start === -1) return;
  selectionOrder.slice(start).forEach((key) => setSelectedId(key, null));
};

const setSelectedId = (key, value) => {
  switch (key) {
    case "processes":
      selectedAuditProcessId.value = value;
      break;
    case "matrix":
      selectedRiskControlMatrixId.value = value;
      break;
    case "objectives":
      selectedObjectiveId.value = value;
      break;
    case "exitcontrols":
      selectedExitControlId.value = value;
      break;
    case "controldesigns":
      selectedControlDesignId.value = value;
      break;
    case "auditprograms":
      selectedAuditProgramId.value = value;
      break;
    case "observations":
      selectedObservationId.value = value;
      break;
    default:
      return;
  }

  const storageKey = storageKeys[key];
  if (!storageKey) return;

  if (value) {
    localStorage.setItem(storageKey, value);
  } else {
    localStorage.removeItem(storageKey);
  }
};

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
      case "matrix":
        // await getSubProcessesByUserId(); // Removed - already removed but leaving this chunk to match previous edit boundaries if needed? No, logic was removed
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

const handleObservationSelected = async (id) => {
  selectedObservationId.value = id;
  await getObservationsByUserId();
  setActiveTab("recommendations");
};

// Add Risk - Navigate to risks tab with selected objective
const handleAddRisks = (objectiveId) => {
  selectedObjectiveId.value = objectiveId;
  setActiveTab("risks");
};

// Deleted

const handleAuditProcessDeleted = async (deletedId) => {
  if (selectedAuditProcessId.value === deletedId) {
    clearSelectionCascade("processes");
  }
  await getAuditProcessesByUserId();
};

const handleRiskControlMatrixDeleted = async (deletedId) => {
  if (selectedRiskControlMatrixId.value === deletedId) {
    clearSelectionCascade("matrix");
  }
  await getRiskControlMatricesByUserId();
};

const handleObjectiveDeleted = async (deletedId) => {
  if (selectedObjectiveId.value === deletedId) {
    clearSelectionCascade("objectives");
  }
  await getObjectivesByUserId();
};

const handleRiskDeleted = async () => {
  await getObjectivesByUserId();
};

const handleExitControlDeleted = async (deletedId) => {
  if (selectedExitControlId.value === deletedId) {
    clearSelectionCascade("exitcontrols");
  }
  await getExitControlsByUserId();
};

const handleControlDesignDeleted = async (deletedId) => {
  if (selectedControlDesignId.value === deletedId) {
    clearSelectionCascade("controldesigns");
  }
  await getExitControlsByUserId();
};

const handleAuditProgramDeleted = async (deletedId) => {
  if (selectedAuditProgramId.value === deletedId) {
    clearSelectionCascade("auditprograms");
  }
  await getAuditProgramsByUserId();
};

const handleObservationDeleted = async (deletedId) => {
  if (selectedObservationId.value === deletedId) {
    clearSelectionCascade("observations");
  }
  await getObservationsByUserId();
};

const handleRecommendationDeleted = async () => { await getObservationsByUserId() };

const handleRouteQueries = async () => {
  const {
    subDepartmentId,
    auditProcessId,
    riskControlMatrixId,
    objectiveId,
    exitControlId,
    controlDesignId,
    auditProgramId,
    observationId,
    tab,
  } = route.query;

  if (tab && tabs.value.some((t) => t.id === tab)) {
    activeTab.value = tab;
  }

  let dataAlreadyLoaded = false;

  if (subDepartmentId) {
    selectedSubDepartmentId.value = subDepartmentId;
    localStorage.setItem("organizationSelectedSubDepartmentId", subDepartmentId);
    await getSubDepartmentsByUserId();
    dataAlreadyLoaded = true;
  }

  if (auditProcessId) setSelectedId("processes", auditProcessId);
  if (riskControlMatrixId) setSelectedId("matrix", riskControlMatrixId);
  if (objectiveId) setSelectedId("objectives", objectiveId);
  if (exitControlId) setSelectedId("exitcontrols", exitControlId);
  if (controlDesignId) setSelectedId("controldesigns", controlDesignId);
  if (auditProgramId) setSelectedId("auditprograms", auditProgramId);
  if (observationId) setSelectedId("observations", observationId);

  if (Object.keys(route.query).length) {
    await router.replace({ path: "/risk-control" });
  }

  return dataAlreadyLoaded;
};

onMounted(async () => {
  const dataAlreadyLoaded = await handleRouteQueries();
  // Only fetch data if it wasn't already loaded from route queries
  if (!dataAlreadyLoaded) {
    await setActiveTab(activeTab.value);
  }
});
</script>