<script setup>
import Logo from '@/components/common/AuditLogo.vue'
</script>

<template>
  <div class="flex flex-col items-center justify-center min-h-screen p-4">
    <!-- Logo -->
    <div class="flex justify-center mb-8 w-full">
      <Logo />
    </div>
    <router-view></router-view>
  </div>
</template>
