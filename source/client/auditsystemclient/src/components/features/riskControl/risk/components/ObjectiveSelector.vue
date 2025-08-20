<template>
  <CustomSelect
    :model-value="selectedObjectiveId"
    :options="objectives"
    label="Select Objective"
    placeholder="Choose an objective"
    value-key="id"
    label-key="description"
    :error="objectiveError"
    @update:modelValue="handleObjectiveChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  objectives: {
    type: Array,
    default: () => []
  },
  selectedObjectiveId: { 
    type: String,
    default: ''
  },
  objectiveError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedObjective', 'objective-changed']);

const handleObjectiveChange = (value) => {  
    emit('update:selectedObjective', value);
    emit('objective-changed', value);
};
</script>