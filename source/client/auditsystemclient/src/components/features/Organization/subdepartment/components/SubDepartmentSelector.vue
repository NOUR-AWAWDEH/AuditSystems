<template>
  <CustomSelect
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    :options="options"
    :label="label"
    :placeholder="placeholder"
    :required="required"
    :error="error"
    :disabled="isLoading"
    value-key="id"
    label-key="name"
    :emit-object="emitObject"
    class="mt-4"
  />
</template>

<script setup>
import { computed, onMounted } from "vue";
import CustomSelect from "@/components/common/CustomSelect.vue";
import { useGetSubDepartmentsByUserId } from "@/composables/organization/subdepartment/useGetSubDepartmentsByUserId";

const props = defineProps({
  modelValue: {
    type: [String, Number, Object],
    required: false,
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
  // New props for reusability
  subDepartments: {
    type: Array,
    default: null, // If null, we'll try to fetch internal data
  },
  autoFetch: {
    type: Boolean,
    default: true,
  },
  emitObject: {
    type: Boolean,
    default: false,
  },
});

defineEmits(["update:modelValue"]);

const {
  subDepartments: internalSubDepartments,
  getSubDepartmentsByUserId,
  isLoading: isInternalLoading,
  error: internalError,
} = useGetSubDepartmentsByUserId();

// If external options are provided, use them. Otherwise use internal state.
const options = computed(() => {
  if (props.subDepartments !== null) {
    return props.subDepartments;
  }
  return internalSubDepartments.value;
});

const isLoading = computed(() => {
  if (props.subDepartments !== null) {
    return false; // Or passed prop if we want to support external loading state
  }
  return isInternalLoading.value;
});

const error = computed(() => {
  if (props.subDepartments !== null) {
    return null; // External error handling not implemented here yet
  }
  return internalError.value;
});

onMounted(() => {
  // Only fetch if no external data provided AND autoFetch is true
  if (props.subDepartments === null && props.autoFetch) {
    getSubDepartmentsByUserId();
  }
});
</script>
