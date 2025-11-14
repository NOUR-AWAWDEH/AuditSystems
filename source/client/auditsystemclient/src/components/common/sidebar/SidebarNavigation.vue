<template>
  <nav class="space-y-1 w-full">
    <NavigationItem
      v-for="item in visibleItems"
      :key="item.route"
      :route="item.route"
      :label="item.label"
      :icon="item.icon"
      :is-collapsed="isCollapsed"
      :is-active="current.path === item.route"
    />
  </nav>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import NavigationItem from './NavigationItem.vue'
import { getVisibleNavigationItems } from '@/config/navigation.js'

defineProps({
  isCollapsed: {
    type: Boolean,
    default: false
  }
})

const current = useRoute()

// Authentication check
const isAuthenticated = computed(() => {
  return !!localStorage.getItem('token')
})

// Visible items based on auth
const visibleItems = computed(() => {
  return getVisibleNavigationItems(isAuthenticated.value)
})
</script>
