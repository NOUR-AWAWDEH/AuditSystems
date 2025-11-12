<template>
  <!--LoginForm.vue-->
  <div class="bg-white text-secondary-base p-6 rounded-lg w-full max-w-sm mx-auto h-auto md:max-w-md lg:max-w-lg">
    <div class="flex justify-center mb-6 w-full">
      <Logo />
    </div>
    <h2 class="text-2xl font-bold mb-6 text-center text-secondary-shade">Login</h2>
    
    <form @submit.prevent="handleLogin">
      <ErrorDisplay :errors="validationErrors" :isVisible="isValidationErrorVisible"/>

      <transition name="fade">
        <div
          v-if="errorMessage && isErrorVisible"
          class="mb-4 p-3 bg-soft-error text-error rounded-md"
        >
          {{ errorMessage }}
        </div>
      </transition>
      
      <div class="mb-4">
        <label for="username" class="block mb-2 font-medium text-left">User Name</label>
        <BaseInput
          id="username"
          type="text"
          v-model="username"
          placeholder="Enter your username"
          :disabled="isLoading"
          autocomplete="username"
          autofocus
        />
      </div>

      <div class="mb-6">
        <label for="password" class="block mb-2 font-medium text-left">Password</label>
        <PasswordInput
          id="password"
          v-model="password"
          placeholder="Enter your password"
          :disabled="isLoading"
        />
      </div>

      
      <BaseButton 
        type="submit" 
        :disabled="isLoading" 
        class="w-full py-2 px-4 rounded-md hover:bg-secondary-shade bg-secondary-base"
      >
        <template #loading>Logging in...</template>
        <span v-if="!isLoading">Login</span> 
        <span v-else>Logging in...</span>
      </BaseButton>
    </form>
    
    <div class="text-center mt-4">
      <BaseLink @click="clearLocalStorage" to="/forgot-password">
        Forgot Password?
      </BaseLink>
    </div>
    
    <!-- Build Time Info (Development Only) -->
    <div class="text-center mt-2 text-xs text-gray-400 font-mono">
      Build: {{ buildTime }}
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import { validateUsername } from '@/services/validations/userValidation';
import validatePassword from '@/services/validations/passwordValidation';
import { useLogin } from '@/composables/auth/useLogin';
import BaseButton from '@/components/base/BaseButton.vue';
import BaseInput from '@/components/base/BaseInput.vue';
import BaseLink from '@/components/base/BaseLink.vue';
import PasswordInput from '@/components/base/PasswordInput.vue';
import ErrorDisplay from '@/components/common/ErrorDisplay.vue';
import { useRouter } from 'vue-router';
import Logo from '@/components/common/AuditLogo.vue';

// Composables
const { login, error } = useLogin();
const router = useRouter();

// Refs
const username = ref('');
const password = ref('');
const isLoading = ref(false);
const validationErrors = ref([]);
const isValidationErrorVisible = ref(false);

const errorMessage = ref('')
const isErrorVisible = ref(false);

// Build time for development
const buildTime = ref(import.meta.env.VITE_BUILD_TIME || new Date().toLocaleString());

// Constants
const ERROR_DISPLAY_DURATION = 5000; // 5 seconds
let errorTimeout = null;

// Watchers
watch(error, (newError) => {
  if (newError) {
    errorMessage.value = newError;
    isErrorVisible.value = true;
    clearTimeout(errorTimeout);
    errorTimeout = setTimeout(() => {
      if (error.value === newError) {
        isErrorVisible.value = false;
      }
    }, ERROR_DISPLAY_DURATION);
  } else {
    errorMessage.value = '';
    isErrorVisible.value = false;
    clearTimeout(errorTimeout);
  }
});

// Methods
const validateForm = () => {
  validationErrors.value = [];
  const usernameError = validateUsername(username.value);
  const passwordError = validatePassword(password.value);

  if (usernameError) validationErrors.value.push(usernameError);
  if (passwordError) validationErrors.value.push(passwordError);

  isValidationErrorVisible.value = validationErrors.value.length > 0;
  if (isValidationErrorVisible.value) {
    setTimeout(() => {
      isValidationErrorVisible.value = false;
    }, ERROR_DISPLAY_DURATION);
  }
  return validationErrors.value.length === 0;
};

const handleLogin = async () => {
  if (!validateForm()) {
    return;
  }
  
  isLoading.value = true;
  errorMessage.value = '';
  isErrorVisible.value = false;
  
  try {
    const response = await login(username.value, password.value);
    
    if (response?.success) {
      router.replace('/dashboard');
    } else {
      // Error is already set in the composable and will be picked up by the watcher
    }
  } catch (err) {
    console.error('💥 [LOGIN FORM] Unexpected error during login:', {
      err,
      message: err?.message,
      stack: err?.stack
    });
    errorMessage.value = 'An unexpected error occurred. Please try again.';
    isErrorVisible.value = true;
  } finally {
    isLoading.value = false;
  }
};

const clearLocalStorage = () => {
  localStorage.clear();
};
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>