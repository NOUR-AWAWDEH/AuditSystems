<template>
  <CustomSelect
    :model-value="selectedAuditProcessId"
    :options="auditProcesses"
    label="Select Process"
    placeholder="Choose a process"
    value-key="id"
    label-key="processName"
    :error="auditProcessError"
    @update:modelValue="handleAuditProcessChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  auditProcesses: {
    type: Array,
    default: () => []
  },
  selectedAuditProcessId: { 
    type: String,
    default: ''
  },
  auditProcessError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedAuditProcess', 'audit-process-changed']);

const handleAuditProcessChange = (value) => {  
    emit('update:selectedAuditProcess', value);
    emit('audit-process-changed', value);
};
</script>