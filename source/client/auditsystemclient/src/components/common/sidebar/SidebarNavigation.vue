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
    
    <!-- User Menu trigger row -->
    <div
      :class="[
        'flex items-center hover:bg-secondary-shade rounded cursor-pointer transition-all duration-200 w-full',
        isCollapsed 
          ? 'px-2 py-1 justify-center' 
          : 'px-2 py-1 justify-start'
      ]"
      @click="toggleUserMenu"
    >
      <UserMenu ref="userMenuRef" />
      <span 
        :class="[
          'text-white transition-all duration-200 truncate',
          isCollapsed ? 'hidden' : 'block ml-2 sm:ml-3 text-sm sm:text-base'
        ]"
      > User Menu
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