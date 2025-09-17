<script setup>
import { computed } from 'vue';

const props = defineProps({
  width: {
    type: String,
    default: 'w-60'
  },
  height: {
    type: String,
    default: 'h-auto'
  },
  variant: {
    type: String,
    default: 'default' // 'default' or 'white'
  }
});

const logoSrc = computed(() => {
  if (props.variant === 'white') {
    return new URL('@/assets/icons/AuditSystem_White.svg', import.meta.url).href;
  }

  return new URL('@/assets/icons/AuditSystem.svg', import.meta.url).href;
});
</script>

<template>
  <img
    :src="logoSrc"
    :class="[width, height]"
    alt="Audit System Logo"
  />
</template>
