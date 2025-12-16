<template>
  <CustomSelect
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    :options="subDepartments"
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
import { useGetSubDepartmentsByUserId } from "@/composables/organization/subdepartment/useGetSubDepartmentsByUserId";

defineProps({
  modelValue: {
    type: [String, Number, Object],
    required: true,
  },
  label: {
    type: String,
    default: "Sub Department",
  },
  placeholder: {
    type: String,
    default: "Select Sub Department",
  },
  required: {
    type: Boolean,
    default: false,
  },
});

defineEmits(["update:modelValue"]);

const { subDepartments, getSubDepartmentsByUserId, isLoading, error } =
  useGetSubDepartmentsByUserId();

onMounted(() => {
  getSubDepartmentsByUserId();
});
</script>
