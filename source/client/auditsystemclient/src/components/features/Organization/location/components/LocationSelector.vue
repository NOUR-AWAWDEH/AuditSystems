<template>
  <CustomSelect
    :model-value="selectedLocation"
    :options="locations"
    label="Select Location"
    placeholder="Choose a location"
    value-key="id"
    label-key="name"
    :error="locationError"
    @update:modelValue="handleLocationChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

const props = defineProps({
  locations: {
    type: Array,
    default: () => []
  },
  selectedLocation: {
    type: String,
    default: ''
  },
  locationError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedLocation', 'location-changed']);

const handleLocationChange = (value) => {
  emit('update:selectedLocation', value); // for v-model
  emit('location-changed', value); // if you want custom logic
};
</script>