<template>
  <CustomSelect
    id="user-role"
    :model-value="modelValue"
    @update:model-value="$emit('update:modelValue', $event)"
    :options="roleOptions"
    label="Role"
    class="mt-4"
    placeholder="e.g. Select Role"
    required
  />
</template>

<script setup>
import CustomSelect from "@/components/common/CustomSelect.vue";

defineProps({
  modelValue: {
    type: String,
    required: false,
  },
});

defineEmits(["update:modelValue"]);

const roleOptions = [
  { label: "Action Owner", value: "ActionOwner" },
  { label: "Admin", value: "Admin" },
];
</script>
