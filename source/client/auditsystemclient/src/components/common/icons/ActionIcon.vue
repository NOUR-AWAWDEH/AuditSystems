<template>
  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor">
    <path stroke-linecap="round" stroke-linejoin="round" :d="iconPath" />
  </svg>
</template>

<script setup>
defineProps({
  iconPath: {
    type: String,
    required: true
  }
});
</script>