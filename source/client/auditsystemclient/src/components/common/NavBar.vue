<script setup>
import { useLogout } from '@/composables/auth/useLogout';
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import BaseLink from '@/components/base/BaseLink.vue';
import Logo from './AuditLogo.vue'; // Make sure this path is correct

const { isAuthenticated, logout } = useLogout();
const router = useRouter();
const isLoggingOut = ref(false);
const isDropdownOpen = ref(false); // Added for dropdown visibility

const toggleDropdown = () => {
  isDropdownOpen.value = !isDropdownOpen.value;
};

const handleLogout = async () => {
  isLoggingOut.value = true;
  isDropdownOpen.value = false; // Close dropdown on logout attempt
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
  <nav class="bg-secondary-shade w-full fixed text-white text-xl">
    <div class="container flex justify-between items-center">

      <BaseLink to="/" variant="logo">

        <div class="h-16 flex items-center pl-2 py-3">
          <Logo variant="white" height="h-full" width="w-auto" />
        </div>
      </BaseLink>


      <div class="flex items-center space-x-6">
        <BaseLink to="/dashboard" class="text-white hover:text-white">Home</BaseLink>

        <div v-if="isAuthenticated" class="relative">
          <button @click="toggleDropdown" class="focus:outline-none flex items-center cursor-pointer" style="position: relative; left: -10px;">

            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="w-8 h-8 text-white">
              <path fill-rule="evenodd" d="M18.685 19.097A9.723 9.723 0 0021.75 12c0-5.385-4.365-9.75-9.75-9.75S2.25 6.615 2.25 12a9.723 9.723 0 003.065 7.097A9.716 9.716 0 0012 21.75a9.716 9.716 0 006.685-2.653zm-12.54-1.285A7.486 7.486 0 0112 15a7.486 7.486 0 015.855 2.812A8.224 8.224 0 0112 20.25a8.224 8.224 0 01-5.855-2.438zM15.75 9a3.75 3.75 0 11-7.5 0 3.75 3.75 0 017.5 0z" clip-rule="evenodd" />
            </svg>
          </button>

          <div v-if="isDropdownOpen" class="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 z-20">
            <BaseLink
              to="#"
              class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 w-full text-left"
              @click.prevent="handleLogout"
              :disabled="isLoggingOut"
            >
              <template v-if="isLoggingOut">Logging out...</template>
              <template v-else>Logout</template>
            </BaseLink>
          </div>
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
