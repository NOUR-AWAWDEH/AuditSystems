<template>
  <!--RateSelector.vue-->
  <div>
     <div v-if="props.rates.length > 0">
      <CustomSelect
        :model-value="selectedRateId"
        :options="props.rates"
        :label="props.label"
        :placeholder="props.placeholder"
        value-key="id"
        label-key="level"
        :error="props.error"
        @update:modelValue="handleRateChange"
      />
    </div>
    <div v-else-if="props.error" class="text-error text-sm mt-1">{{ props.error }}</div> 
  </div>
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

const props = defineProps({
  rates: {
    type: Array,
    default: () => []
  },
  selectedRateId: { 
    type: [String, Number],
    default: ''
  },
  label: {
    type: String,
    default: 'Rate'
  },
  placeholder: {
    type: String,
    default: 'Select a rate'
  },
  error: {
    type: String,
    default: ''
  }
});


const emit = defineEmits(['update:selectedRateId', 'rate-changed']);

const handleRateChange = (value) => {  
  emit('update:selectedRateId', value);
  emit('rate-changed', value);
};
</script>