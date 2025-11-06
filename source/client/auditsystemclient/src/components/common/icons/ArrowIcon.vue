<template>
  <!--ArrowIcon.vue-->
  <div 
    class="w-8 h-8 bg-white rounded-full flex items-center justify-center shadow-sm">
    <svg 
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      stroke-width="2"
      stroke="currentColor"
      class="w-4 h-4 text-secondary-base flex-shrink-0">
      <path 
        v-if="direction === 'right'" 
        stroke-linecap="round" 
        stroke-linejoin="round" 
        d="m5.25 4.5 7.5 7.5-7.5 7.5m6-15 7.5 7.5-7.5 7.5" />
      <path 
        v-else stroke-linecap="round"
        stroke-linejoin="round" 
        d="m18.75 4.5-7.5 7.5 7.5 7.5m-6-15L5.25 12l7.5 7.5" />
    </svg>
  </div>
</template>

<script setup>
defineProps({
  direction: {
    type: String,
    default: 'left',
    validator: (value) => ['left', 'right'].includes(value)
  }
})
</script>