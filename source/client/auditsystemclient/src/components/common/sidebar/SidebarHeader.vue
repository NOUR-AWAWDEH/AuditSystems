<template>
  <!--SidebarHeader.vue-->
  <a href="#" class="flex justify-center">
    <img 
      src="@/assets/icons/AuditSystem_White.svg" 
      alt="Logo" 
      class="h-15 w-50 pr-5" 
      :class="{ 'hidden': isCollapsed }"
    >
  </a>
</template>

<script setup>
defineProps({
  isCollapsed: {
    type: Boolean,
    default: false
  }
})
</script>