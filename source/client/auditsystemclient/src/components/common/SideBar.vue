<template>
  <div class="relative flex min-h-screen">
    <!-- Mobile Overlay Background -->
    <div 
      v-if="!isCollapsed && isMobile" 
      class="fixed inset-0 bg-white bg-opacity-10 z-40 md:hidden"
      @click="toggleSidebar"
    ></div>
    
    <!-- Sidebar - Always Fixed Full Height but positioned under NavBar -->
    <div :class="[
      'bg-secondary-base text-white transition-all duration-300 z-50',
      'fixed left-0 top-16 h-[calc(100vh-4rem)]',
      isCollapsed 
        ? 'w-12 sm:w-14 md:w-16 px-1' 
        : isMobile 
          ? 'w-64 px-3 shadow-xl'
          : 'w-40 sm:w-48 md:w-56 lg:w-64 px-2 sm:px-3'
    ]">
      <div class="py-2 sm:py-3 space-y-1 sm:space-y-2 h-full flex flex-col">
        <!-- Move toggle button to the top of sidebar -->
        <div class="mb-2 flex justify-center">
          <SidebarToggle 
            :is-collapsed="isCollapsed" 
            :toggle-sidebar="toggleSidebar" 
          />
        </div>
        <div class="flex-1 overflow-y-auto">
          <SidebarNavigation :is-collapsed="isCollapsed" />
        </div>
      </div>
    </div>
    
    <!-- Main Content -->
    <div :class="[
      'flex-1 min-h-screen transition-all duration-300',
      isCollapsed 
        ? 'ml-12 sm:ml-14 md:ml-16' 
        : isMobile 
          ? 'ml-0'
          : 'ml-40 sm:ml-48 md:ml-56 lg:ml-64'
    ]">
      <!-- Content goes here -->
    </div>
  </div>
</template>

<script setup>
import { computed, onMounted, onUnmounted, ref } from 'vue'
import { useSidebar } from '@/composables/sideBar/useSidebar.js'
import SidebarNavigation from './sidebar/SidebarNavigation.vue'
import SidebarToggle from './sidebar/SidebarToggle.vue'

const { isCollapsed, toggleSidebar } = useSidebar()

// Mobile detection
const windowWidth = ref(window.innerWidth)
const isMobile = computed(() => windowWidth.value < 768)

// Handle window resize
const handleResize = () => {
  windowWidth.value = window.innerWidth
}

onMounted(() => {
  window.addEventListener('resize', handleResize)
})

onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
})
</script>

<style scoped>
/* Ensure sidebar is always fixed and positioned under navbar */
.fixed {
  position: fixed !important;
}

/* Ensure sidebar takes full height minus navbar height */
.h-\[calc\(100vh-4rem\)\] {
  height: calc(100vh - 4rem);
  height: calc(100dvh - 4rem); /* Dynamic viewport height for better mobile support */
}

.h-full {
  height: 100%;
}

/* Remove the relative positioning override for desktop */
/* This ensures the sidebar stays fixed even on desktop */

@media (max-width: 640px) {
  .w-12 {
    min-width: 3rem;
  }
  .w-40 {
    min-width: 9rem;
    max-width: 10rem;
  }
}

@media (max-width: 480px) {
  .w-12 {
    min-width: 2.5rem;
  }
}

@media (max-width: 360px) {
  .w-12 {
    min-width: 2rem;
  }
}
</style>