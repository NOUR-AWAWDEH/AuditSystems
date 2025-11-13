<template>
  <router-link
    :to="route"
    :class="[
      'flex items-center rounded transition-all duration-200 hover:bg-secondary-shade',
      isCollapsed
        ? 'py-2 px-1 sm:px-2 justify-center'
        : 'py-2 px-2 sm:px-3 md:px-4'
    ]"
  >
    <component
      :is="icon"
      class="flex-shrink-0 transition-all duration-200 w-5 h-5"
    />
    <span
      :class="[
        'text-white transition-all duration-200 truncate',
        isCollapsed ? 'hidden' : 'block ml-2 sm:ml-3 text-sm sm:text-base'
      ]"
    >
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
