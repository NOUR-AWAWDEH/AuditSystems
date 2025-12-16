<template>
  <!--ResetPassword.vu-->
  <div
    class="flex flex-col items-center justify-center max-w-xs sm:max-w-sm md:max-w-md mt-1 sm:my-2 mx-auto py-8"
  >
    <AuditLogo />
    <H2Heading class="mt-5">Reset Password</H2Heading>
    <BaseParagraph>Enter your new password below.</BaseParagraph>

    <PasswordInput
      id="new-password"
      placeholder="New Password"
      class="mb-4 w-full"
      v-model="newPassword"
    />

    <PasswordInput
      id="confirm-password"
      placeholder="Confirm Password"
      class="mb-6 w-full"
      v-model="confirmPassword"
    />

    <div class="flex items-center gap-25 w-full">
      <BaseButton
        class="self-start py-2 px-4 rounded-md hover:bg-secondary-shade bg-secondary-base"
        @click="handleResetPassword"
        :disabled="isLoading"
      >
        {{ isLoading ? "Resetting..." : "Reset Password" }}
      </BaseButton>
    </div>
  </div>
</template>

<script setup>
import validatePassword from "@/services/validations/validatePassword";
import BaseButton from "@/components/base/BaseButton.vue";
import PasswordInput from "@/components/base/PasswordInput.vue";
import H2Heading from "@/components/base/H2Heading.vue";
import AuditLogo from "@/components/common/AuditLogo.vue";
import BaseParagraph from "@/components/base/BaseParagraph.vue";
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useResetPassword } from "@/composables/password/useResetPassword";
import { useNotification } from "@/composables/notification/useNotification";

const router = useRouter();
const route = useRoute();
const newPassword = ref("");
const confirmPassword = ref("");
const isLoading = ref(false);

const { resetPassword: resetPasswordAction } = useResetPassword();
const { showNotification } = useNotification();

// Store email and token for submission
const emailParam = ref("");
const tokenParam = ref("");

onMounted(() => {
  const urlToken = route.query.token;
  const urlEmail = route.query.email;

  if (urlToken && urlEmail) {
    emailParam.value = urlEmail;
    tokenParam.value = urlToken;
  } else {
    showNotification("Missing reset token or email. Please request a password reset.", "error");
  }
});

const handleResetPassword = async () => {
  if (!newPassword.value || !confirmPassword.value) {
    showNotification("Please fill in all fields.", "error");
    return;
  }

  if (newPassword.value !== confirmPassword.value) {
    showNotification("Passwords do not match.", "error");
    return;
  }

  const passwordError = validatePassword(newPassword.value);
  if (passwordError) {
    showNotification(passwordError, "error");
    return;
  }

  isLoading.value = true;
  try {
    const response = await resetPasswordAction({
      email: emailParam.value,
      token: tokenParam.value,
      newPassword: newPassword.value,
      confirmNewPassword: confirmPassword.value,
    });

    isLoading.value = false;

    if (response?.success) {
      showNotification(response.successMessage || "Password reset successfully.", "success");
      // Redirect to login after a short delay
      setTimeout(() => {
        router.push("/login");
      }, 2000);
    } else {
      showNotification(response.message || "Failed to reset password. Please try again.", "error");
    }
  } catch (err) {
    isLoading.value = false;
    console.error("Reset Password error:", err);
    showNotification(err.message || "An unexpected error occurred.", "error");
  }
};
</script>
