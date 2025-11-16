<template>
  <BaseButton class="h-10 w-10 rounded-r-2xl mt-4" @click="toggleSidebar">
    <ArrowIcon
      :direction="isCollapsed ? 'right' : 'left'"
      :size="isCollapsed && isMobileView ? 'xl' : 'md'"
      bgClass="bg-white shadow"
      custom-class="text-secondary-base"
    />

  </BaseButton>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from "vue";
import BaseButton from "@/components/base/BaseButton.vue";
import ArrowIcon from "@/components/common/icons/ArrowIcon.vue";

defineProps({
  isCollapsed: {
    type: Boolean,
    default: false,
  },
  toggleSidebar: {
    type: Function,
    required: true,
  },
});

// Reactive mobile state
const isMobileView = ref(false);
let mediaQuery;
let handler;

onMounted(() => {
  mediaQuery = window.matchMedia("(max-width: 640px)");

  // Initial value
  isMobileView.value = mediaQuery.matches;

  // Handler to update state
  handler = (e) => {
    isMobileView.value = e.matches;
  };

  mediaQuery.addEventListener("change", handler);
});

onUnmounted(() => {
  if (mediaQuery && handler) {
    mediaQuery.removeEventListener("change", handler);
  }
});
</script>

<style scoped>
</style>
