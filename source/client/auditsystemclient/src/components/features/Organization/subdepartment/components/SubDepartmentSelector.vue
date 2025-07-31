<template>
  <CustomSelect
    :model-value="selectedSubDepartmentId"
    :options="subDepartments"
    label="Select Sub Department"
    placeholder="Choose a sub department"
    value-key="id"
    label-key="name"
    :error="subDepartmentError"
    @update:modelValue="handleSubDepartmentChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  subDepartments: {
    type: Array,
    default: () => []
  },
  selectedSubDepartmentId: { 
    type: String,
    default: ''
  },
  subDepartmentError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedSubDepartment', 'sub-department-changed']);

const handleSubDepartmentChange = (value) => {  
    emit('update:selectedSubDepartment', value);
    emit('sub-department-changed', value);
};
</script>