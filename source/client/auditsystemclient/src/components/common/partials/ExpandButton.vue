<template>
  <button
    class="mr-2 p-1 hover:bg-gray-100 rounded transition-colors flex items-center expand-button"
    :class="{
      'bg-blue-50 text-blue-600': expanded,
      'text-gray-500': !expanded
    }"
    @click="$emit('click')"
  >
    <svg
      :class="['w-4 h-4 transition-transform', expanded ? 'rotate-90' : '']"
      fill="none"
      stroke="currentColor"
      viewBox="0 0 24 24"
    >
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
    </svg>
    <span class="ml-1 text-xs" v-if="childrenCount > 0">
      ({{ childrenCount }})
    </span>
  </button>
</template>

<script setup>
const props = defineProps({
  expanded: Boolean,
  childrenCount: {
    type: Number,
    default: 0
  }
});
</script>