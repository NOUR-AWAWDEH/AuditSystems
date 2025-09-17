<template>
  <div class="relative">
    <BaseInput
      :id="props.id"
      :type="showPassword ? 'text' : 'password'"
      :modelValue="props.modelValue"
      @update:modelValue="$emit('update:modelValue', $event)"
      :placeholder="props.placeholder"
      :disabled="props.disabled"
      :inputClass="inputClasses" class="pr-10 transition-all duration-200"
    />
    <button
      type="button"
      class="absolute inset-y-0 right-0 pr-3 flex items-center"
      @click="togglePasswordVisibility"
      aria-label="Toggle password visibility"
    >
      <svg
        class="h-5 w-5 text-gray-base transition-colors duration-200"
        :class="{ 'text-secondary-tint': showPassword }"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
        stroke-width="2"
        stroke-linecap="round"
        stroke-linejoin="round"
      >
        <path
          v-if="showPassword"
          d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"
        />
        <circle v-if="showPassword" cx="12" cy="12" r="3" />
        <path
          v-if="!showPassword"
          d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"
        />
        <line v-if="!showPassword" x1="1" y1="1" x2="23" y2="23" />
      </svg>
    </button>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import BaseInput from './BaseInput.vue';

const props = defineProps({
  id: {
    type: String,
    required: true,
  },
  modelValue: {
    type: String,
    default: '',
  },
  placeholder: {
    type: String,
    default: 'Enter your password',
  },
  disabled: {
    type: Boolean,
    default: false,
  },
  inputClass: {
    type: String,
    default: '', // Remove default here to allow more flexibility in the parent
  },
});

const showPassword = ref(false);

const togglePasswordVisibility = () => {
  showPassword.value = !showPassword.value;
};

const inputClasses = computed(() => {
  return `w-full px-4 py-2 text-secondary-shade border border-secondary-base rounded-md focus:outline-none focus:ring-2 disabled:bg-gray-tint ${props.inputClass}`;
});
</script>

<style scoped>
/* Ensure the input has padding to accommodate the toggle button - handled by pr-10 now */
</style>
