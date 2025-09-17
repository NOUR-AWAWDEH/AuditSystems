<template>
  <BaseButton
    class="h-9 w-7 rounded-r-2xl mt-3.25"
    @click="toggleSidebar"
  >
    <ArrowIcon :direction="isCollapsed ? 'right' : 'left'" />
  </BaseButton>
</template>

<script setup>
import BaseButton from '@/components/base/BaseButton.vue'
import ArrowIcon from '@/components/common/icons/ArrowIcon.vue'

defineProps({
  isCollapsed: {
    type: Boolean,
    default: false
  },
  toggleSidebar: {
    type: Function,
    required: true
  }
})
</script>