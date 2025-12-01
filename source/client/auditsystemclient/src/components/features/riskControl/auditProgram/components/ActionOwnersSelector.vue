<template>
  <!--ActionOwnersSelector.vue-->
  <CustomSelect
    :model-value="selectedActionOwnerId"
    :options="actionOwners"
    label="Select Action Owner"
    placeholder="Choose an action owner"
    value-key="id"
    label-key="fullName"
    :error="actionOwnerError"
    @update:modelValue="handleActionOwnerChange"
  />
</template>

<script setup>
import CustomSelect from "@/components/common/CustomSelect.vue";
import { onMounted } from "vue";
const { actionOwners, selectedActionOwnerId, actionOwnerError } = defineProps({
  actionOwners: {
    type: Array,
    default: () => [],
  },
  selectedActionOwnerId: {
    type: String,
    default: "",
  },
  actionOwnerError: {
    type: String,
    default: "",
  },
});

const emit = defineEmits(["update:selectedActionOwner", "action-owner-changed"]);

const handleActionOwnerChange = (value) => {
  emit("update:selectedActionOwner", value);
  emit("action-owner-changed", value);
};

onMounted(() => {
  console.log("actionOwners", actionOwners);
});
</script>
