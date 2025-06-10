<script setup>
import { ref } from 'vue';
import SideBar from '@/components/common/SideBar.vue';

const isSidebarOpen = ref(false);
</script>

<template>
  <div class="text-secondary-shade flex flex-col h-screen bg-gray-tint overflow-hidden">
    <div class="flex flex-1 overflow-hidden">
      <SideBar v-model:isOpen="isSidebarOpen" />

      <div class="flex-1 flex flex-col relative overflow-y-auto bg-white">
        <!-- Mobile toggle button -->
        <button
          @click="isSidebarOpen = !isSidebarOpen"
          class="fixed top-4 left-4 z-[50] p-2 rounded text-gray-700 bg-white shadow-md lg:hidden"
          aria-label="Toggle sidebar"
        >
          <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"/>
          </svg>
        </button>

        <main class="flex-1">
          <!-- Content will be injected here by child routes -->
        </main>
      </div>
    </div>
  </div>
</template>

<style scoped>
:global(html), :global(body), :global(#app) {
  height: 100%;
  margin: 0;
}
</style>
