<template>
  <input
    :id="id"
    :type="type"
    :value="modelValue"
    @input="$emit('update:modelValue', $event.target.value)"
    :class="inputClass"
    :placeholder="placeholder"
    :disabled="disabled"
  />
</template>

<script setup>

defineProps({
  id: {
    type: String,
    required: true,
  },
  type: {
    type: String,
    default: 'text',
  },
  modelValue: {
    type: String,
    default: '',
  },
  placeholder: {
    type: String,
    default: '',
  },
  disabled: {
    type: Boolean,
    default: false,
  },
  inputClass: {
    type: String,
    default: 'w-full px-4 py-2 text-secondary-shade border border-secondary-base rounded-md focus:outline-none focus:ring-2 disabled:bg-gray-tint',
  },
});

defineEmits(['update:modelValue']);
</script>
