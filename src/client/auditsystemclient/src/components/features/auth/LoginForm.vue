<script setup>
import { ref } from 'vue'
import { useAuth } from '@/composables/useAuth'

const props = defineProps({
  onLoginSuccess: {
    type: Function,
    default: () => {}
  }
})

const username = ref('')
const password = ref('')
const error = ref('')
const isLoading = ref(false)

const { login } = useAuth()

const handleLogin = async () => {
  if (!username.value || !password.value) {
    error.value = 'Please enter both username and password'
    return
  }
  
  try {
    isLoading.value = true
    error.value = ''
    
    // This would call your actual authentication service
    await login(username.value, password.value)
    props.onLoginSuccess()
  } catch (err) {
    error.value = err.message || 'Login failed. Please try again.'
  } finally {
    isLoading.value = false
  }
}

const handleForgotPassword = () => {
  // Implement forgot password functionality
  console.log('Forgot password clicked')
}
</script>

<template>
  <div class="bg-white text-secondary-base p-8 rounded-lg w-full max-w-md mx-auto">
    <h2 class="text-2xl font-bold mb-6 text-center">Login</h2>
    
    <div v-if="error" class="mb-4 p-3 bg-red-100 text-red-700 rounded">
      {{ error }}
    </div>
    
    <div class="mb-4">
      <label for="username" class="block mb-2 font-medium text-left">User Name</label>
      <input 
        id="username" 
        v-model="username" 
        type="text" 
        class="text-gray-base w-full px-4 py-2 border border-grey-light rounded focus:outline-none focus:ring-2 focus:ring-primary"
        placeholder="Enter your user name"
        :disabled="isLoading"
      />
    </div>
    
    <div class="mb-6">
      <label for="password" class="block text-grey mb-2 font-medium text-left">Password</label>
      <input 
        id="password" 
        v-model="password" 
        type="password" 
        class="text-gray-base w-full px-4 py-2 border border-gray-light rounded focus:outline-none focus:ring-2 focus:ring-primary"
        placeholder="Enter your password"
        :disabled="isLoading"
      />
    </div>
    
    <div class="flex justify-center">
      <button 
        @click="handleLogin" 
        class="w-full bg-secondary-base hover:bg-primary-dark text-white font-medium py-2 px-4 rounded transition-colors duration-300"
        :disabled="isLoading"
      >
        <span v-if="isLoading">Logging in...</span>
        <span v-else>Login</span>
      </button>
    </div>
    
    <!-- Forgot Password Link -->
    <div class="text-center mt-4">
      <a 
        @click="handleForgotPassword" 
        class="text-primary hover:text-primary-dark cursor-pointer text-sm"
      >
        Forgot Password?
      </a>
    </div>
  </div>
</template>