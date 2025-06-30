<template>
  <div><h2>Add Sublocation</h2></div>
</template>

<script setup>
// Add proper props and functionality here
defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  locationId: {
    type: [String, Number],
    default: null
  }
});

defineEmits(['close', 'sublocation-added']);
</script>

<style scoped>
</style>