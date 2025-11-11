<template>
  <!--ErrorDisplay.vue-->
  <transition name="fade">
    <div
      v-if="errors.length && isVisible"
      class="mb-4 p-3 bg-soft-error text-error rounded-md w-full"
    >
      <ul class="list-none pl-5" >
        <li v-for="(err, index) in errors" :key="index">{{ err }}</li>
      </ul>
    </div>
  </transition>
</template>

<script setup>

defineProps({
  errors: {
    type: Array,
    required: true,
  },
  isVisible: {
    type: Boolean,
    required: true,
  },
});
</script>

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
