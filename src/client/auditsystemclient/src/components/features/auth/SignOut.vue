<script setup>
import { onMounted } from 'vue';
import { useRouter } from 'vue-router'; // Import useRouter
import { useAuth } from '@/composables/useAuth';

const { logout } = useAuth();
const router = useRouter(); // Initialize router

onMounted(async () => {
  try {
    await logout();
  } catch (error) {
    console.error("Logout failed:", error);
    // Handle logout error if necessary, maybe show a message
  } finally {
    // Redirect to login after logout attempt (success or fail)
    router.push('/login'); // Use router.push for navigation
  }
});
</script>

<template>

  <div v-if="loginSuccessMessage" class="mt-4 mb-4 p-3 bg-soft-success text-success rounded">
    {{ loginSuccessMessage }}
  </div>

  <div class="flex items-center justify-center min-h-screen">
    <div class="text-center">
      <h1 class="text-2xl font-bold mb-4">Logging out...</h1>
      <p>Please wait while we securely log you out.</p>
      <!-- Optional: Add a spinner here -->
    </div>
  </div>
</template>
