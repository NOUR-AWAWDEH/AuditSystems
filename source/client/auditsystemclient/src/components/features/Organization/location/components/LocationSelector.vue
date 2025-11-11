<template>
    <!--LocationSelector.vue-->
  <CustomSelect
    :model-value="selectedLocationId"
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

defineProps({
  locations: {
    type: Array,
    default: () => []
  },
  selectedLocationId: {
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
  emit('update:selectedLocation', value);
  emit('location-changed', value);
};
</script>