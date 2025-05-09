<script setup>
import { useLogout } from '@/composables/useLogout';
import { ref } from 'vue';
import { useRouter } from 'vue-router';

const { isAuthenticated, user, logout } = useLogout();
const router = useRouter();
const isLoggingOut = ref(false);

const handleLogout = async () => {
  isLoggingOut.value = true;
  try {
    const response = await logout();
    if (response.success) {
      router.push('/login');
    } else {
      console.error('Logout failed:', response.message);
    }
  } catch (err) {
    console.error('Logout error:', err);
  } finally {
    isLoggingOut.value = false;
  }
};
</script>

<template>
  <nav class="bg-secondary-base text-white p-2 fixed  w-full">
    <div class="container mx-auto flex justify-between items-center">
      <!-- Logo or Brand -->
      <div class="text-lg font-bold">
        <router-link to="/">Audit System</router-link>
      </div>

      <!-- Navigation Links -->
      <div class="space-x-4">
        <router-link to="/" class="hover:text-gray-300">Home</router-link>
        <router-link v-if="isAuthenticated" to="/dashboard" class="hover:text-gray-300">Dashboard</router-link>
      </div>

      <!-- Logout Button -->
      <div v-if="isAuthenticated" class="flex items-center space-x-4">
        <span v-if="user?.username" class="text-sm">Welcome, {{ user.username }}</span>
        <button
          @click="handleLogout"
          class="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded-lg transition"
          :disabled="isLoggingOut"
        >
          {{ isLoggingOut ? 'Logging out...' : 'Logout' }}
        </button>
      </div>
      <div v-else>
        <router-link to="/login" class="bg-primary-base hover:bg-primary-shade text-white px-4 py-2 rounded transition">
          Login
        </router-link>
      </div>
    </div>
  </nav>
</template>


<style scoped>
nav {
  position: fixed; /* Fix the navbar at the top */
  top: 0; /* Align it to the top */
  width: 100%; /* Make it span the full width */
  z-index: 1000; /* Ensure it stays above other elements */
  background-color: --color-secandary-base; /* Maintain the background color */
}
</style>
