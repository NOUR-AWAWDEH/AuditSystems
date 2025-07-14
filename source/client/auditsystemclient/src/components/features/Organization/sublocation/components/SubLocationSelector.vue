<template>
  <CustomSelect
    :model-value="selectedSubLocation"
    :options="subLocations"
    label="Select Sub Location"
    placeholder="Choose a sub location"
    value-key="id"
    label-key="name"
    :error="subLocationError"
    @update:modelValue="handleSubLocationChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  subLocations: {
    type: Array,
    default: () => []
  },
  selectedSubLocation: { 
    type: String,
    default: ''
  },
  subLocationError: {
    type: String,
    default: null
  }
});

// ✅ Correct event names
const emit = defineEmits(['update:selectedSubLocation', 'subLocation-changed']);

const handleSubLocationChange = (value) => {
  emit('update:selectedSubLocation', value); 
  emit('subLocation-changed', value);
};
</script>