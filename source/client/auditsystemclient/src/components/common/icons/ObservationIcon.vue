<template>
  <!-- ObservationIcon.vue -->
  <svg
    :class="[sizeClass, customClass]"
    fill="none"
    stroke="currentColor"
    viewBox="0 0 64 64"
  >
    <!-- Eye outline -->
    <path
      stroke-linecap="round"
      stroke-linejoin="round"
      :stroke-width="strokeWidth"
      d="M4 32 C12 18, 28 14, 36 32 C28 50, 12 46, 4 32 Z"
    />
    <!-- Iris -->
    <circle
      cx="20"
      cy="32"
      r="6"
      fill="currentColor"
      stroke="none"
    />
    <!-- Magnifying glass -->
    <circle
      cx="46"
      cy="46"
      r="10"
      :stroke-width="strokeWidth"
    />
    <line
      x1="54"
      y1="54"
      x2="60"
      y2="60"
      :stroke-width="strokeWidth"
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
  return sizes[props.size];
});
</script>