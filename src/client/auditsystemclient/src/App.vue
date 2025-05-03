<script setup>
import { ref } from 'vue'
import LoginForm from '@/components/features/auth/LoginForm.vue'
import Logo from '@/components/common/Logo.vue'
import { useAuth } from '@/composables/useAuth'

const { isAuthenticated } = useAuth()

const handleLoginSuccess = () => {
  // In a real app with router, you would redirect to dashboard
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