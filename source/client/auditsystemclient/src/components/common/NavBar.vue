<template>
  <!--NavBar.vue-->
  <nav class="bg-secondary-base w-full fixed text-white text-xl border-b border-secondary-base">
    <div class="container flex justify-between items-center">

      <BaseLink to="/dashboard" variant="logo">
        <div class="h-16 flex items-center pl-2 py-3">
          <Logo variant="white" height="h-5" width="w-auto" />
        </div>
      </BaseLink>

      <div class="flex items-center space-x-6">
        <div v-if="isAuthenticated" class="relative">
          <button 
            @click="handleUserMenuClick" 
            class="focus:outline-none flex items-center cursor-pointer" 
            style="position: relative; left: -10px;"
          >
            <UserMenu ref="userMenuRef" />
          </button>
        </div>
        
        <BaseLink v-else to="/login" variant="primary">
          Login
        </BaseLink>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { useLogout } from '@/composables/auth/useLogout';
import { ref } from 'vue';
import BaseLink from '@/components/base/BaseLink.vue';
import Logo from './AuditLogo.vue';
import UserMenu from './UserMenu.vue';

const { isAuthenticated } = useLogout();
const userMenuRef = ref(null);

const handleUserMenuClick = () => {
  if (userMenuRef.value && userMenuRef.value.toggleDropdown) {
    userMenuRef.value.toggleDropdown();
  }
};
</script>

<style scoped>
nav {
  position: fixed; /* Fix the navbar at the top */
  top: 0; /* Align it to the top */
  width: 100%; /* Make it span the full width */
  z-index: 1000; /* Ensure it stays above other elements */
}
</style>
