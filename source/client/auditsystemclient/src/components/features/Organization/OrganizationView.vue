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
        <LocationView
          ref="locationViewRef"
          @location-selected="onLocationSelected"
        />
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
import { ref, computed, watch, nextTick } from 'vue';

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

// Replace computed properties with reactive refs
const locations = ref([]);
const subLocations = ref([]);
const departments = ref([]);

// Watch for locationViewRef changes and update locations
watch(
  () => locationViewRef.value,
  async (newLocationViewRef) => {
    if (newLocationViewRef) {
      await nextTick();
      const checkLocations = () => {
        const currentLocations = newLocationViewRef.locations;
        if (currentLocations && currentLocations.length > 0) {
          locations.value = currentLocations;
        } else {
          setTimeout(checkLocations, 100);
        }
      };
      checkLocations();
    }
  },
  { immediate: true }
);

// Watch for direct changes to locationViewRef.locations
watch(
  () => locationViewRef.value?.locations,
  (newLocations) => {
    if (newLocations && newLocations.length > 0) {
      locations.value = newLocations;
    }
  },
  { immediate: true, deep: true }
);

// Watch for subLocationViewRef changes and update subLocations
watch(
  () => subLocationViewRef.value,
  async (newSubLocationViewRef) => {
    if (newSubLocationViewRef) {
      await nextTick();
      const checkSubLocations = () => {
        const currentSubLocations = newSubLocationViewRef.subLocations;
        if (currentSubLocations && currentSubLocations.length > 0) {
          subLocations.value = currentSubLocations;
        } else {
          setTimeout(checkSubLocations, 100);
        }
      };
      checkSubLocations();
    }
  },
  { immediate: true }
);

// Watch for direct changes to subLocationViewRef.subLocations
watch(
  () => subLocationViewRef.value?.subLocations,
  (newSubLocations) => {
    if (newSubLocations && newSubLocations.length > 0) {
      subLocations.value = newSubLocations;
    }
  },
  { immediate: true, deep: true }
);

// Watch for departmentViewRef changes and update departments
watch(
  () => departmentViewRef.value,
  async (newDepartmentViewRef) => {
    if (newDepartmentViewRef) {
      await nextTick();
      const checkDepartments = () => {
        const currentDepartments = newDepartmentViewRef.departments;
        if (currentDepartments && currentDepartments.length > 0) {
          departments.value = currentDepartments;
        } else {
          setTimeout(checkDepartments, 100);
        }
      };
      checkDepartments();
    }
  },
  { immediate: true }
);

// Watch for direct changes to departmentViewRef.departments
watch(
  () => departmentViewRef.value?.departments,
  (newDepartments) => {
    if (newDepartments && newDepartments.length > 0) {
      departments.value = newDepartments;
    }
  },
  { immediate: true, deep: true }
);

// Keep this computed property for effectiveSelectedSubLocationId
const effectiveSelectedSubLocationId = computed(() =>
  selectedSubLocationId.value || subLocationViewRef.value?.selectedLocationId
);

// Helper function to reset all lower components
function resetViews(...refs) {
  refs.forEach(r => r?.value?.resetToDefaultView?.());
}

// Event Handlers - Modified to NOT automatically change tabs
const onLocationSelected = (locationId) => {
  selectedLocationId.value = locationId;
  selectedSubLocationId.value = null;
  selectedDepartmentId.value = null;
  selectedSubDepartmentId.value = null;

  resetViews(subLocationViewRef, departmentViewRef, subDepartmentViewRef);

  if (locationId) {
    activeTab.value = 'sublocations';
  }
};

const onSubLocationSelected = (subLocationId) => {
  selectedSubLocationId.value = subLocationId;
  selectedDepartmentId.value = null;
  selectedSubDepartmentId.value = null;

  resetViews(departmentViewRef, subDepartmentViewRef);

  if(subLocationId) {
    activeTab.value = 'departments';
  }
};

const onDepartmentSelected = (departmentId) => {
  selectedDepartmentId.value = departmentId;
  selectedSubDepartmentId.value = null;

  resetViews(subDepartmentViewRef);
  if (departmentId) {
    activeTab.value = 'subdepartments';
  }
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
