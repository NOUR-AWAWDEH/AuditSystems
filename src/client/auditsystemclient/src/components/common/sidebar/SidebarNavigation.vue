<template>
  <nav>
    <NavigationItem
      v-for="item in visibleItems"
      :key="item.route"
      :route="item.route"
      :label="item.label"
      :icon="item.icon"
      :is-collapsed="isCollapsed"
    />
    
    <!-- User Menu trigger row -->
    <div
      class="flex items-center px-3 hover:bg-secondary-shade rounded cursor-pointer"
      @click="toggleUserMenu"
    >
      <UserMenu ref="userMenuRef" />
      <span class="text-white pl-2" :class="{ 'hidden': isCollapsed }">
        User Menu
      </span>
    </div>
  </nav>
</template>

<script setup>
import { computed } from 'vue'
import NavigationItem from './NavigationItem.vue'
import UserMenu from '../UserMenu.vue'
import { getVisibleNavigationItems } from '@/config/navigation.js'
import { useUserMenu } from '@/composables/sideBar/useUserMenu.js'

defineProps({
  isCollapsed: {
    type: Boolean,
    default: false
  }
})

const { userMenuRef, toggleUserMenu } = useUserMenu()

// Check authentication status (same logic as your router)
const isAuthenticated = computed(() => {
  return !!localStorage.getItem('token')
})

// Get navigation items based on auth status
const visibleItems = computed(() => {
  return getVisibleNavigationItems(isAuthenticated.value)
})
</script>