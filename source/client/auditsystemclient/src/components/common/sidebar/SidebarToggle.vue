<template>
  <BaseButton class="h-10 w-10 rounded-r-2xl mt-4" @click="toggleSidebar">
    <ArrowIcon
      :direction="isCollapsed ? 'right' : 'left'"
      bgClass="bg-white shadow"
      custom-class="text-secondary-base"
    />

  </BaseButton>
</template>

<script setup>
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
</script>

<style scoped>
</style>
