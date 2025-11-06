<template>
    <!--AuditProgramSelector.vue-->
  <CustomSelect
    :model-value="selectedAuditProgramId"
    :options="auditPrograms"
    label="Select Audit Program"
    placeholder="Choose an audit program"
    value-key="id"
    label-key="description"
    :error="auditProgramError"
    @update:modelValue="handleAuditProgramChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  auditPrograms: {
    type: Array,
    default: () => []
  },
  selectedAuditProgramId: { 
    type: String,
    default: ''
  },
  auditProgramError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedAuditProgram', 'audit-program-changed']);

const handleAuditProgramChange = (value) => {  
    emit('update:selectedAuditProgram', value);
    emit('audit-program-changed', value);
};
</script>