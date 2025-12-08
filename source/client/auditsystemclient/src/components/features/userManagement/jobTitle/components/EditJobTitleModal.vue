<template>
  <!--EditJobTitle.vue-->
  <div
    v-if="isVisible"
    class="fixed inset-0 bg-gray-base bg-opacity-50 flex items-center justify-center z-50"
  >
    <div class="bg-white rounded-lg p-6 w-full max-w-md mx-4">
      <h2 class="text-xl font-semibold mb-4 text-secondary-shade">Edit Job Title</h2>

      <form @submit.prevent="handleSubmit">
        <div class="mb-4">
          <label for="edit-name" class="block text-sm font-medium text-secondary-base mb-2">
            Job Title Name
          </label>
          <BaseInput
            id="edit-name"
            v-model="formData.name"
            type="text"
            placeholder="Enter job title name"
            :input-class="'w-full px-3 py-2 border border-gray-tint rounded-md focus:outline-none focus:ring-2 focus:ring-secondary-base text-secondary-shade'"
          />
        </div>

        <div class="flex justify-end space-x-3">
          <BaseButton
            @click="$emit('close')"
            :disabled="isUpdating"
            class="px-4 py-2 text-white border border-error rounded-md hover:bg-red-800 bg-error"
          >
            Cancel
          </BaseButton>
          <BaseButton
            @click="handleSubmit"
            :disabled="isUpdating || !formData.name.trim() || (!jobTitle?.companyId && !companyId)"
            :is-loading="isUpdating"
            class="px-4 py-2 bg-primary-base text-white rounded-md hover:bg-primary-shade disabled:opacity-50"
          >
            <template #loading>Updating...</template>
            Update Job Title
          </BaseButton>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from "vue";
import BaseInput from "@/components/base/BaseInput.vue";
import BaseButton from "@/components/base/BaseButton.vue";
import { useUpdateJobTitleById } from "@/composables/organization/jobTitle/useUpdateJobTitleById";

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false,
  },
  jobTitle: {
    type: Object,
    default: null,
  },
  companyId: {
    type: String,
    default: "",
  },
});

const emit = defineEmits(["close", "job-title-updated"]);

const { isUpdating, updateJobTitle } = useUpdateJobTitleById();

const formData = ref({
  name: "",
});

watch(
  () => props.jobTitle,
  (newJobTitle) => {
    if (newJobTitle) {
      formData.value = {
        name: newJobTitle.name || "",
      };
    }
  },
  { immediate: true }
);

const handleSubmit = async () => {
  if (!props.jobTitle?.id) return;

  const result = await updateJobTitle({
    id: props.jobTitle.id,
    name: formData.value.name.trim(),
    companyId: props.jobTitle.companyId || props.companyId,
  });

  if (result.success) {
    emit("job-title-updated", {
      ...props.jobTitle,
      name: formData.value.name.trim(),
    });

    emit("close");
  }
};
</script>

<style scoped>
/* Enhanced mobile responsiveness matching EditLocationModal.vue */
@media (max-width: 640px) {
  .mx-4 {
    margin-left: 0.75rem;
    margin-right: 0.75rem;
  }

  .p-6 {
    padding: 1rem;
  }
}

@media (max-width: 480px) {
  .mx-4 {
    margin-left: 0.5rem;
    margin-right: 0.5rem;
  }

  .p-6 {
    padding: 0.75rem;
  }
}
</style>
