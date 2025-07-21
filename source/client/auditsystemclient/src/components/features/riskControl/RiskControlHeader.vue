<template>
  <div>
    <h1 class="text-3xl md:text-4xl font-bold text-secondary-base mb-2">
      Risk & Control Management
    </h1>
    <p class="text-gray-600 text-lg">
      Manage processes, risks, controls, and observations
    </p>
    <!-- Selected SubDepartment Info -->
    <div v-if="selectedSubDepartment" class="mt-4 p-3 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
      <div class="flex items-center">
        <LocationIcon size="md" custom-class="text-blue-600 mr-2" />
        <p class="text-sm text-gray-700">
          The Selected SubDepartment : &nbsp;&nbsp;
          <span class="font-semibold text-blue-800">
            <span v-if="isLoadingSubDepartment">Loading...</span>
            <span v-else>{{ selectedSubDepartment?.name || 'Unknown SubDepartment' }}</span>
          </span>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import LocationIcon from '@/components/common/icons/LocationIcon.vue';

defineProps({
  selectedSubDepartment: {
    type: Object,
    default: null
  },
  isLoadingSubDepartment: {
    type: Boolean,
    default: false
  }
});
</script>