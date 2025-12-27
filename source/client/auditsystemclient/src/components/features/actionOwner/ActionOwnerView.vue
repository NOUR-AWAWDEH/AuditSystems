<template>
  <div class="min-h-screen bg-gray-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Header -->
      <div class="mb-8">
        <h1 class="text-3xl font-bold text-gray-900">Action Owner Dashboard</h1>
        <p class="mt-2 text-gray-600">Manage and track audit program actions assigned to you</p>
      </div>

      <!-- User Info Card -->
      <div class="bg-white rounded-lg shadow mb-6 p-6">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <p class="text-sm text-gray-600">Username</p>
            <p class="text-lg font-semibold text-gray-900">{{ authStatus?.username || 'N/A' }}</p>
          </div>
          <div>
            <p class="text-sm text-gray-600">Email</p>
            <p class="text-lg font-semibold text-gray-900">{{ authStatus?.email || 'N/A' }}</p>
          </div>
          <div>
            <p class="text-sm text-gray-600">Role</p>
            <p class="text-lg font-semibold text-gray-900">{{ authStatus?.roles?.[0] || 'N/A' }}</p>
          </div>
          <div>
            <p class="text-sm text-gray-600">Company ID</p>
            <p class="text-lg font-semibold text-gray-900">{{ authStatus?.companyId || 'N/A' }}</p>
          </div>
        </div>
      </div>

      <!-- Placeholder for Action Owner Audit Programs -->
      <div class="bg-white rounded-lg shadow p-6">
        <h2 class="text-xl font-bold text-gray-900 mb-4">Your Assigned Audit Programs</h2>
        <p class="text-gray-600">This section will display audit programs assigned to you as an action owner.</p>
        <!-- TODO: Add audit program list component here -->
      </div>
    </div>
  </div>
</template>

<script setup>
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

const { authStatus } = useAuthStatus();
</script>

<style scoped>
/* Component-specific styles can be added here */
</style>
