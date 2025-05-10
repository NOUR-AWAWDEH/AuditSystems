<template>
  <div class="flex flex-col items-center justify-center max-w-md mx-auto py-8">
    <AuditLogo class="mb-4" />
    <H2Heading class="mt-5">Forgot Password</H2Heading>
    <BaseParagraph>Please enter your email to reset your password.</BaseParagraph>
    <ErrorDisplay :errors="validationErrors" :isVisible="isValidationErrorVisible" />
    <BaseInput
      placeholder="Enter your email"
      class="mb-4"
      v-model="email"
      @blur="validate"
    />
    <BaseButton @click="validate">Submit</BaseButton>
    <div class="text-center mt-4">
      <BaseLink to="/login">Back To Login</BaseLink>
    </div>

  </div>
</template>

<script setup>
import BaseButton from '../../BaseButton.vue';
import BaseInput from '@/components/BaseInput.vue';
import H2Heading from '@/components/H2Heading.vue';
import AuditLogo from '@/components/common/AuditLogo.vue';
import BaseParagraph from '@/components/BaseParagraph.vue';
import ErrorDisplay from '@/components/ErrorDisplay.vue';
import { ref } from 'vue';
import { validateEmail } from '@/services/validations/emailValidation';
import BaseLink from '@/components/BaseLink.vue';

const email = ref('');
const validationErrors = ref([]);

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
