<template>
  <!--LocationIcon.vue-->
  <svg 
    :class="[sizeClass, customClass]" 
    fill="none" 
    stroke="currentColor" 
    viewBox="0 0 24 24"
  >
    <path 
      stroke-linecap="round" 
      stroke-linejoin="round" 
      :stroke-width="strokeWidth" 
      d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"
    />
    <path 
      stroke-linecap="round" 
      stroke-linejoin="round" 
      :stroke-width="strokeWidth" 
      d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"
    />
  </svg>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  size: {
    type: String,
    default: 'md',
    validator: (value) => ['xs', 'sm', 'md', 'lg', 'xl','xxl', 'xxxl'].includes(value)
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
    xxxl: 'w-20 h-20',
  };
  return sizes[props.size];
});
</script>