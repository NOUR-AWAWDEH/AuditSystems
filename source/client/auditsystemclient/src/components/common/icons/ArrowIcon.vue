<template>
  <!--ArrowIcon.vue-->
  <div
    :class="[
      'flex items-center justify-center rounded-full',
      bgClass,
      sizeClassWrapper,
      customWrapperClass
    ]"
  >
    <svg
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      stroke="currentColor"
      :stroke-width="strokeWidth"
      :class="[iconSizeClass, customClass]"
    >
      <!-- RIGHT -->
      <path
        v-if="direction === 'right'"
        stroke-linecap="round"
        stroke-linejoin="round"
        d="m5.25 4.5 7.5 7.5-7.5 7.5m6-15 7.5 7.5-7.5 7.5"
      />
      <!-- LEFT -->
      <path
        v-else
        stroke-linecap="round"
        stroke-linejoin="round"
        d="m18.75 4.5-7.5 7.5 7.5 7.5m-6-15L5.25 12l7.5 7.5"
      />
    </svg>
  </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  direction: {
    type: String,
    default: 'left',
    validator: (value) => ['left', 'right'].includes(value)
  },

  size: {
    type: String,
    default: 'md',
    validator: (value) => ['xs','sm','md','lg','xl','xxl','xxxl'].includes(value)
  },

  strokeWidth: {
    type: [String, Number],
    default: 2
  },

  customClass: {
    type: String,
    default: ''
  },

  customWrapperClass: {
    type: String,
    default: ''
  },

  bgClass: {
    type: String,
    default: 'bg-white'
  }
});

/* ICON SIZE (svg) */
const iconSizeClass = computed(() => {
  const sizes = {
    xs: 'w-2 h-2',
    sm: 'w-3 h-3',
    md: 'w-4 h-4',
    lg: 'w-5 h-5',
    xl: 'w-6 h-6',
    xxl: 'w-8 h-8',
    xxxl: 'w-10 h-10',
  };
  return sizes[props.size];
});

/* WRAPPER SIZE (outer circle) */
const sizeClassWrapper = computed(() => {
  const sizes = {
    xs: 'w-5 h-5',
    sm: 'w-7 h-7',
    md: 'w-8 h-8',
    lg: 'w-10 h-10',
    xl: 'w-12 h-12',
    xxl: 'w-14 h-14',
    xxxl: 'w-20 h-20',
  };
  return sizes[props.size];
});
</script>
