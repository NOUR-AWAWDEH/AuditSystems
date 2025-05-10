<template>
  <div class="flex flex-col items-center justify-center max-w-md mx-auto py-8">
    <H2Heading>Forgot Password</H2Heading>
    <BaseParagraph>Please enter your email to reset your password.</BaseParagraph>
    <ErrorDisplay :errors="validationErrors" :isVisible="isValidationErrorVisible" />
    <BaseInput
      placeholder="Enter your email"
      class="mb-4"
      v-model="email"
      @blur="validate"
    />
    <BaseButton @click="validate">Submit</BaseButton>
    <BaseButton class="mt-2" @click="goToLogin()">Back to Login</BaseButton>
  </div>
</template>

<script setup>
import BaseButton from '../../BaseButton.vue';
import BaseInput from '@/components/BaseInput.vue';
import H2Heading from '@/components/H2Heading.vue';
import BaseParagraph from '@/components/BaseParagraph.vue';
import ErrorDisplay from '@/components/ErrorDisplay.vue';
import { ref } from 'vue';
import { validateEmail } from '@/services/validations/emailValidation';
import router from '@/router/router';

const email = ref('');
const validationErrors = ref([]);

const goToLogin = () => {
  console.log('goToLogin function called');
  router.replace('/login');
};
const isValidationErrorVisible = ref(false);

const validate = () => {
  validationErrors.value = [];
  const emailError = validateEmail(email.value) ? null : 'Please enter a valid email address.';
  if (emailError) {
    validationErrors.value.push(emailError);
    isValidationErrorVisible.value = true;
    setTimeout(() => {
      isValidationErrorVisible.value = false;
    }, 2000);
  } else {
    isValidationErrorVisible.value = false;
  }
};
</script>
