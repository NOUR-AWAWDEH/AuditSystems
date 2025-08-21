<template>
  <CustomSelect
    :model-value="selectedRiskId"
    :options="risks"
    label="Select Risk"
    placeholder="Choose an Risk"
    value-key="id"
    label-key="riskName"
    :error="riskError"
    @update:modelValue="handleRiskChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  risks: {
    type: Array,
    default: () => []
  },
  selectedRiskId: { 
    type: String,
    default: ''
  },
  riskError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedRisk', 'risk-changed']);

const handleRiskChange = (value) => {  
    emit('update:selectedRisk', value);
    emit('risk-changed', value);
};
</script>