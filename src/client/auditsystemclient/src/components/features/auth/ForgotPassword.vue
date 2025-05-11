<template>
  <div class="flex flex-col items-center justify-center max-w-xs sm:max-w-sm md:max-w-md mt-1 sm:my-2 mx-auto py-8">
    <AuditLogo />
    <H2Heading class="mt-5">Forgot Password</H2Heading>
    <BaseParagraph>Please enter your email to reset your password.</BaseParagraph>
    <ErrorDisplay :errors="validationErrors" :isVisible="isValidationErrorVisible" />
    <BaseInput
      placeholder="Enter your email"
      class="mb-4 w-full"
      v-model="email"
      @blur="validate"
    />
    <div class="flex items-center gap-25 w-full">
      <BaseButton class="self-start" @click="validate">Submit</BaseButton>
      <BaseLink class="text-secondary-shade whitespace-nowrap" to="/login">Back To Login</BaseLink>
    </div>
  </div>
</template>

<script setup>
import BaseButton from '@/components/base/BaseButton.vue';
import BaseInput from '@/components/base/BaseInput.vue';
import H2Heading from '@/components/base/H2Heading.vue';
import AuditLogo from '@/components/common/AuditLogo.vue';
import BaseParagraph from '@/components/base/BaseParagraph.vue';
import ErrorDisplay from '@/components/common/ErrorDisplay.vue';
import { ref } from 'vue';
import { validateEmail } from '@/services/validations/emailValidation';
import BaseLink from '@/components/base/BaseLink.vue';

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
