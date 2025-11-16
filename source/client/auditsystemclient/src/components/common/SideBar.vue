<template>
  <div class="relative flex min-h-screen bg-gray-50">
    <!-- Sidebar -->
    <aside
      :class="[
        'bg-linear-to-b from-secondary-base to-tertiary-base text-white fixed top-16 left-0 h-[calc(100vh-4rem)] transition-all duration-300 z-50',
        isCollapsed
          ? 'w-0 sm:w-14 md:w-16 px-0 sm:px-1'
          : 'w-40 sm:w-48 md:w-56 lg:w-64 px-2 sm:px-3',
      ]"
    >
      <div class="py-2 sm:py-3 space-y-1 sm:space-y-2 h-full flex flex-col">
        <!-- Toggle Button - removed custom margin, added consistent padding -->
        <div class="flex justify-center px-2 sm:px-3">
          <SidebarToggle :is-collapsed="isCollapsed" :toggle-sidebar="toggleSidebar" />
        </div>

        <!-- Navigation Items -->
        <div class="flex-1 overflow-y-auto">
          <SidebarNavigation :is-collapsed="isCollapsed" />
        </div>
      </div>
    </aside>

    <!-- Main Content -->
    <div
      :class="[
        'flex-1 min-h-screen transition-all duration-300',
        isCollapsed ? 'ml-12 sm:ml-14 md:ml-16' : 'ml-40 sm:ml-48 md:ml-56 lg:ml-64',
      ]"
    ></div>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, computed, watch } from "vue";
import { useSidebar } from "@/composables/sideBar/useSidebar.js";
import SidebarNavigation from "./sidebar/SidebarNavigation.vue";
import SidebarToggle from "./sidebar/SidebarToggle.vue";

const { isCollapsed, toggleSidebar } = useSidebar();

// Responsive auto-collapse
const windowWidth = ref(window.innerWidth);
const isMobile = computed(() => windowWidth.value < 768);

// Auto-collapse when entering mobile view
watch(isMobile, (mobile) => {
  if (mobile) {
    isCollapsed.value = true;
  }
});

const handleResize = () => {
  windowWidth.value = window.innerWidth;
};

onMounted(() => {
  // Initial check
  if (isMobile.value) {
    isCollapsed.value = true;
  }
  window.addEventListener("resize", handleResize);
});

onUnmounted(() => {
  window.removeEventListener("resize", handleResize);
});
</script>

<style scoped>
/* Ensure full height and correct offset from navbar */
.h-\[calc\(100vh-4rem\)\] {
  height: calc(100vh - 4rem);
  height: calc(100dvh - 4rem);
}
</style>
