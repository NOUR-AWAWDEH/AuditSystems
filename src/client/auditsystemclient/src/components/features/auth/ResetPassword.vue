<template>
  <div class="flex flex-col items-center justify-center max-w-xs sm:max-w-sm md:max-w-md mt-1 sm:my-2 mx-auto py-8">
    <AuditLogo />
    <H2Heading class="mt-5">Reset Password</H2Heading>
    <BaseParagraph>Please enter your new password.</BaseParagraph>
    <ErrorDisplay :errors="validationErrors" :isVisible="isValidationErrorVisible" />
    <SuccessDisplay :messages="[successMessage]" :isVisible="isSuccessVisible" />
    <BaseInput
      id="newPassword"
      placeholder="Enter new password"
      class="mb-4 w-full"
      v-model="newPassword"
      type="password"
    />
    <BaseInput
      id="confirmPassword"
      placeholder="Confirm new password"
      class="mb-4 w-full"
      v-model="confirmPassword"
      type="password"
    />
    <div class="flex items-center gap-25 w-full">
      <BaseButton class="self-start" @click="handleResetPassword">Reset Password</BaseButton>
      <BaseLink class=" whitespace-nowrap " to="/login">Back To Login</BaseLink>
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
import SuccessDisplay from '@/components/common/SuccessDisplay.vue';
import BaseLink from '@/components/base/BaseLink.vue';
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const newPassword = ref('');
const confirmPassword = ref('');
const validationErrors = ref([]);
const isValidationErrorVisible = ref(false);
const isSuccessVisible = ref(false);
const successMessage = ref('');

onMounted(() => {
  const resetToken = localStorage.getItem('resetToken');
  if (!resetToken) {
    router.push('/login'); // Redirect to login if token is not present
  }
});

const handleResetPassword = () => {
  validationErrors.value = [];
  if (newPassword.value !== confirmPassword.value) {
    validationErrors.value.push('Passwords do not match.');
    isValidationErrorVisible.value = true;
    setTimeout(() => {
      isValidationErrorVisible.value = false;
    }, 6000);
    return;
  }
  // Add logic to handle password reset
  successMessage.value = 'Password reset successfully.';
  isSuccessVisible.value = true;
  setTimeout(() => {
    isSuccessVisible.value = false;
  }, 6000);
};
</script>
