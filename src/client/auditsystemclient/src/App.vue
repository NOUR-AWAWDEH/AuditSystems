<script setup>
import LoginForm from '@/components/features/auth/LoginForm.vue'
import Logo from '@/components/common/AuditLogo.vue'
import { useAuth } from '@/composables/useAuth'

const { isAuthenticated } = useAuth()

const handleLoginSuccess = () => {
  console.log('Login successful, would redirect to dashboard')
}
</script>

<template>
  <div class="flex flex-col items-center justify-center min-h-screen p-4">
    <!-- Logo -->
    <div class="flex justify-center mb-8 w-full">
      <Logo />
    </div>

    <!-- Login Form -->
    <LoginForm
      v-if="!isAuthenticated"
      :onLoginSuccess="handleLoginSuccess"
    />

  </div>
</template>
