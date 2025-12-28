<template>
    <!--RecommendationSelector.vue-->
  <CustomSelect
    :model-value="selectedRecommendationId"
    :options="recommendations"
    label="Select Recommendation"
    placeholder="Choose an Recommendation"
    value-key="id"
    label-key="title"
    :error="recommendationError"
    @update:modelValue="handleRecommendationChange"
  />
</template>

<script setup>
import CustomSelect from '@/components/common/CustomSelect.vue';

defineProps({
  recommendations: {
    type: Array,
    default: () => []
  },
  selectedRecommendationId: { 
    type: String,
    default: ''
  },
  recommendationError: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:selectedRecommendation', 'recommendation-changed']);

const handleRecommendationChange = (value) => {  
    emit('update:selectedRecommendation', value);
    emit('recommendation-changed', value);
};
</script>