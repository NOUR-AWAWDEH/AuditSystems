<template>
  <div class="flex justify-center items-center min-h-screen bg-gray-100 font-sans">
    <div class="bg-white p-10 rounded-xl shadow-lg text-center max-w-md w-[90%]">
      <div class="flex justify-center mb-6">
        <AuditLogo width="w-48" />
      </div>
      <div v-if="isLoading" class="loading-state">
        <div
          class="w-12 h-12 border-4 border-gray-200 border-t-blue-600 rounded-full animate-spin mx-auto mb-6"
        ></div>
        <p class="text-gray-400 mb-6 leading-relaxed">Verifying your email...</p>
      </div>

      <div v-else-if="success" class="success-state">
        <div class="text-6xl mb-4 leading-none text-emerald-500">✓</div>
        <h2 class="text-2xl font-bold mb-2 text-secondary-shade">Email Confirmed!</h2>
        <p class="text-secondary-base mb-6 leading-relaxed">{{ success.message }}</p>
        <p class="text-sm text-gray-400 mt-4">Redirecting to login in {{ count }} seconds...</p>
      </div>

      <div v-else class="error-state">
        <div class="text-6xl mb-4 leading-none text-red-500">✕</div>
        <h2 class="text-2xl font-bold mb-2 text-secondary-shade">Verification Failed</h2>
        <p class="text-gray-400 mb-6 leading-relaxed">{{ error }}</p>
        <button
          @click="goToLogin"
          class="bg-blue-600 text-white px-6 py-3 rounded-md font-semibold hover:bg-blue-700 transition-transform hover:-translate-y-px active:translate-y-0 cursor-pointer"
        >
          Go to Login
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useConfirmEmail } from "@/composables/userManagement/confirmEmail/useConfirmEmail";
import AuditLogo from "@/components/common/AuditLogo.vue";

const count = ref(3);
const route = useRoute();
const router = useRouter();
// Destructure and ensure reactivity
const { confirmEmail, isLoading, error, success } = useConfirmEmail();

const goToLogin = () => {
  router.push("/login");
};

onMounted(async () => {
  const { uid, token } = route.query;
  // Even if params are missing, call confirmEmail so it can handle the error state consistent with the composable's logic
  const result = await confirmEmail(uid, token);

  if (result?.success) {
    const timer = setInterval(() => {
      count.value--;
      if (count.value <= 0) {
        clearInterval(timer);
        goToLogin();
      }
    }, 1000);
  }
});
</script>