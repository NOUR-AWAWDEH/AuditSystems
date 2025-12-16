<template>
  <CustomSelect
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    :options="jobTitles"
    :label="label"
    :placeholder="placeholder"
    :required="required"
    :error="error"
    :disabled="isLoading"
    value-key="id"
    label-key="name"
    class="mt-4"
  />
</template>

<script setup>
import { onMounted } from "vue";
import CustomSelect from "@/components/common/CustomSelect.vue";
import { useGetJobTitlesByUserId } from "@/composables/organization/jobTitle/useGetJobTitlesByUserId";

defineProps({
  modelValue: {
    type: [String, Number, Object],
    required: true,
  },
  label: {
    type: String,
    default: "Job Title",
  },
  placeholder: {
    type: String,
    default: "Select Job Title",
  },
  required: {
    type: Boolean,
    default: false,
  },
});

defineEmits(["update:modelValue"]);

const { jobTitles, getJobTitlesByUserId, isLoading, error } = useGetJobTitlesByUserId();

onMounted(() => {
  getJobTitlesByUserId();
});
</script>
