<template>
  <div
    class="fixed z-50 top-20 right-5 2xl:right-20 w-[80%] sm:w-[70%] md:w-[50%] max-w-lg flex flex-col gap-3"
  >
    <div
      v-for="n in notifications"
      :key="n.id"
      :class="[
        'p-4 rounded-md shadow-lg transition-all relative flex items-center',
        notificationTypeClass(n),
      ]"
    >
      <!-- Icons -->
      <!-- Success -->
      <svg
        v-if="n.type === 'success'"
        class="w-8 h-8 mr-3 shrink-0"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
        />
      </svg>

      <!-- Error -->
      <svg
        v-if="n.type === 'error'"
        class="w-8 h-8 mr-3 shrink-0"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
        />
      </svg>

      <!-- Warning -->
      <svg
        v-if="n.type === 'warning'"
        class="w-8 h-8 mr-3 shrink-0"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
        />
      </svg>

      <!-- Info -->
      <svg
        v-if="n.type === 'info'"
        class="w-8 h-8 mr-3 shrink-0"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
        />
      </svg>

      <span>{{ n.message }}</span>

      <button @click="close(n.id)" class="absolute top-1 right-1">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M6 18L18 6M6 6l12 12"
          />
        </svg>
      </button>
    </div>
  </div>
</template>

<script setup>
import { useNotification } from "@/composables/notification/useNotification";

const { notifications, hideNotification } = useNotification();

const notificationTypeClass = (n) => {
  switch (n.type) {
    case "success":
      return "bg-[var(--color-soft-success)] text-[var(--color-success)]";
    case "error":
      return "bg-[var(--color-soft-error)] text-[var(--color-error)]";
    case "warning":
      return "bg-[var(--color-tertiary-tint)] text-[var(--color-tertiary-shade)]";
    case "info":
      return "bg-[var(--color-primary-tint)] text-[var(--color-primary-base)]";
    default:
      return "bg-[var(--color-gray-tint)] text-[var(--color-gray-base)]";
  }
};

const close = (id) => hideNotification(id);
</script>

<style scoped></style>
