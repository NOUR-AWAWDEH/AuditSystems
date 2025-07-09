<template>
  <div class="custom-select" :class="{ 'is-open': isOpen }">
    <label v-if="label" :for="id" class="select-label">
      {{ label }}
    </label>
    
    <div class="select-wrapper">
      <select
        :id="id"
        :value="modelValue"
        :disabled="disabled"
        :required="required"
        @change="handleChange"
        @focus="handleFocus"
        @blur="handleBlur"
        class="select-input"
      >
        <option v-if="placeholder" value="" disabled>
          {{ placeholder }}
        </option>
        <option
          v-for="option in options"
          :key="getOptionValue(option)"
          :value="getOptionValue(option)"
        >
          {{ getOptionLabel(option) }}
        </option>
      </select>
      
      <div class="select-icon">
        <svg class="w-4 h-4 text-secondary-base" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
        </svg>
      </div>
    </div>
    
    <div v-if="error" class="error-message">
      {{ error }}
    </div>
  </div>
</template>

<script setup>
import { computed, ref } from 'vue';

const props = defineProps({
  modelValue: {
    type: [String, Number, Object],
    default: ''
  },
  options: {
    type: Array,
    required: true
  },
  label: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: 'Select an option'
  },
  disabled: {
    type: Boolean,
    default: false
  },
  required: {
    type: Boolean,
    default: false
  },
  error: {
    type: String,
    default: ''
  },
  valueKey: {
    type: String,
    default: 'value'
  },
  labelKey: {
    type: String,
    default: 'label'
  }
});

const emit = defineEmits(['update:modelValue', 'change', 'focus', 'blur']);

const id = computed(() => `select-${Math.random().toString(36).substr(2, 9)}`);
const isOpen = ref(false);

const getOptionValue = (option) => {
  return typeof option === 'object' ? option[props.valueKey] : option;
};

const getOptionLabel = (option) => {
  return typeof option === 'object' ? option[props.labelKey] : option;
};

const handleChange = (event) => {
  const value = event.target.value;
  emit('update:modelValue', value);
  emit('change', value);
};

const handleFocus = () => {
  isOpen.value = true;
  emit('focus');
};

const handleBlur = () => {
  isOpen.value = false;
  emit('blur');
};
</script>

<style scoped>
@reference "../../styles/style.css";

.custom-select {
  @apply relative;
}

.select-label {
  @apply block text-sm font-medium text-secondary-shade mb-1;
}

.select-wrapper {
  @apply relative;
}

.select-input {
  @apply w-full px-4 py-2 text-secondary-shade border border-secondary-base rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-secondary-base appearance-none bg-white;
}

.select-input:disabled {
  @apply bg-gray-tint cursor-not-allowed;
}

.select-icon {
  @apply absolute right-3 top-1/2 transform -translate-y-1/2 pointer-events-none;
}

.error-message {
  @apply text-sm text-red-600 mt-1;
}
</style>