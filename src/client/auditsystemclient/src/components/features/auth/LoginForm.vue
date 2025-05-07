<script setup>
import { ref, watch } from 'vue';
import { useAuth } from '@/composables/useAuth';
import { useLoginValidation } from '@/services/validations/loginValidation';
import { useRouter } from 'vue-router';
import Logo from '@/components/common/AuditLogo.vue';

const { login, error } = useAuth();
const { validateUsername, validatePassword } = useLoginValidation();
const router = useRouter();

const username = ref('');
const password = ref('');
const isLoading = ref(false);
const validationErrors = ref([]);
const loginSuccessMessage = ref('');
const isValidationErrorVisible = ref(false);
const showPassword = ref(false);
const isErrorVisible = ref(false);

watch(error, (newError) => {
  if (newError) {
    isErrorVisible.value = true;
    setTimeout(() => {
      if (error.value === newError) {
        isErrorVisible.value = false;
      }
    }, 2000);
  }
});

const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value;
};

const handleLogin = async () => {
  validationErrors.value = [];

  const usernameError = validateUsername(username.value);
  const passwordError = validatePassword(password.value);

  if (usernameError) validationErrors.value.push(usernameError);
  if (passwordError) validationErrors.value.push(passwordError);

  if (validationErrors.value.length) {
    isValidationErrorVisible.value = true;
    setTimeout(() => {
      if (validationErrors.value.length === 0) {
        isValidationErrorVisible.value = false;
      }
    }, 2000);
    return;
  }

  isLoading.value = true;
  const response = await login(username.value, password.value);
  isLoading.value = false;
  if (response.success) {
    router.replace('/dashboard');
  }
};

const handleForgotPassword = () => {
  console.log('Forgot password clicked');
};
</script>

<template>
  <div class="bg-white text-secondary-base p-6 rounded-lg w-full max-w-md mx-auto h-auto">
    <div class="flex justify-center mb-6 w-full">
      <Logo />
    </div>
    <h2 class="text-2xl font-bold mb-6 text-center text-secondary-shade">Login</h2>

    <transition name="fade">
      <div
        v-if="validationErrors.length && isValidationErrorVisible"
        class="mb-4 p-3 bg-soft-error text-error rounded-md"
      >
        <ul class="list-none pl-5">
          <li v-for="(error, index) in validationErrors" :key="index">{{ error }}</li>
        </ul>
      </div>
    </transition>

    <transition name="fade">
      <div
        v-if="error && isErrorVisible"
        class="mb-4 p-3 bg-soft-error text-error rounded-md"
      >
        {{ error }}
      </div>
    </transition>

    <transition name="fade">
      <div
        v-if="loginSuccessMessage && isSuccessMessageVisible"
        class="mb-4 p-3 bg-soft-success text-success rounded-md"
      >
        {{ loginSuccessMessage }}
      </div>
    </transition>

    <div class="mb-4">
      <label for="username" class="block mb-2 font-medium text-left">User Name</label>
      <input
        id="username"
        v-model="username"
        type="text"
        class="w-full px-4 py-2 text-secondary-shade border border-secondary-base rounded-md focus:outline-none focus:ring-2 disabled:bg-gray-tint"
        placeholder="Enter your username"
        :disabled="isLoading"
        @keydown.enter="handleLogin"
      />
    </div>

    <div class="mb-6 relative">
      <label for="password" class="block mb-2 font-medium text-left">Password</label>
      <div class="relative">
        <input
          id="password"
          v-model="password"
          :type="showPassword ? 'text' : 'password'"
          class="w-full px-4 py-2 text-secondary-shade border border-secondary-base rounded-md focus:outline-none focus:ring-2 disabled:bg-gray-tint pr-10"
          placeholder="Enter your password"
          :disabled="isLoading"
          @keydown.enter="handleLogin"
        />
        <button
          type="button"
          class="absolute inset-y-0 right-0 pr-3 flex items-center"
          @click="togglePasswordVisibility"
        >
          <svg
            class="h-5 w-5 text-gray-base"
            :class="{ 'text-secondary-tint': showPassword }"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          >
            <path
              v-if="showPassword"
              d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"
            />
            <circle v-if="showPassword" cx="12" cy="12" r="3" />
            <path
              v-if="!showPassword"
              d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"
            />
            <line v-if="!showPassword" x1="1" y1="1" x2="23" y2="23" />
          </svg>
        </button>
      </div>
    </div>

    <button
      @click="handleLogin"
      class="w-full bg-secondary-base hover:bg-secondary-shade text-white font-medium py-2 px-4 rounded-md transition-colors duration-300 cursor-pointer"
      :disabled="isLoading"
    >
      <span v-if="isLoading">Logging in...</span>
      <span v-else>Login</span>
    </button>

    <div class="text-center mt-4">
      <a
        @click="handleForgotPassword"
        class="hover:text-secondary-shade cursor-pointer text-sm"
      >
        Forgot Password?
      </a>
    </div>
  </div>
</template>

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
