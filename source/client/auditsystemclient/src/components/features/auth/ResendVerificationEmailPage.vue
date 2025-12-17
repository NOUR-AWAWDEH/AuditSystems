<template>
  <!--ResendVerificationEmailPage.vue-->
  <div
    class="flex flex-col items-center justify-center max-w-xs sm:max-w-sm md:max-w-md mt-1 sm:my-2 mx-auto py-8"
  >
    <AuditLogo />
    <H2Heading class="mt-5">Resend Verification</H2Heading>
    <BaseParagraph>Please enter your email to resend the verification link.</BaseParagraph>

    <BaseInput
      id="email"
      placeholder="Enter your email"
      class="mb-4 w-full"
      v-model="email"
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
import { useVerificationEmail } from "@/composables/userManagement/confirmEmail/useSendVerificationEmail";
import { useNotification } from "@/composables/notification/useNotification";

// --- State ---
const email = ref("");

// --- Composables ---
const { sendVerificationEmail, isLoading } = useVerificationEmail();
const { showNotification } = useNotification();

// --- Computed Properties ---
const computedButtonText = computed(() => {
  return isLoading.value ? "Sending..." : "Send";
});

const isFormValid = computed(() => {
  return validateEmail(email.value);
});

// --- Methods ---
const validateEmailField = () => {
  if (!validateEmail(email.value)) {
    showNotification("Please enter a valid email address.", "error");
  }
};

const handleSubmit = async () => {
  if (!validateEmail(email.value)) {
    showNotification("Please enter a valid email address.", "error");
    return;
  }

  try {
    const response = await sendVerificationEmail(email.value);

    if (response?.success) {
      showNotification(response.message || "Verification email sent successfully.", "success");
      email.value = "";
    } else {
      showNotification(response?.message || "Failed to send verification email.", "error");
    }
  } catch (error) {
    console.error("Resend verification error:", error);
    showNotification(error.message || "An error occurred while processing your request.", "error");
  }
};

</script>
