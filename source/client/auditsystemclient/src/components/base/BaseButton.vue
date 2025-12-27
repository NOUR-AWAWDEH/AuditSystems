<template>
  <!--BaseButton.vue-->
  <button
    @click="$emit('click')"
    class=" text-white font-medium   transition-colors duration-300 cursor-pointer"
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
