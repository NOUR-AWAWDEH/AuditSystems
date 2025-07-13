<template>
  <div class="custom-select" :class="{ 'is-open': isOpen }">
    <label v-if="label" :for="id" class="block text-sm font-semibold text-secondary-base mb-2">
      <svg class="inline w-4 h-4 mr-1 text-secondary-base" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 9l4-4 4 4m0 6l-4 4-4-4"></path>
      </svg>
      {{ label }}
    </label>
    
    <div class="select-wrapper relative">
      <select
        :id="id"
        :value="modelValue"
        :disabled="disabled"
        :required="required"
        @change="handleChange"
        @focus="handleFocus"
        @blur="handleBlur"
        class="w-full px-4 py-3 pr-10 border border-black rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all duration-200 hover:border-gray-400 appearance-none bg-white text-gray-900 text-sm font-medium cursor-pointer"
      >
        <option v-if="placeholder" value="" disabled class="text-gray-500">
          {{ placeholder }}
        </option>
        <option
          v-for="option in options"
          :key="getOptionValue(option)"
          :value="getOptionValue(option)"
          class="py-2 px-4 text-gray-900 hover:bg-blue-50 focus:bg-blue-50"
        >
          {{ getOptionLabel(option) }}
        </option>
      </select>
      
      <div class="absolute right-3 top-1/2 transform -translate-y-1/2 pointer-events-none">
        <svg 
          class="w-5 h-5 text-gray-400 transition-transform duration-200" 
          :class="{ 'rotate-180': isOpen }"
          fill="none" 
          stroke="currentColor" 
          viewBox="0 0 24 24"
        >
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7"></path>
        </svg>
      </div>
    </div>
    
    <div v-if="error" class="flex items-center mt-2 text-sm text-red-600">
      <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
      </svg>
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
@reference "tailwindcss";

.custom-select {
  @apply relative;
}

.select-wrapper {
  @apply relative;
}

.custom-select select:disabled {
  @apply bg-gray-100 cursor-not-allowed opacity-60;
}

.custom-select select:focus {
  @apply ring-2 ring-blue-500/50;
}

/* Custom dropdown arrow animation */
.custom-select.is-open .select-wrapper svg {
  @apply rotate-180;
}

/* Better option styling for browsers that support it */
.custom-select select option {
  @apply py-2 px-4;
}

.custom-select select option:hover {
  @apply bg-blue-50;
}
</style>