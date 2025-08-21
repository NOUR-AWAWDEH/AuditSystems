<template>
<svg
  :class="[sizeClass, customClass]"
  fill="none"
  stroke="currentColor"
  viewBox="0 0 24 24"
>
  <!-- Outer ring -->
  <circle
    cx="12"
    cy="12"
    r="10"
    stroke-linecap="round"
    stroke-linejoin="round"
    :stroke-width="strokeWidth"
  />
  <!-- Middle ring -->
  <circle
    cx="12"
    cy="12"
    r="6.5"
    stroke-linecap="round"
    stroke-linejoin="round"
    :stroke-width="strokeWidth"
  />
  <!-- Inner ring -->
  <circle
    cx="12"
    cy="12"
    r="3"
    stroke-linecap="round"
    stroke-linejoin="round"
    :stroke-width="strokeWidth"
  />
  <!-- Arrow shaft -->
  <line
    x1="12"
    y1="12"
    x2="19"
    y2="5"
    stroke-linecap="round"
    :stroke-width="strokeWidth * 0.8"
  />
  <!-- Arrowhead (thin + sharp) -->
  <polygon
    points="19,5 17.8,5.6 18.4,4.2"
    fill="currentColor"
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
      ['xs', 'sm', 'md', 'lg', 'xl', 'xxl', 'xxxl'].includes(value),
  },
  strokeWidth: {
    type: [String, Number],
    default: 2,
  },
  customClass: {
    type: String,
    default: '',
  },
});

const sizeClass = computed(() => {
  const sizes = {
    xs: 'w-3 h-3',
    sm: 'w-4 h-4',
    md: 'w-5 h-5',
    lg: 'w-6 h-6',
    xl: 'w-8 h-8',
    xxl: 'w-10 h-10',
    xxxl: 'w-20 h-20',
  };
  return sizes[props.size];
});
</script>
