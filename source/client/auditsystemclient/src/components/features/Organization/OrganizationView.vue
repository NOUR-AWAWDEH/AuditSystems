<template>
    <!--OrganizationView.vue-->
  <div class="min-h-[calc(100vh-4rem)] bg-gray-50 p-2 sm:p-4 md:p-6">
    <!-- Header Section -->
    <div class="mb-4 sm:mb-6 md:mb-8">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between">
        <OrganizationHeader />
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
        <!-- Locations -->
        <div v-if="activeTab === 'locations'" class="p-3 sm:p-4 md:p-6">
          <LocationView
            @location-selected="handleLocationSelected"
          />
        </div>

        <!-- Departments -->
        <div v-if="activeTab === 'departments'" class="p-3 sm:p-4 md:p-6">
          <DepartmentView
            :locations="locations"
            :selected-location-id="selectedLocationId"
            @department-selected="handleDepartmentSelected"
          />
        </div>

        <!-- Sub-Departments -->
        <div v-if="activeTab === 'subdepartments'" class="p-3 sm:p-4 md:p-6">
          <SubDepartmentView
            :departments="departments"
            :selected-department-id="selectedDepartmentId"
            @sub-department-selected="handleSubDepartmentSelected"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, nextTick } from 'vue'; // Added nextTick
import { useRoute, useRouter } from 'vue-router';

import OrganizationHeader from './components/OrganizationHeader.vue';
import LocationView from './location/LocationView.vue';
import DepartmentView from './department/DepartmentView.vue';
import SubDepartmentView from './subdepartment/SubDepartmentView.vue';
import { useGetLocationById } from '@/composables/organization/location/useGetLocationById';
import { useGetDepartmentsByUserId } from '@/composables/organization/department/useGetDepartmentsByUserId';
import { useGetDepartmentById } from '@/composables/organization/department/useGetDepartmentById';
import { useGetSubDepartmentById } from '@/composables/organization/subdepartment/useGetSubDepartmentById';
import { useGetLocationsByUserId } from '@/composables/organization/location/useGetLocationsByUserId';
// Router
const route = useRoute();
const router = useRouter();

// State
const activeTab = ref('locations');

// localStorage get
const selectedLocationId = ref(localStorage.getItem('organizationSelectedLocationId') || null);
const selectedDepartmentId = ref(localStorage.getItem('organizationSelectedDepartmentId') || null);
const selectedSubDepartmentId = ref(localStorage.getItem('organizationSelectedSubDepartmentId') || null);

// Composables
const { getLocationsByUserId, locations } = useGetLocationsByUserId();
const { getLocationById } = useGetLocationById();

const { getDepartmentsByUserId, departments } = useGetDepartmentsByUserId();
const { getDepartmentById } = useGetDepartmentById();

const { getSubDepartmentById } = useGetSubDepartmentById();

const isLoading = ref(false);
const errorMessage = ref(null);

// Add ref for LocationView and a ref for locations data from LocationView
const locationViewRef = ref(null);
const organizationLocations = ref([]); 
// Tabs
const tabs = ref([
  { id: 'locations', name: 'Locations' },
  { id: 'departments', name: 'Departments' },
  { id: 'subdepartments', name: 'Sub-Departments' }
]);

// Selection handlers localStorage set
const setSelectedId = (key, value) => {
  switch (key) {
    case 'location': selectedLocationId.value = value; localStorage.setItem('organizationSelectedLocationId', value || ''); break;
    case 'department': selectedDepartmentId.value = value; localStorage.setItem('organizationSelectedDepartmentId', value || ''); break;
    case 'subDepartment': selectedSubDepartmentId.value = value; localStorage.setItem('organizationSelectedSubDepartmentId', value || ''); break;
  }
};

// Event handlers
const handleLocationSelected = async (id) => {
  setSelectedId('location', id);
  await getLocationsByUserId();
  activeTab.value = 'departments';
};

const handleDepartmentSelected = async (id) => {
  setSelectedId('department', id);
  await getDepartmentsByUserId();
  activeTab.value = 'subdepartments';
};

const handleSubDepartmentSelected = async (id) => {
  setSelectedId('subDepartment', id);
};

// Tab navigation - always refresh list when switching
const setActiveTab = async (tabId) => {
  activeTab.value = tabId;
  try {
    isLoading.value = true;
    errorMessage.value = null;

    switch (tabId) {
      case 'locations':
        if (locationViewRef.value) {
          await nextTick(); 
          if (typeof locationViewRef.value.refreshData === 'function') {
            await locationViewRef.value.refreshData();
            organizationLocations.value = locationViewRef.value.locations; 
          }
        }
        break;
      case 'departments':
        await getDepartmentsByUserId();
        break;
      case 'subdepartments':
        break;
    }
  } catch (err) {
    errorMessage.value = 'Failed to load data';
    console.error(err);
  } finally {
    isLoading.value = false;
  }
};

// Handle route queries
const handleRouteQueries = async () => {
  const { locationId, departmentId, subDepartmentId } = route.query;

  if (locationId) { selectedLocationId.value = locationId; await getLocationById(locationId); }
  if (departmentId) { selectedDepartmentId.value = departmentId; await getDepartmentById(departmentId); }
  if (subDepartmentId) {selectedSubDepartmentId.value= subDepartmentId; await getSubDepartmentById(subDepartmentId);}
  // Clean up URL
  if (locationId || departmentId || subDepartmentId) {
    await router.replace({ path: '/organization' });
  }
};

// Init
onMounted(async () => {
  await handleRouteQueries();
  await setActiveTab(activeTab.value);
});

watch(() => route.query, handleRouteQueries);
</script>

<style scoped>
.min-h-\[calc\(100vh-4rem\)\] {
  min-height: calc(100vh - 4rem);
  min-height: calc(100dvh - 4rem);
}
</style>