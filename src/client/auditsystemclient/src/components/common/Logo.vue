<script setup>
defineProps({
  width: {
    type: String,
    default: 'w-60'
  },
  height: {
    type: String,
    default: 'h-auto'
  }
})
</script>

<template>
  <img 
    src="@/assets/icons/AuditSystem.svg" 
    :class="[width, height]" 
    alt="Audit System Logo" 
  />
</template>