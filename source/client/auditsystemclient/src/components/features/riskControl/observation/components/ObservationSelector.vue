<template>
    <!--ObservationSelector.vue-->
  <CustomSelect
    :model-value="selectedObservationId"
    :options="observations"
    label="Select Observation"
    placeholder="Choose an observation"
    value-key="id"
    label-key="particular"
    :error="observationError"
    @update:modelValue="handleObservationChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  observations: {
    type: Array,
    default: () => []
  },
  selectedObservationId: { 
    type: String,
    default: ''
  },
  observationError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedObservation', 'observation-changed']);

const handleObservationChange = (value) => {  
    emit('update:selectedObservation', value);
    emit('observation-changed', value);
};
</script>