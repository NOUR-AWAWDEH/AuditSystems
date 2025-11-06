<template>
  <!--DepartmentSelector.vue-->
  <CustomSelect
    :model-value="selectedDepartmentId"
    :options="departments"
    label="Select Department"
    placeholder="Choose a department"
    value-key="id"
    label-key="name"
    :error="departmentError"
    @update:modelValue="handleDepartmentChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  departments: {
    type: Array,
    default: () => []
  },
  selectedDepartmentId: { 
    type: String,
    default: ''
  },
  departmentError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedDepartment', 'department-changed']);

const handleDepartmentChange = (value) => {  
    emit('update:selectedDepartment', value);
    emit('department-changed', value);
};
</script>