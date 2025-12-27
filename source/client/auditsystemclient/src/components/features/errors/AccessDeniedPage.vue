<template>
  <div class="min-h-screen bg-gray-50 flex items-center justify-center">
    <div class="max-w-md mx-auto text-center p-6 bg-white rounded-lg shadow">
      <div class="text-6xl mb-4">🚫</div>
      <h1 class="text-3xl font-bold text-gray-900 mb-2">Access Denied</h1>
      <p class="text-gray-600 mb-6">
        You don't have permission to access this page. Only users with the required role(s) can view this content.
      </p>
      
      <div class="bg-blue-50 border border-blue-200 rounded-md p-4 mb-6 text-left">
        <p class="text-sm text-blue-800">
          <strong>Your Role:</strong> {{ userRoles.join(', ') || 'No roles assigned' }}
        </p>
      </div>

      <div class="flex flex-col sm:flex-row gap-3">
        <button @click="goToActionOwner" v-if="hasActionOwnerRole" class="flex-1 py-2 px-4 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition">
          Go to Action Owner Dashboard
        </button>
          
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed} from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

const router = useRouter();
const { authStatus} = useAuthStatus();

const userRoles = computed(() => authStatus.value?.roles || []);
const hasActionOwnerRole = computed(() => userRoles.value.includes('ActionOwner'));

const goToActionOwner = () => {
  router.push('/action-owner');
};
</script>