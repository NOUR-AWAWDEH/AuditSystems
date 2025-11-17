<template>
  <!-- OrganizationView.vue -->
  <div
    class="min-h-[calc(100dvh-4rem)] bg-gray-50 p-2 sm:p-3 md:p-6 lg:p-8 xl:p-10 transition-all duration-300"
  >
    <!-- Header Section -->
    <header class="mb-3 sm:mb-5 md:mb-8">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-2 sm:gap-4">
        <OrganizationHeader />
      </div>
    </header>

    <!-- Navigation Tabs / Dropdown -->
    <div class="mb-4 sm:mb-6 border-b border-gray-200">
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
      <nav
        class="hidden md:flex space-x-2 sm:space-x-4 md:space-x-6 lg:space-x-8 overflow-x-auto scrollbar-hide"
      >
        <button
          v-for="tab in tabs"
          :key="tab.id"
          @click="setActiveTab(tab.id)"
          :class="[
            'whitespace-nowrap border-b-2 font-medium shrink-0 px-3 md:px-4 lg:px-6 py-2 text-sm md:text-base transition-colors duration-200',
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
        <!-- Locations -->
        <div v-if="activeTab === 'locations'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <LocationView
            @location-selected="handleLocationSelected"
            @location-deleted="handleLocationDeleted"
          />
        </div>

        <!-- Departments -->
        <div v-if="activeTab === 'departments'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <DepartmentView
            :locations="locations"
            :selected-location-id="selectedLocationId"
            @department-selected="handleDepartmentSelected"
            @department-deleted="handleDepartmentDeleted"
          />
        </div>

        <!-- Sub-Departments -->
        <div v-if="activeTab === 'subdepartments'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <SubDepartmentView
            :departments="departments"
            :selected-department-id="selectedDepartmentId"
            @sub-department-selected="handleSubDepartmentSelected"
            @sub-department-deleted="handleSubDepartmentDeleted"
          />
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, nextTick } from "vue";
import { useRoute, useRouter } from "vue-router";

import OrganizationHeader from "./components/OrganizationHeader.vue";
import LocationView from "./location/LocationView.vue";
import DepartmentView from "./department/DepartmentView.vue";
import SubDepartmentView from "./subdepartment/SubDepartmentView.vue";
import { useGetLocationById } from "@/composables/organization/location/useGetLocationById";
import { useGetDepartmentsByUserId } from "@/composables/organization/department/useGetDepartmentsByUserId";
import { useGetDepartmentById } from "@/composables/organization/department/useGetDepartmentById";
import { useGetSubDepartmentById } from "@/composables/organization/subdepartment/useGetSubDepartmentById";
import { useGetLocationsByUserId } from "@/composables/organization/location/useGetLocationsByUserId";

const route = useRoute();
const router = useRouter();

const activeTab = ref("locations");
const selectedLocationId = ref(localStorage.getItem("organizationSelectedLocationId") || null);
const selectedDepartmentId = ref(localStorage.getItem("organizationSelectedDepartmentId") || null);
const selectedSubDepartmentId = ref(localStorage.getItem("organizationSelectedSubDepartmentId") || null);

const { getLocationsByUserId, locations } = useGetLocationsByUserId();
const { getLocationById } = useGetLocationById();
const { getDepartmentsByUserId, departments } = useGetDepartmentsByUserId();
const { getDepartmentById } = useGetDepartmentById();
const { getSubDepartmentById } = useGetSubDepartmentById();

const isLoading = ref(false);
const errorMessage = ref(null);
const locationViewRef = ref(null);
const organizationLocations = ref([]);

const tabs = ref([
  { id: "locations", name: "Locations" },
  { id: "departments", name: "Departments" },
  { id: "subdepartments", name: "Sub-Departments" },
]);

const storageKeys = {
  locations: "organizationSelectedLocationId",
  departments: "organizationSelectedDepartmentId",
  subdepartments: "organizationSelectedSubDepartmentId"
}

const setSelectedId = (key, value) => {
  switch (key) {
    case "location":
      selectedLocationId.value = value;
      break;
    case "department":
      selectedDepartmentId.value = value;
      break;
    case "subDepartment":
      selectedSubDepartmentId.value = value;
      break;
    default:
      return;
  }

  const storageKey = storageKeys[key];
  if (!storageKey) return;

  if (value) {
    localStorage.setItem(storageKey, value);
  } else {
    localStorage.removeItem(storageKey)
  }
};

const handleLocationSelected = async (id) => {
  setSelectedId("location", id);
  await getLocationsByUserId();
  activeTab.value = "departments";
};

const handleDepartmentSelected = async (id) => {
  setSelectedId("department", id);
  await getDepartmentsByUserId();
  activeTab.value = "subdepartments";
};

const handleSubDepartmentSelected = async (id) => {
  setSelectedId("subDepartment", id);
};


// Handle Deleted SelectedId

const handleLocationDeleted = (deletedId) => {
  if (selectedLocationId.value === deletedId) {
    setSelectedId("location", null);
    setSelectedId("department", null);
    setSelectedId("subDepartment", null);
    selectedLocationId.value = null;
  }

  // remove the location from the parent list so downstream tabs stay in sync
  if (Array.isArray(locations.value)) {
    locations.value = locations.value.filter((loc) => loc.id !== deletedId);
  }
};

const handleDepartmentDeleted = (deletedId) => {
  if (selectedDepartmentId.value === deletedId) {
    setSelectedId("department", null);
    setSelectedId("subDepartment", null);
    selectedDepartmentId.value = null;
  }

  if (Array.isArray(departments.value)) {
    departments.value = departments.value.filter((dep) => dep.id !== deletedId);
  }
};

const handleSubDepartmentDeleted = (deletedId) => {
  if (selectedSubDepartmentId.value === deletedId) {
    setSelectedId("subDepartment", null);
    selectedSubDepartmentId.value = null;
  }
};

const setActiveTab = async (tabId) => {
  activeTab.value = tabId;
  isLoading.value = true;
  errorMessage.value = null;

  try {
    switch (tabId) {
      case "locations":
        if (locationViewRef.value) {
          await nextTick();
          if (typeof locationViewRef.value.refreshData === "function") {
            await locationViewRef.value.refreshData();
            organizationLocations.value = locationViewRef.value.locations;
          }
        }
        break;
      case "departments":
        await getDepartmentsByUserId();
        break;
      case "subdepartments":
        break;
    }
  } catch (err) {
    errorMessage.value = "Failed to load data";
    console.error(err);
  } finally {
    isLoading.value = false;
  }
};

const handleRouteQueries = async () => {
  const { locationId, departmentId, subDepartmentId } = route.query;
  if (locationId) {
    selectedLocationId.value = locationId;
    await getLocationById(locationId);
  }
  if (departmentId) {
    selectedDepartmentId.value = departmentId;
    await getDepartmentById(departmentId);
  }
  if (subDepartmentId) {
    selectedSubDepartmentId.value = subDepartmentId;
    await getSubDepartmentById(subDepartmentId);
  }
  if (locationId || departmentId || subDepartmentId) {
    await router.replace({ path: "/organization" });
  }
};

onMounted(async () => {
  await handleRouteQueries();
  await setActiveTab(activeTab.value);
});

watch(() => route.query, handleRouteQueries);
</script>

<style scoped>
.scrollbar-hide::-webkit-scrollbar {
  display: none;
}
.scrollbar-hide {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>
