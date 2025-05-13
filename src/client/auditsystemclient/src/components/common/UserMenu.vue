<script setup>
import { useLogout } from '@/composables/auth/useLogout'; // Adjust path as needed
import { ref, onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';

// Initialize composables
const { logout } = useLogout();
const router = useRouter();

// Reactive states
const isLoggingOut = ref(false);
const isDropdownOpen = ref(false); // Fixed double ref issue
const menuRef = ref(null);

// Toggle dropdown visibility
const toggleDropdown = () => {
  isDropdownOpen.value = !isDropdownOpen.value;
};

// Close dropdown when clicking outside
const handleClickOutside = (event) => {
  if (menuRef.value && !menuRef.value.contains(event.target)) {
    isDropdownOpen.value = false;
  }
};

// Setup click outside listener
onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside);
});

// Handle logout
const handleLogout = async () => {
  isLoggingOut.value = true;
  isDropdownOpen.value = false; // Close dropdown on logout attempt
  try {
    const response = await logout();
    if (response.success) {
      router.push('/login');
    } else {
      console.error('Logout failed:', response.message);
    }
  } catch (err) {
    console.error('Logout error:', err);
  } finally {
    isLoggingOut.value = false;
  }
};

// Navigate to profile page
const goToProfile = () => {
  isDropdownOpen.value = false;
  router.push('/profile');
};
</script>

<template>
  <div class="relative" ref="menuRef">
    <!-- User icon button to toggle dropdown -->
    <button
      @click="toggleDropdown"
      class="p-2 rounded-full text-white focus:outline-none duration-200"
      :class="{ 'bg-gray-200': isDropdownOpen }"
      aria-label="User menu"
      title="User menu"
      :disabled="isLoggingOut"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
        stroke-width="1.5"
        stroke="currentColor"
        class="size-7"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M15.75 6a3.75 3.75 0 1 1-7.5 0 3.75 3.75 0 0 1 7.5 0ZM4.501 20.118a7.5 7.5 0 0 1 14.998 0A17.933 17.933 0 0 1 12 21.75c-2.676 0-5.216-.584-7.499-1.632Z"
        />
      </svg>
    </button>

    <!-- Dropdown menu - changed from right-0 to left-0 -->
    <div
      v-if="isDropdownOpen"
      class="absolute left-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 z-50 border border-gray-200"
    >
      <button
        @click="goToProfile"
        class="block w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 transition-colors duration-150"
      >
        <div class="flex items-center">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-7 mr-2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M17.982 18.725A7.488 7.488 0 0 0 12 15.75a7.488 7.488 0 0 0-5.982 2.975m11.963 0a9 9 0 1 0-11.963 0m11.963 0A8.966 8.966 0 0 1 12 21a8.966 8.966 0 0 1-5.982-2.275M15 9.75a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
          </svg>
          Profile
        </div>
      </button>

      <div class="border-t border-gray-100 my-1"></div>

      <button
        @click="handleLogout"
        class="block w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 transition-colors duration-150"
        :disabled="isLoggingOut"
      >
        <div class="flex items-center">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5 mr-2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 9V5.25A2.25 2.25 0 0 0 13.5 3h-6a2.25 2.25 0 0 0-2.25 2.25v13.5A2.25 2.25 0 0 0 7.5 21h6a2.25 2.25 0 0 0 2.25-2.25V15M12 9l-3 3m0 0 3 3m-3-3h12.75" />
          </svg>
          <span v-if="isLoggingOut">Logging out...</span>
          <span v-else>Logout</span>
        </div>
      </button>
    </div>
  </div>
</template>

<style scoped>
button {
  transition: all 0.2s ease;
}

/* Ensure dropdown stays above other elements */
.relative {
  position: relative;
}

/* Dropdown animation */
.absolute {
  animation: dropdown 0.2s ease-in-out;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
}

@keyframes dropdown {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
