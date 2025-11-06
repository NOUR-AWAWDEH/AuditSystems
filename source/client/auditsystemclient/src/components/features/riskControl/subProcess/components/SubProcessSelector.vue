<template>
    <!--SubProcessSelector.vue-->
  <CustomSelect
    :model-value="selectedSubProcessId"
    :options="subProcesses"
    label="Select SubProcess"
    placeholder="Choose a subprocess"
    value-key="id"
    label-key="particular"
    :error="subProcessError"
    @update:modelValue="handleSubProcessChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  subProcesses: {
    type: Array,
    default: () => []
  },
  selectedSubProcessId: { 
    type: String,
    default: ''
  },
  subProcessError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedSubProcess', 'sub-process-changed']);

const handleSubProcessChange = (value) => {  
    emit('update:selectedSubProcess', value);
    emit('sub-process-changed', value);
};
</script>