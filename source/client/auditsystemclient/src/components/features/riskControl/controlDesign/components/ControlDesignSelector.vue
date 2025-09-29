<template>
  <CustomSelect
    :model-value="selectedControlDesignId"
    :options="controlDesigns"
    label="Select Control Design"
    placeholder="Choose a control design"
    value-key="id"
    label-key="description"
    :error="controlDesignError"
    @update:modelValue="handleControlDesignChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  controlDesigns: {
    type: Array,
    default: () => []
  },
  selectedControlDesignId: { 
    type: String,
    default: ''
  },
  controlDesignError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedControlDesign', 'control-design-changed']);

const handleControlDesignChange = (value) => {  
    emit('update:selectedControlDesign', value);
    emit('control-design-changed', value);
};
</script>