<template>
  <!--ResetPassword.vue-->
  <div class="flex flex-col items-center justify-center max-w-xs sm:max-w-sm md:max-w-md mt-1 sm:my-2 mx-auto py-8">
    <AuditLogo />
    <H2Heading class="mt-5">Reset Password</H2Heading>
    <BaseParagraph>Please enter your new password.</BaseParagraph>
    <ErrorDisplay :errors="validationErrors" :isVisible="isValidationErrorVisible" />
    <SuccessDisplay :messages="[successMessage]" :isVisible="isSuccessVisible" />
    <PasswordInput
      id="newPassword"
      placeholder="Enter new password"
      class="mb-4 w-full "
      v-model="newPassword"
    />
    <PasswordInput
      id="confirmPassword"
      placeholder="Confirm new password"
      class="mb-4 w-full"
      v-model="confirmPassword"
    />
    <div class="flex items-center gap-25 w-full">
      <BaseButton class="self-start py-2 px-4 rounded-md hover:bg-secondary-shade bg-secondary-base" @click="handleResetPassword">Reset Password</BaseButton>
      <BaseLink class=" whitespace-nowrap " to="/login">Back To Login</BaseLink>
    </div>
  </div>
</template>

<script setup>
import validatePassword from '@/services/validations/passwordValidation';
import BaseButton from '@/components/base/BaseButton.vue';
import PasswordInput from '@/components/base/PasswordInput.vue';
import H2Heading from '@/components/base/H2Heading.vue';
import AuditLogo from '@/components/common/AuditLogo.vue';
import BaseParagraph from '@/components/base/BaseParagraph.vue';
import ErrorDisplay from '@/components/common/ErrorDisplay.vue';
import SuccessDisplay from '@/components/common/SuccessDisplay.vue';
import BaseLink from '@/components/base/BaseLink.vue';
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useResetPassword } from '@/composables/password/useResetPassword';

const router = useRouter();
const newPassword = ref('');
const confirmPassword = ref('');
const validationErrors = ref([]);
const isValidationErrorVisible = ref(false);
const isSuccessVisible = ref(false);
const successMessage = ref('');

const { error, resetPassword: resetPasswordAction } = useResetPassword();

onMounted(() => {
  const resetToken = localStorage.getItem('resetToken');
  const email = localStorage.getItem('resetEmail');
  if (!resetToken || !email) {
    alert('Missing reset token or email. Please request a password reset.');
    router.push('/login');
  }
});

const handleResetPassword = async () => {
  validationErrors.value = [];
  isValidationErrorVisible.value = false;
  isSuccessVisible.value = false;

  const newPasswordError = validatePassword(newPassword.value);
  if (newPasswordError) {
    validationErrors.value.push(newPasswordError);
    isValidationErrorVisible.value = true;
    setTimeout(() => {
      isValidationErrorVisible.value = false;
    }, 6000);
    return;
  }

  const confirmPasswordError = validatePassword(confirmPassword.value);
  if (confirmPasswordError) {
    validationErrors.value.push(confirmPasswordError);
    isValidationErrorVisible.value = true;
    setTimeout(() => {
      isValidationErrorVisible.value = false;
    }, 6000);
    return;
  }

  if (newPassword.value !== confirmPassword.value) {
    validationErrors.value.push('Passwords do not match.');
    isValidationErrorVisible.value = true;
    setTimeout(() => {
      isValidationErrorVisible.value = false;
    }, 6000);
    return;
  }

  const resetToken = localStorage.getItem('resetToken');
  const email = localStorage.getItem('resetEmail');

  const response = await resetPasswordAction(email, resetToken, newPassword.value, confirmPassword.value);

  if (response?.success) {
    successMessage.value = response?.successMessage || 'Password reset successfully!';
    isSuccessVisible.value = true;
    setTimeout(() => {
      isSuccessVisible.value = false;
      router.push('/login');
    }, 5000);
  } else {
    validationErrors.value.push(error.value || 'Password reset failed.');
    isValidationErrorVisible.value = true;
    setTimeout(() => {
      isValidationErrorVisible.value = false;
    }, 6000);
  }
};
</script>
