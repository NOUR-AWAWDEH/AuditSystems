<template>
  <!-- CloudAuditLogsIcon.vue -->
  <svg
    :class="[sizeClass, customClass]"
    viewBox="0 0 24 24"
    xmlns="http://www.w3.org/2000/svg"
  >
    <path
      class="fill-[#4285f4]"
      d="M12.77,12.72a1.07,1.07,0,0,1-1.52,0L9.76,11.23l.76-.76L12,11.93l4.2-4.2.81.79Z"
    />
    <path
      class="fill-[#669df6]"
      d="M16,11.59a.91.91,0,0,0-.81.81,3.21,3.21,0,0,1-2.86,2.76,3.25,3.25,0,0,1-1.84-.36,3.19,3.19,0,0,1,1.2-6,3.07,3.07,0,0,1,1.21.13.9.9,0,0,0,1-.28h0a.91.91,0,0,0-.4-1.45A5,5,0,0,0,7,12.49,5,5,0,0,0,12.48,17,5,5,0,0,0,17,12.63a.91.91,0,0,0-1-1Z"
    />
    <path class="fill-[#aecbfa]" d="M12,18a2,2,0,1,1-2,2,2,2,0,0,1,2-2" />
    <path class="fill-[#aecbfa]" d="M12,2a2,2,0,1,1-2,2,2,2,0,0,1,2-2" />
    <path
      class="fill-[#4285f4]"
      d="M18,20a1,1,0,0,1-.71-1.7L20,15.59V8.41L17.31,5.7a1,1,0,0,1,0-1.41,1,1,0,0,1,1.42,0l3,3A1,1,0,0,1,22,8v8a1,1,0,0,1-.29.7l-3,3A1,1,0,0,1,18,20Z"
    />
    <path
      class="fill-[#669df6]"
      d="M6,20a1,1,0,0,1-.71-.3l-3-3A1,1,0,0,1,2,16V8a1,1,0,0,1,.29-.71l3-3a1,1,0,0,1,1.42,0,1,1,0,0,1,0,1.41L4,8.41v7.18L6.69,18.3a1,1,0,0,1,0,1.42A1,1,0,0,1,6,20Z"
    />
  </svg>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  size: {
    type: String,
    default: 'md',
    validator: (value) => ['xs', 'sm', 'md', 'lg', 'xl', 'xxl','xxxl'].includes(value)
  },
  customClass: {
    type: String,
    default: ''
  }
})

const sizeClass = computed(() => {
  const sizes = {
    xs: 'w-3 h-3',
    sm: 'w-4 h-4',
    md: 'w-5 h-5',
    lg: 'w-6 h-6',
    xl: 'w-8 h-8',
    xxl: 'w-10 h-10',
    xxxl: 'w-30 h-30'
  }
  return sizes[props.size]
})
</script>
