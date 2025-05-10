<script setup>
import { ref, watch } from 'vue';
import { useLogin } from '@/composables/useLogin';
import BaseButton from '@/components/BaseButton.vue';
import BaseInput from '@/components/BaseInput.vue';
import BaseLink from '@/components/BaseLink.vue';
import PasswordInput from '@/components/PasswordInput.vue';
import { useLoginValidation } from '@/services/validations/loginValidation';
import { useRouter } from 'vue-router';
import Logo from '@/components/common/AuditLogo.vue';

// Composables
const { login, error } = useLogin();
const { validateUsername, validatePassword } = useLoginValidation();
const router = useRouter();

// Refs
const username = ref('');
const password = ref('');
const isLoading = ref(false);
const validationErrors = ref([]);
const isValidationErrorVisible = ref(false);
const isErrorVisible = ref(false);

// Constants
const ERROR_DISPLAY_DURATION = 2000;
let errorTimeout = null;

// Watchers
watch(error, (newError) => {
  if (newError) {
    isErrorVisible.value = true;
    clearTimeout(errorTimeout);
    errorTimeout = setTimeout(() => {
      if (error.value === newError) {
        isErrorVisible.value = false;
      }
    }, ERROR_DISPLAY_DURATION);
  }
});

// Methods
const validateForm = () => {
  validationErrors.value.splice(0);
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
  if (!validateForm()) return;
  isLoading.value = true;
  const response = await login(username.value, password.value);
  isLoading.value = false;

  if (response?.success) {
    router.replace('/dashboard');
  }
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
          <li v-for="(err, index) in validationErrors" :key="index">{{ err }}</li>
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

    <div class="mb-4">
      <label for="username" class="block mb-2 font-medium text-left">User Name</label>
      <BaseInput
        id="username"
        type="text"
        v-model="username"
        placeholder="Enter your username"
        :disabled="isLoading"
        autofocus
        @keydown.enter="handleLogin"
      />
    </div>

    <div class="mb-6">
      <label for="password" class="block mb-2 font-medium text-left">Password</label>
      <PasswordInput
        id="password"
        v-model="password"
        placeholder="Enter your password"
        :disabled="isLoading"
        @login="handleLogin"
        @keydown.enter="handleLogin"/>
    </div>

    <BaseButton @click="handleLogin" :disabled="isLoading">
  <template #loading>Logging in...</template>
  <span v-if="!isLoading">Login</span> <span v-else>Is logging in...</span>
</BaseButton>

    <div class="text-center mt-4">
      <BaseLink to="/forgot-password">
        Forgot Password?
      </BaseLink>
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
