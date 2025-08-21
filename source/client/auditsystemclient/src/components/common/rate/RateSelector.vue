<template>
  <CustomSelect
    :model-value="selectedRateId"
    :options="rates"
    :label="label"
    :placeholder="placeholder"
    value-key="id"
    label-key="riskName"
    :error="error"
    @update:modelValue="handleRateChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  rates: {
    type: Array,
    default: () => []
  },
  selectedRateId: { 
    type: [String, Number],
    default: ''
  },
  label: {
    type: String,
    default: 'Rate'
  },
  placeholder: {
    type: String,
    default: 'Select a rate'
  },
  error: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedRateId', 'rate-changed']);

const handleRateChange = (value) => {  
  emit('update:selectedRateId', value);
  emit('rate-changed', value);
};
</script>