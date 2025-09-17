<template>
  <nav class="space-y-1 w-full">
    <NavigationItem
      v-for="item in visibleItems"
      :key="item.route"
      :route="item.route"
      :label="item.label"
      :icon="item.icon"
      :is-collapsed="isCollapsed"
    />
  </nav>
</template>

<script setup>
import { computed } from 'vue'
import NavigationItem from './NavigationItem.vue'
import { getVisibleNavigationItems } from '@/config/navigation.js'

defineProps({
  isCollapsed: {
    type: Boolean,
    default: false
  }
})

// Check authentication status (same logic as your router)
const isAuthenticated = computed(() => {
  return !!localStorage.getItem('token')
})

// Get navigation items based on auth status
const visibleItems = computed(() => {
  return getVisibleNavigationItems(isAuthenticated.value)
})
</script>