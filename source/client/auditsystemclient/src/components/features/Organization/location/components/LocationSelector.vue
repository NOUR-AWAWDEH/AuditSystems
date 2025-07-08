<template>
  <CustomSelect
    v-model="selectedLocation"
    :options="locations"
    label="Select Location"
    placeholder="Choose a location"
    value-key="id"
    label-key="name"
    :error="locationError"
    @change="handleLocationChange"
  />
</template>

<script setup>
import { ref, watch } from 'vue';
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

const emit = defineEmits(['location-changed']);

const selectedLocation = ref(props.selectedLocation);

// Watch for external changes to selectedLocation prop
watch(() => props.selectedLocation, (newValue) => {
  selectedLocation.value = newValue;
});

const handleLocationChange = (value) => {
  selectedLocation.value = value;
  emit('location-changed', value);
};
</script>