<script setup>
defineProps({
  messages: {
    type: Array,
    required: true,
  },
  isVisible: {
    type: Boolean,
    required: true,
  },
});
</script>

<template>
  <transition name="fade">
    <div
      v-if="messages.length && isVisible"
      class="mb-4 p-3 bg-soft-success text-success rounded-md w-full"
    >
      <ul class="list-none pl-5">
        <li v-for="(msg, index) in messages" :key="index">{{ msg }}</li>
      </ul>
    </div>
  </transition>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
