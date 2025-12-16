<template>
  <!--ForgotPassword.vue-->
  <div
    class="flex flex-col items-center justify-center max-w-xs sm:max-w-sm md:max-w-md mt-1 sm:my-2 mx-auto py-8"
  >
    <AuditLogo />
    <H2Heading class="mt-5">Forgot Password</H2Heading>
    <BaseParagraph>Please enter your email to reset your password.</BaseParagraph>

    <BaseInput
      id="email"
      placeholder="Enter your email"
      class="mb-4 w-full"
      v-model="resetEmail"
      @blur="validateEmailField"
      :disabled="isLoading"
    />

    <div class="flex items-center gap-25 w-full">
      <BaseButton
        class="self-start py-2 px-4 rounded-md hover:bg-secondary-shade bg-secondary-base"
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
import BaseButton from "@/components/base/BaseButton.vue";
import BaseInput from "@/components/base/BaseInput.vue";
import H2Heading from "@/components/base/H2Heading.vue";
import AuditLogo from "@/components/common/AuditLogo.vue";
import BaseParagraph from "@/components/base/BaseParagraph.vue";
import BaseLink from "@/components/base/BaseLink.vue";
import { ref, computed } from "vue";
import { validateEmail } from "@/services/validations/validateEmail";
import { useForgetPassword } from "@/composables/password/useForgetPassword";
import { useNotification } from "@/composables/notification/useNotification";

// --- State ---
const resetEmail = ref("");
const isLoading = ref(false);

// --- Composables ---
const { forgetPassword } = useForgetPassword();
const { showNotification } = useNotification();

// --- Computed Properties ---
const computedButtonText = computed(() => {
  return isLoading.value ? "Loading..." : "Submit";
});

const isFormValid = computed(() => {
  return validateEmail(resetEmail.value);
});

// --- Methods ---
const validateEmailField = () => {
  if (!validateEmail(resetEmail.value)) {
    showNotification("Please enter a valid email address.", "error");
  }
};

const handleSubmit = async () => {
  if (!validateEmail(resetEmail.value)) {
    showNotification("Please enter a valid email address.", "error");
    return;
  }

  isLoading.value = true;
  try {
    const response = await forgetPassword(resetEmail.value);
    isLoading.value = false;

    if (response.success) {
      showNotification(
        response.message ||
          "If an account matches that email, a password reset link has been sent.",
        "success"
      );
      resetEmail.value = ""; // Clear input on success
    } else {
      showNotification(response.message || "Failed to process request. Please try again.", "error");
    }
  } catch (error) {
    isLoading.value = false;
    console.error("forgetPassword error:", error);
    showNotification(error.message || "An error occurred while processing your request.", "error");
  }
};
</script>
