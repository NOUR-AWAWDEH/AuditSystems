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
