<template>
  <div class="flex flex-col min-h-screen font-poppins">
    <!-- NavBar always on top -->
    <NavBar v-if="shouldShowNavBar" class="z-50 fixed top-0 left-0 right-0" />

    <div v-if="shouldShowSideBar" class="flex flex-1 pt-16">
      <!-- Sidebar -->
      <SideBar />

      <!-- Main content -->
      <main class="flex-1 overflow-x-hidden overflow-y-auto">
        <slot />
      </main>
    </div>

    <div v-else :class="{ 'pt-16': shouldShowNavBar }">
      <slot />
    </div>

    <BaseNotification />
  </div>
</template>

<script setup>
import BaseNotification from '@/components/base/BaseNotification.vue'
import NavBar from '@/components/common/NavBar.vue'
import SideBar from '@/components/common/SideBar.vue'
import { useAppLayout } from '@/composables/layout/useAppLayout'

const { shouldShowNavBar, shouldShowSideBar } = useAppLayout()
</script>
