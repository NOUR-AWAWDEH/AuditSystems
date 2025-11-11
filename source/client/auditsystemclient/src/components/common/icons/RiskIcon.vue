<template>
  <!--RiskIcon.vue-->
   <svg 
    :class="[sizeClass, customClass]" 
    fill="none" 
    stroke="currentColor" 
    viewBox="0 0 24 24" 
    xmlns="http://www.w3.org/2000/svg"
  >
    <!-- Shield -->
    <path 
      stroke-linecap="round" 
      stroke-linejoin="round" 
      :stroke-width="strokeWidth" 
      d="M12 3l7 4v7c0 5-3.5 9-7 9s-7-4-7-9V7l7-4z"
    />

    <!-- Checkmark inside shield -->
    <path 
      stroke-linecap="round" 
      stroke-linejoin="round" 
      :stroke-width="strokeWidth" 
      d="M9 15l2 2 5-6"
    />

    <!-- Exclamation mark for alert/attention -->
    <path 
      stroke-linecap="round" 
      stroke-linejoin="round" 
      :stroke-width="strokeWidth" 
      d="M12 7v1M12 10v.01"
    />
  </svg>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  size: {
    type: String,
    default: 'md',
    validator: (value) => ['xs', 'sm', 'md', 'lg', 'xl', 'xxl', 'xxxl'].includes(value)
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
  return sizes[props.size] || sizes.md;
});
</script>