<template>
  <!--CompanySelector.vue-->
  <CustomSelect
    :model-value="selectedCompanyId"
    :options="companies"
    label="Select Company"
    placeholder="Choose a company"
    value-key="id"
    label-key="name"
    :error="companyError"
    @update:modelValue="handleCompanyChange"
  />
</template>

<script setup>
import CustomSelect from "@/components/common/CustomSelect.vue";

defineProps({
  companies: {
    type: Array,
    default: () => [],
  },
  selectedCompanyId: {
    type: String,
    default: "",
  },
  companyError: {
    type: String,
    default: "",
  },
});

const emit = defineEmits(["update:selectedCompany", "company-changed"]);

const handleCompanyChange = (value) => {
  emit("update:selectedCompany", value);
  emit("company-changed", value);
};
</script>