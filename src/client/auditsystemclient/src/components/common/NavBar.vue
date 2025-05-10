<script setup>
import { useLogout } from '@/composables/useLogout';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import BaseLink from '@/components/BaseLink.vue';
import Logo from './AuditLogo.vue';

const { isAuthenticated, logout } = useLogout();
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
  <nav class="bg-white w-full fixed text-secondary-shade ">
    <div class="container flex justify-between items-center">
      <!-- Logo or Brand -->
      <BaseLink to="/" variant="logo">
        <div class="w-20 px-2 py-2">
          <Logo/>
        </div>
      </BaseLink>

      <!-- Navigation Links -->
      <div class="flex items-center space-x-6">
        <BaseLink to="/dashboard">Home</BaseLink>
        <!-- Logout Button -->
        <div v-if="isAuthenticated" class="flex items-center space-x-4">
          <BaseButton class="cursor-pointer pr-5 " @click="handleLogout" :disabled="isLoggingOut">
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
}
</style>
