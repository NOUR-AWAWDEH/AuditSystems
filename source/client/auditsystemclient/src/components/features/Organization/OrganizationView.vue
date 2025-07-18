<template>
    <div class="flex min-h-[calc(100vh-4rem)] bg-gray-50">
        <!-- Main Content with full height -->
        <div class="flex-1 min-h-[calc(100vh-4rem)] p-3 sm:p-4 md:p-6">
            <!-- Organization Header -->
            <OrganizationHeader />
            
            <!-- Location Management Section -->
            <LocationView 
                ref="locationViewRef"
                @location-selected="handleLocationSelected"
            />
            
            <!-- Sublocation Management Section -->
            <div v-if="selectedLocationId" class="mt-8">
                <SubLocationView 
                    ref="subLocationViewRef" 
                    :locations="locations"
                    :selected-location-id="selectedLocationId"
                    @sub-location-selected="handleSubLocationSelected"
                />
            </div>
            
            <!-- Department Management Section -->
            <div v-if="effectiveSelectedSubLocationId" class="mt-8">
                <DepartmentView 
                    ref="departmentViewRef" 
                    :sub-locations="subLocations"
                    :selected-sub-location-id="effectiveSelectedSubLocationId"
                    @department-selected="handleDepartmentSelected"
                />
            </div>
            
            <!-- SubDepartment Management Section -->
            <div v-if="selectedDepartmentId" class="mt-8">
                <SubDepartmentView 
                    ref="subDepartmentViewRef" 
                    :departments="departments"
                    @selected-department-id="handleDepartmentSelected"
                    @sub-department-selected="handleSubDepartmentSelected"
                />
            </div>
        </div>
    </div>
</template>

<script setup>
import OrganizationHeader from './components/OrganizationHeader.vue';
import LocationView from './location/LocationView.vue';
import SubLocationView from './sublocation/SubLocationView.vue';
import DepartmentView from './department/DepartmentView.vue';
import SubDepartmentView from './subdepartment/SubDepartmentView.vue';
import { ref, computed } from 'vue';

// Component refs
const locationViewRef = ref(null);
const subLocationViewRef = ref(null);
const departmentViewRef = ref(null);
const subDepartmentViewRef = ref(null);

// State for hierarchical selection
const selectedLocationId = ref(null);
const selectedSubLocationId = ref(null);
const selectedDepartmentId = ref(null);
const selectedSubDepartmentId = ref(null);

// Get locations from LocationView
const locations = computed(() => {
    return locationViewRef.value?.locations || [];
});

// Get sublocations from SubLocationView
const subLocations = computed(() => {
    return subLocationViewRef.value?.subLocations || [];
});

// Get departments from DepartmentView
const departments = computed(() => {
    return departmentViewRef.value?.departments || [];
});

// Get the actual selected sublocation ID from SubLocationView
const actualSelectedSubLocationId = computed(() => {
    return subLocationViewRef.value?.selectedSubLocationId || null;
});

// Use the actual selected sublocation ID for DepartmentView
const effectiveSelectedSubLocationId = computed(() => {
    return actualSelectedSubLocationId.value || selectedSubLocationId.value;
});

// Handle location selection
const handleLocationSelected = (locationId) => {
    selectedLocationId.value = locationId;
    selectedSubLocationId.value = null;
    selectedDepartmentId.value = null;
    selectedSubDepartmentId.value = null;
    
    // Reset child views
    if (subLocationViewRef.value?.resetToDefaultView) {
        subLocationViewRef.value.resetToDefaultView();
    }
    if (departmentViewRef.value?.resetToDefaultView) {
        departmentViewRef.value.resetToDefaultView();
    }
    if (subDepartmentViewRef.value?.resetToDefaultView) {
        subDepartmentViewRef.value.resetToDefaultView();
    }
};

// Handle sublocation selection
const handleSubLocationSelected = (subLocationId) => {
    selectedSubLocationId.value = subLocationId;
    selectedDepartmentId.value = null; // Reset department selection
    selectedSubDepartmentId.value = null; // Reset subdepartment selection
    
    if (departmentViewRef.value?.resetToDefaultView) {
        departmentViewRef.value.resetToDefaultView();
    }
    if (subDepartmentViewRef.value?.resetToDefaultView) {
        subDepartmentViewRef.value.resetToDefaultView();
    }
};

// Handle department selection
const handleDepartmentSelected = (departmentId) => {
    selectedDepartmentId.value = departmentId;
    selectedSubDepartmentId.value = null; // Reset subdepartment selection
    
    if (subDepartmentViewRef.value?.resetToDefaultView) {
        subDepartmentViewRef.value.resetToDefaultView();
    }
};

// Handle subdepartment selection
const handleSubDepartmentSelected = (subdepartmentId) => {
    selectedSubDepartmentId.value = subdepartmentId;
};
</script>

<style scoped>
.min-h-\[calc\(100vh-4rem\)\] {
    min-height: calc(100vh - 4rem);
    min-height: calc(100dvh - 4rem);
}

@media (max-width: 640px) {
    .p-3 {
        padding: 0.75rem;
    }
    
    .space-y-6 > * + * {
        margin-top: 1rem;
    }
    
    .mt-8 {
        margin-top: 1.5rem;
    }
}

@media (max-width: 480px) {
    .p-3 {
        padding: 0.5rem;
    }
    
    .space-y-6 > * + * {
        margin-top: 0.75rem;
    }
    
    .mt-8 {
        margin-top: 1rem;
    }
}
</style>