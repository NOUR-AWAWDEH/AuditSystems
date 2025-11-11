<template>
    <!--RiskControlMatrixSelector.vue-->
  <CustomSelect
    :model-value="selectedRiskControlMatrixId"
    :options="riskControlMatrices"
    label="Select Risk Control Matrix"
    placeholder="Choose a risk control matrix"
    value-key="id"
    label-key="description"
    :error="riskControlMatrixError"
    @update:modelValue="handleRiskControlMatrixChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  riskControlMatrices: {
    type: Array,
    default: () => []
  },
  selectedRiskControlMatrixId: { 
    type: String,
    default: ''
  },
  riskControlMatrixError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedRiskControlMatrix', 'risk-control-matrix-changed']);

const handleRiskControlMatrixChange = (value) => {  
    emit('update:selectedRiskControlMatrix', value);
    emit('risk-control-matrix-changed', value);
};
</script>