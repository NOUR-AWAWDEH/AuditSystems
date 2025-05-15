<template>
  <button
    @click="$emit('click')"
    class=" bg-secondary-base text-white font-medium   transition-colors duration-300 cursor-pointer"
    :disabled="disabled"
  >
    <span v-if="isLoading"><slot name="loading">Loading...</slot></span>
    <span v-else><slot>Button</slot></span>
  </button>
</template>

<script>
export default {
  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
    isLoading: {
      type: Boolean,
      default: false,
    },
  },
  emits: ['click'],
};
</script>

<style scoped>
/* Add any component-specific styling here */
</style>
