<template>
  <!--MainLayout.vue-->
  <div class="flex flex-col h-full font-poppins">
    <!-- NavBar always on top -->
    <NavBar v-if="shouldShowNavBar" class="fixed z-50 top-0 left-0 right-0 h-16" />

    <div v-if="shouldShowSideBar" class="flex flex-1 pt-16 overflow-hidden">
      <!-- Sidebar -->
      <SideBar class="shrink-0" />

      <!-- Main content with its own scroll -->
      <main class="flex-1">
        <slot />
      </main>
    </div>

    <div v-else :class="{ 'pt-16': shouldShowNavBar }" class="flex-1 overflow-y-auto">
      <slot />
    </div>

    <BaseNotification/>
  </div>
</template>

<script setup>
import NavBar from "@/components/common/NavBar.vue";
import SideBar from "@/components/common/SideBar.vue";
import { useAppLayout } from "@/composables/layout/useAppLayout";
import BaseNotification from "../base/BaseNotification.vue";

const { shouldShowNavBar, shouldShowSideBar } = useAppLayout();
</script>
