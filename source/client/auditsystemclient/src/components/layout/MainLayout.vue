<template>
  <!--MainLayout.vue-->
  <div class="flex flex-col min-h-screen font-poppins">
    <!-- Main layout with SideBar and content -->
    <div v-if="shouldShowSideBar" class="flex flex-1" :class="{ 'pt-16': shouldShowNavBar }">
      <!-- SideBar - shown only for authenticated users -->
      <NavBar/>
      <SideBar />
      
      <!-- Main content area -->
      <div class="flex-1">
        <slot />
      </div>
    </div>
    
    <!-- Content without SideBar for auth pages -->
    <div v-else :class="{ 'pt-16': shouldShowNavBar }">
      <slot />
    </div>
    
    <BaseNotification />
  </div>
</template>

<script setup>
import BaseNotification from '@/components/base/BaseNotification.vue';
import NavBar from '@/components/common/NavBar.vue';
import SideBar from '@/components/common/SideBar.vue';
import { useAppLayout } from '@/composables/layout/useAppLayout';

const {
  shouldShowNavBar,
  shouldShowSideBar
} = useAppLayout();
</script>