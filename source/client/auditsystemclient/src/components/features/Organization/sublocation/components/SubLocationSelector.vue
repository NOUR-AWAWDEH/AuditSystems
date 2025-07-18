<template>
  <CustomSelect
    :model-value="selectedSubLocationId"
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
  selectedSubLocationId: { 
    type: String,
    default: ''
  },
  subLocationError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selected-sub-location', 'sub-location-changed']);

const handleSubLocationChange = (value) => {  
    emit('update:selected-sub-location', value);
    emit('sub-location-changed', value);
};
</script>