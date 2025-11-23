<template>
  <svg
    :class="[sizeClass, customClass]"
    fill="none"
    stroke="currentColor"
    viewBox="0 0 24 24"
    xmlns="http://www.w3.org/2000/svg"
  >
    <!-- Top-Left Arrow -->
    <path
      :stroke-width="strokeWidth"
      stroke-linecap="round"
      stroke-linejoin="round"
      d="M4 9h5M5 8l4 4M4 9l4-4"
    />

    <!-- Top-Right Arrow -->
    <path
      :stroke-width="strokeWidth"
      stroke-linecap="round"
      stroke-linejoin="round"
      d="M20 9h-5M19 8l-4 4M20 9l-4-4"
    />

    <!-- Bottom-Left Arrow -->
    <path
      :stroke-width="strokeWidth"
      stroke-linecap="round"
      stroke-linejoin="round"
      d="M4 15h5M5 16l4-4M4 15l4 4"
    />

    <!-- Bottom-Right Arrow -->
    <path
      :stroke-width="strokeWidth"
      stroke-linecap="round"
      stroke-linejoin="round"
      d="M20 15h-5M19 16l-4-4M20 15l-4 4"
    />
  </svg>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  size: {
    type: String,
    default: 'md',
    validator: (value) =>
      ['xs', 'sm', 'md', 'lg', 'xl', 'xxl', 'xxxl'].includes(value)
  },
  strokeWidth: {
    type: [String, Number],
    default: 2
  },
  customClass: {
    type: String,
    default: ''
  }
});

const sizeClass = computed(() => {
  const sizes = {
    xs: 'w-3 h-3',
    sm: 'w-4 h-4',
    md: 'w-5 h-5',
    lg: 'w-6 h-6',
    xl: 'w-8 h-8',
    xxl: 'w-10 h-10',
    xxxl: 'w-20 h-20'
  };
  return sizes[props.size] || sizes.md;
});
</script>
