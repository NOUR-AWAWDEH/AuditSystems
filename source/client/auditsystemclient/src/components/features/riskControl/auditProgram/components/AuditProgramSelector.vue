<template>
  <CustomSelect
    :model-value="selectedExitControlId"
    :options="exitControls"
    label="Select Exit Control"
    placeholder="Choose an exit control"
    value-key="id"
    label-key="description"
    :error="exitControlError"
    @update:modelValue="handleExitControlChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  exitControls: {
    type: Array,
    default: () => []
  },
  selectedExitControlId: { 
    type: String,
    default: ''
  },
  exitControlError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedExitControl', 'exit-control-changed']);

const handleExitControlChange = (value) => {  
    emit('update:selectedExitControl', value);
    emit('exit-control-changed', value);
};
</script>