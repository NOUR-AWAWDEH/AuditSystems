<template>
  <router-link 
    :to="route" 
    class="flex items-center py-2 px-5 hover:bg-secondary-shade rounded"
  >
    <component :is="icon" class="size-7 flex-shrink-0" />
    <span class="text-white pl-4" :class="{ 'hidden': isCollapsed }">
      {{ label }}
    </span>
  </router-link>
</template>

<script setup>
defineProps({
  route: {
    type: String,
    required: true
  },
  label: {
    type: String,
    required: true
  },
  icon: {
    type: [String, Object],
    required: true
  },
  isCollapsed: {
    type: Boolean,
    default: false
  }
})
</script>