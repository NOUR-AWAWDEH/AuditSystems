<template>
<div class="relative flex min-h-screen">
  <!-- Sidebar with dynamic width based on collapsed state -->
  <div :class="[
    'bg-secondary-base text-white space-y-2 px-1.5 transition-all duration-300',
    isCollapsed ? 'w-16' : 'w-55'
  ]">
    <a href="#" class="flex justify-center">
      <img src="@/assets/icons/AuditSystem_White.svg" alt="Logo" class="h-15 w-50 pr-5" :class="{ 'hidden': isCollapsed }">
      <!-- You might want to add a smaller logo for collapsed state -->
    </a>
    <nav>
      <!-- Here We Add The Pages -->
      <a href="/" class="flex items-center py-2 px-5 hover:bg-secondary-shade rounded">
        <!--Icon - fixed size regardless of sidebar state-->
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-7 flex-shrink-0">
          <path stroke-linecap="round" stroke-linejoin="round" d="m2.25 12 8.954-8.955c.44-.439 1.152-.439 1.591 0L21.75 12M4.5 9.75v10.125c0 .621.504 1.125 1.125 1.125H9.75v-4.875c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125V21h4.125c.621 0 1.125-.504 1.125-1.125V9.75M8.25 21h8.25" />
        </svg>
        <!--Text - hidden when collapsed-->
        <span class="text-white pl-2" :class="{ 'hidden': isCollapsed }">Dashboard</span>
      </a>
      <!-- Add more menu items with the same pattern -->

      <a class="flex items-center py-2 px-3 hover:bg-secondary-shade rounded">
        <UserMenu />
        <span class="text-white pl-2" :class="{ 'hidden': isCollapsed }">User Menu </span>
      </a>
    </nav>
  </div>

  <!-- Main Content -->
  <div class="flex-1">
    <!-- header -->
    <div class="bg-white shadow px-2 py-4">Header</div>

    <!-- Toggle button with dynamic arrow direction -->
    <BaseButton
      class="h-9 w-7 rounded-r-2xl mt-3.25"
      @click="toggleSidebar"
    >
      <!-- Arrow icon that changes based on sidebar state - fixed size -->
      <svg v-if="isCollapsed" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6 flex-shrink-0">
        <path stroke-linecap="round" stroke-linejoin="round" d="m5.25 4.5 7.5 7.5-7.5 7.5m6-15 7.5 7.5-7.5 7.5" />
      </svg>

      <!-- Original left-pointing arrow when sidebar is expanded - fixed size -->
      <svg v-else xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6 flex-shrink-0">
        <path stroke-linecap="round" stroke-linejoin="round" d="m18.75 4.5-7.5 7.5 7.5 7.5m-6-15L5.25 12l7.5 7.5" />
      </svg>
    </BaseButton>

    <!-- content -->
    <div class="p-8 text-primary-shade">Content</div>
  </div>
</div>
</template>

<script setup>
import { ref } from 'vue';
import BaseButton from '@/components/base/BaseButton.vue';
import UserMenu from './UserMenu.vue';

// State to track if sidebar is collapsed
const isCollapsed = ref(false);

// Function to toggle sidebar state
const toggleSidebar = () => {
  isCollapsed.value = !isCollapsed.value;
};
</script>
