<template>
  <div class="relative flex min-h-screen">
    <div :class="['bg-secondary-base text-white space-y-2 px-1.5 transition-all duration-300', isCollapsed ? 'w-16' : 'w-55']">
      <SidebarHeader :is-collapsed="isCollapsed" />
      <SidebarNavigation :is-collapsed="isCollapsed" />
    </div>
    
    <!-- Main Content -->
    <div class="flex-1">
      <SidebarToggle 
        :is-collapsed="isCollapsed" 
        :toggle-sidebar="toggleSidebar" 
      />
    </div>
  </div>
</template>

<script setup>
import { useSidebar } from '@/composables/sideBar/useSidebar.js'
import SidebarHeader from './sidebar/SidebarHeader.vue'
import SidebarNavigation from './sidebar/SidebarNavigation.vue'
import SidebarToggle from './sidebar/SidebarToggle.vue'

const { isCollapsed, toggleSidebar } = useSidebar()
</script>