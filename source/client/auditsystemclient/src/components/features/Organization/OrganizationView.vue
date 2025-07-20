<template>
  <div class="min-h-[calc(100vh-4rem)] bg-gray-50 p-4 md:p-6">
    <!-- Header -->
    <OrganizationHeader />

    <!-- Tabs Navigation -->
    <div class="mb-6 border-b border-gray-200">
      <nav class="-mb-px flex space-x-8 overflow-x-auto">
        <button
          v-for="tab in tabs"
          :key="tab.id"
          @click="activeTab = tab.id"
          :class="[
            'whitespace-nowrap py-2 px-1 border-b-2 font-medium text-sm transition-colors duration-200',
            activeTab === tab.id
              ? 'border-primary-base text-primary-base'
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          {{ tab.name }}
        </button>
      </nav>
    </div>

    <!-- Tab Panels -->
    <div class="rounded-xl shadow-sm border border-gray-200">
      <!-- Locations -->
      <div v-if="activeTab === 'locations'" class="p-6 bg-white">
        <LocationView ref="locationViewRef" @location-selected="onLocationSelected" />
      </div>

      <!-- Sub-Locations -->
      <div v-if="activeTab === 'sublocations'" class="p-6 bg-white">
        <template v-if="!selectedLocationId">
          <div class="text-center py-8">
            <p class="text-gray-500 text-lg">Please select a location first from the Locations tab</p>
            <button
              @click="activeTab = 'locations'"
              class="mt-4 bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200"
            >
              Go to Locations
            </button>
          </div>
        </template>
        <template v-else>
          <SubLocationView
            ref="subLocationViewRef"
            :locations="locations"
            :selected-location-id="selectedLocationId"
            @sub-location-selected="onSubLocationSelected"
          />
        </template>
      </div>

      <!-- Departments -->
      <div v-if="activeTab === 'departments'" class="p-6 bg-white">
        <template v-if="!effectiveSelectedSubLocationId">
          <div class="text-center py-8">
            <p class="text-gray-500 text-lg">Please select a sub-location first from the Sub-Locations tab</p>
            <button
              @click="activeTab = 'sublocations'"
              class="mt-4 bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200"
            >
              Go to Sub-Locations
            </button>
          </div>
        </template>
        <template v-else>
          <DepartmentView
            ref="departmentViewRef"
            :sub-locations="subLocations"
            :selected-sub-location-id="effectiveSelectedSubLocationId"
            @department-selected="onDepartmentSelected"
          />
        </template>
      </div>

      <!-- Sub-Departments -->
      <div v-if="activeTab === 'subdepartments'" class="p-6 bg-white">
        <template v-if="!selectedDepartmentId">
          <div class="text-center py-8">
            <p class="text-gray-500 text-lg">Please select a department first from the Departments tab</p>
            <button
              @click="activeTab = 'departments'"
              class="mt-4 bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200"
            >
              Go to Departments
            </button>
          </div>
        </template>
        <template v-else>
          <SubDepartmentView
            ref="subDepartmentViewRef"
            :departments="departments"
            :selected-department-id="selectedDepartmentId"
            @sub-department-selected="onSubDepartmentSelected"
          />
        </template>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';

import OrganizationHeader from './components/OrganizationHeader.vue';
import LocationView from './location/LocationView.vue';
import SubLocationView from './sublocation/SubLocationView.vue';
import DepartmentView from './department/DepartmentView.vue';
import SubDepartmentView from './subdepartment/SubDepartmentView.vue';

// Tab State
const activeTab = ref('locations');
const tabs = [
  { id: 'locations', name: 'Locations' },
  { id: 'sublocations', name: 'Sub-Locations' },
  { id: 'departments', name: 'Departments' },
  { id: 'subdepartments', name: 'Sub-Departments' }
];

// View Refs
const locationViewRef = ref(null);
const subLocationViewRef = ref(null);
const departmentViewRef = ref(null);
const subDepartmentViewRef = ref(null);

// Hierarchical Selection State
const selectedLocationId = ref(null);
const selectedSubLocationId = ref(null);
const selectedDepartmentId = ref(null);
const selectedSubDepartmentId = ref(null);

// Computed Reactive Data
const locations = computed(() => locationViewRef.value?.locations || []);
const subLocations = computed(() => subLocationViewRef.value?.subLocations || []);
const departments = computed(() => departmentViewRef.value?.departments || []);
const effectiveSelectedSubLocationId = computed(() =>
  selectedSubLocationId.value || subLocationViewRef.value?.selectedLocationId
);

// Helper function to reset all lower components
function resetViews(...refs) {
  refs.forEach(r => r?.value?.resetToDefaultView?.());
}

// Event Handlers
const onLocationSelected = (locationId) => {
  console.log('Selected Location ID:', locationId); // Add this
  selectedLocationId.value = locationId;
  selectedSubLocationId.value = null;
  selectedDepartmentId.value = null;
  selectedSubDepartmentId.value = null;

  resetViews(subLocationViewRef, departmentViewRef, subDepartmentViewRef);
  activeTab.value = 'sublocations';
};

const onSubLocationSelected = (subLocationId) => {
  console.log('Selected SubLocation ID:', subLocationId); // Add this
  selectedSubLocationId.value = subLocationId;
  selectedDepartmentId.value = null;
  selectedSubDepartmentId.value = null;

  resetViews(departmentViewRef, subDepartmentViewRef);
  activeTab.value = 'departments';
};

const onDepartmentSelected = (departmentId) => {
  selectedDepartmentId.value = departmentId;
  selectedSubDepartmentId.value = null;

  resetViews(subDepartmentViewRef);
  activeTab.value = 'subdepartments';
};

const onSubDepartmentSelected = (subDepartmentId) => {
  selectedSubDepartmentId.value = subDepartmentId;
};
</script>

<style scoped>
.min-h-\[calc\(100vh-4rem\)\] {
  min-height: calc(100vh - 4rem);
  min-height: calc(100dvh - 4rem);
}
</style>
