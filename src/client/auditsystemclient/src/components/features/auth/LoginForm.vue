<script setup>
import { ref } from 'vue';
import { useAuth } from '@/composables/useAuth';

const { login, error } = useAuth();
const username = ref('');
const password = ref('');
const isLoading = ref(false);
const validationErrors = ref([]);

const validateUsername = (username) => {
  if (!username) return 'Username is required';
  if (username.length < 3) return 'Username must be at least 3 characters';
  if (username.length > 50) return 'Username must be less than 50 characters';
  if (!/^[a-zA-Z0-9_.-]+$/.test(username))
    return 'Username can only contain letters, numbers, underscores, dots, and hyphens';
  return null;
};

const validatePassword = (password) => {
  if (!password) return 'Password is required';
  if (password.length < 8) return 'Password must be at least 8 characters';
  if (!/[A-Z]/.test(password)) return 'Password must contain an uppercase letter';
  if (!/[0-9]/.test(password)) return 'Password must contain a number';
  if (!/[^A-Za-z0-9]/.test(password))
    return 'Password must contain a special character';
  return null;
};

const handleLogin = async () => {
  validationErrors.value = [];

  const usernameError = validateUsername(username.value);
  const passwordError = validatePassword(password.value);

  if (usernameError) validationErrors.value.push(usernameError);
  if (passwordError) validationErrors.value.push(passwordError);

  if (validationErrors.value.length) return;

  isLoading.value = true;
  const response = await login(username.value, password.value);
  isLoading.value = false;

  if (response.success) {
    window.location.href = '/logout';
  }
};
</script>

<template>
  <div class="bg-white text-secondary-base p-8 rounded-lg w-full max-w-md mx-auto">
    <h2 class="text-2xl font-bold mb-6 text-center">Login</h2>

    <div v-if="validationErrors.length" class="mb-4 p-3 bg-red-100 text-red-700 rounded">
      <ul class="error-list">
        <li v-for="(error, index) in validationErrors" :key="index">{{ error }}</li>
      </ul>
    </div>

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
