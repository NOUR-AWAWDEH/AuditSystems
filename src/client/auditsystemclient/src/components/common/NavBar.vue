<script setup>
import { useLogout } from '@/composables/useLogout';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import BaseLink from '@/components/BaseLink.vue';

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
  <nav class="bg-secondary-base text-white py-4 fixed w-full z-10">
    <div class="container mx-auto flex justify-between items-center px-4">
      <!-- Logo or Brand -->
      <BaseLink to="/" variant="logo">Audit System</BaseLink>

      <!-- Navigation Links -->
      <div class="flex items-center space-x-6">
        <BaseLink to="/">Home</BaseLink>
        <BaseLink v-if="isAuthenticated" to="/dashboard">Dashboard</BaseLink>

        <!-- Logout Button -->
        <div v-if="isAuthenticated" class="flex items-center space-x-4">
          <span v-if="user?.username" class="text-sm">Welcome, {{ user.username }}</span>
          <BaseButton class="cursor-pointer" @click="handleLogout" :disabled="isLoggingOut">
            <template v-if="isLoggingOut" #loading>Logging out...</template>
            Logout
          </BaseButton>
        </div>
        <BaseLink v-else to="/login" variant="primary">
          Login
        </BaseLink>
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
  background-color: --color-secondary-base; /* Maintain the background color */
}
</style>
