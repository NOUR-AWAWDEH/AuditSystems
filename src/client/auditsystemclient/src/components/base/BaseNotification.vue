<template>
  <div v-if="notification.show" :class="['fixed top-5 right-5 p-4 rounded-md shadow-lg z-50', notificationTypeClass]">
    <div class="flex items-center">
      <svg v-if="notification.type === 'success'" class="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
      <svg v-if="notification.type === 'error'" class="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
      <svg v-if="notification.type === 'warning'" class="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"></path></svg>
      <svg v-if="notification.type === 'info'" class="w-6 h-6 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
      <span>{{ notification.message }}</span>
    </div>
    <button @click="closeNotification" :class="['absolute top-1 right-1', closeButtonClass]">
      <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path></svg>
    </button>
  </div>
</template>

<script setup>
import { computed } from 'vue';
import { useNotification } from '@/composables/notification/useNotification'; // Path will be created next

const { notification, hideNotification } = useNotification();

const notificationTypeClass = computed(() => {
  switch (notification.value.type) {
    case 'success':
      return 'bg-[var(--color-soft-success)] text-[var(--color-success)]';
    case 'error':
      return 'bg-[var(--color-soft-error)] text-[var(--color-error)]';
    case 'warning':
      return 'bg-[var(--color-tertiary-tint)] text-[var(--color-tertiary-shade)]'; // Using tertiary for warning
    case 'info':
      return 'bg-[var(--color-primary-tint)] text-[var(--color-primary-base)]'; // Using primary for info
    default:
      return 'bg-[var(--color-gray-tint)] text-[var(--color-gray-base)]';
  }
});

const closeButtonClass = computed(() => {
  switch (notification.value.type) {
    case 'success':
      return 'text-[var(--color-success)] hover:text-[var(--color-success)]'; // Adjusted hover for consistency
    case 'error':
      return 'text-[var(--color-error)] hover:text-[var(--color-error)]';
    case 'warning':
      return 'text-[var(--color-tertiary-shade)] hover:text-[var(--color-tertiary-shade)]';
    case 'info':
      return 'text-[var(--color-primary-base)] hover:text-[var(--color-primary-base)]';
    default:
      return 'text-[var(--color-gray-base)] hover:text-[var(--color-gray-base)]';
  }
});

const closeNotification = () => {
  hideNotification();
};

</script>

<style scoped>
/* Add any specific styles if needed */
</style>
