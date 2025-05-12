<template>
  <div class="flex flex-col items-center justify-center max-w-xs sm:max-w-sm md:max-w-md mt-1 sm:my-2 mx-auto py-8">
    <AuditLogo />
    <H2Heading class="mt-5">Forgot Password</H2Heading>
    <BaseParagraph>Please enter your email to reset your password.</BaseParagraph>

    <ErrorDisplay :errors="validationErrors" :isVisible="isValidationErrorVisible" />
    <SuccessDisplay :messages="[successMessage]" :isVisible="isSuccessVisible" />

    <BaseInput
      id = "email"
      placeholder="Enter your email"
      class="mb-4 w-full"
      v-model="resetEmail"
      @blur="validateEmailField"
      :disabled="isCodeInputVisible || isLoading"
    />

    <BaseInput
      id= "reset-code"
      v-if="isCodeInputVisible"
      placeholder="Enter the code"
      class="mb-4 w-full"
      v-model="resetCode"
      :disabled="isLoading"
    />

    <div class="flex items-center gap-25 w-full">
      <BaseButton
        class="self-start"
        @click="handleSubmit"
        :disabled="isLoading || !isFormValid"
      >
        {{ computedButtonText }}
      </BaseButton>
      <BaseLink class="whitespace-nowrap" to="/login">Back To Login</BaseLink>
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

import { ref, computed } from 'vue';
import { validateEmail } from '@/services/validations/emailValidation';
import { useForgetPassword } from '@/composables/password/useForgetPassword';
import { useVerifyResetCode } from '@/composables/password/useVerifyResetCode';
import { useRouter } from 'vue-router';

// --- State ---
const resetEmail = ref('');
const resetCode = ref('');
const resetToken = ref('');
const validationErrors = ref([]);
const isValidationErrorVisible = ref(false);
const successMessage = ref('');
const isSuccessVisible = ref(false);
const isCodeInputVisible = ref(false);
const isLoading = ref(false);

const resetCodeExpirationTime = 15 * 60 * 1000; // 15 minutes

// --- Composables ---
const { forgetPassword } = useForgetPassword();
const { verifyCode } = useVerifyResetCode();
const router = useRouter();

// --- Computed Properties ---
const computedButtonText = computed(() => {
  if (isLoading.value) {
    return isCodeInputVisible.value ? 'Verifying...' : 'Loading...';
  }
  return isCodeInputVisible.value ? 'Verify Code' : 'Submit';
});

const isFormValid = computed(() => {
  const valid = isCodeInputVisible.value
    ? !!resetCode.value
    : validationErrors.value.length === 0 && validateEmail(resetEmail.value);
  return valid;
});

// --- Methods ---
const showMessage = (isVisibleRef, duration = 5000) => {
  isVisibleRef.value = true;
  setTimeout(() => {
    isVisibleRef.value = false;
  }, duration);
};

const validateEmailField = () => {
  validationErrors.value = [];
  if (!validateEmail(resetEmail.value)) {
    validationErrors.value.push('Please enter a valid email address.');
    showMessage(isValidationErrorVisible, 5000);
  }
};

const handleSubmit = async () => {
  isValidationErrorVisible.value = false;
  isSuccessVisible.value = false;
  validationErrors.value = [];

  if (!isCodeInputVisible.value) {
    // --- Step 1: Request Reset Code ---
    if (!validateEmail(resetEmail.value)) {
      validationErrors.value.push('Please enter a valid email address.');
      showMessage(isValidationErrorVisible, 5000);
      return;
    }

    isLoading.value = true;
    try {
      const response = await forgetPassword(resetEmail.value);
      isLoading.value = false;

      if (response.success) {
        successMessage.value = response.message || 'A reset code has been sent to your email.';
        showMessage(isSuccessVisible, 5000);
        isCodeInputVisible.value = true;

        // Set a timer to expire the reset code input after 15 minutes
        setTimeout(() => {
          isCodeInputVisible.value = false;
        }, resetCodeExpirationTime);
      } else {
        validationErrors.value.push(response.message || 'Failed to send reset code. Please try again.');
        showMessage(isValidationErrorVisible, 6000);
      }
    } catch (error) {
      isLoading.value = false;
      console.error('forgetPassword error:', error);
      validationErrors.value.push('An error occurred while requesting the reset code.');
      showMessage(isValidationErrorVisible, 6000);
    }
  } else {
    // --- Step 2: Verify Reset Code ---
    if (!resetCode.value) {
      validationErrors.value.push('Please enter the verification code.');
      showMessage(isValidationErrorVisible, 5000);
      return;
    }

    isLoading.value = true;
    try {
      const trimmedCode = resetCode.value.trim();
      const response = await verifyCode(resetEmail.value, trimmedCode);
      isLoading.value = false;

      if (response.success) {
        const resetTokenValue = response.data?.resetToken;
        if (!resetTokenValue) {
          validationErrors.value.push('Invalid response from server: No reset token provided.');
          showMessage(isValidationErrorVisible, 6000);
          return;
        }

        localStorage.setItem('resetToken', resetTokenValue);
        resetToken.value = resetTokenValue;
        localStorage.setItem('resetEmail', resetEmail.value);
        router.push('/reset-password').catch((err) => {
          console.error('Navigation error:', err);
          validationErrors.value.push('Failed to navigate to reset password page.');
          showMessage(isValidationErrorVisible, 6000);
        });
      } else {
        validationErrors.value.push(response.message || 'Invalid or expired code. Please try again.');
        showMessage(isValidationErrorVisible, 6000);
      }
    } catch (error) {
      isLoading.value = false;
      console.error('verifyCode error:', error);
      validationErrors.value.push('An error occurred while verifying the code.');
      showMessage(isValidationErrorVisible, 6000);
    }
  }
};

</script>

<style scoped>
/* Add any necessary styles */
</style>
