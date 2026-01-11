<template>
  <!--EditRecommendationModal.vue-->
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 z-50 flex items-center justify-center p-4">
        <div
          class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity"
          @click="$emit('close')"
        ></div>

        <!-- Modal -->
        <div
          class="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all"
        >
          <!-- Header -->
          <div class="flex items-center justify-between p-4 border-b border-gray-tint">
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Edit Recommendation</h3>
            <button
              @click="$emit('close')"
              class="text-gray-base hover:text-gray-shade focus:outline-none"
            >
              <CloseIcon size="md" custom-class="text-gray-400 hover:text-gray-600" />
            </button>
          </div>

          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">

              <!-- Recommendation Description -->
              <div>
                <label for="recommendation-description" class="block text-sm font-medium text-gray-700">
                  Recommendation Description *
                </label>
                <BaseInput
                  id="recommendation-description"
                  v-model="formData.description"
                  placeholder="Enter Recommendation Description"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                    errors.description ? 'border-error' : 'border-gray-tint'
                  } focus:ring-primary-base`"
                />
                <p v-if="errors.description" class="text-error text-sm mt-1">
                  {{ errors.description }}
                </p>
              </div>

              <!-- API Error -->
              <div v-if="error" class="p-3 bg-soft-error border border-error rounded-md">
                <p class="text-error text-sm">{{ error }}</p>
              </div>

              <!-- Action Buttons -->
              <div class="flex justify-end space-x-3 pt-4">
                <button
                  type="button"
                  @click="$emit('close')"
                  :disabled="isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-error border border-gray-tint rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  Cancel
                </button>

                <button
                  type="submit"
                  :disabled="isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  <span v-if="isUpdating">Updating...</span>
                  <span v-else>Update Recommendation</span>
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { onMounted, ref } from "vue";
import BaseInput from "@/components/base/BaseInput.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import { useUpdateRecommendationById } from "@/composables/riskControl/recommendation/useUpdateRecommendationById";
import { useNotification } from "@/composables/notification/useNotification";

const props = defineProps({
  isVisible: { type: Boolean, default: false },
  recommendation: { type: Object, default: () => ({}) },
});

const emit = defineEmits(["close", "recommendation-updated"]);

const { showNotification } = useNotification();
const { isUpdating, error, updateRecommendation, resetState } = useUpdateRecommendationById();

const formData = ref({
  description: "",
});

const errors = ref({
  description: "",
});

onMounted(async () => {
  resetForm();
});

const resetForm = () => {
  formData.value = { description: "" };
  errors.value = { description: "" };
  resetState();
};

const validateForm = () => {
  let isValid = true;
  errors.value = { description: "" };
  if (!formData.value.description.trim()) {
    errors.value.description = "Recommendation description is required";
    isValid = false;
  }

  return isValid;
};

const handleSubmit = async () => {
  if (!props.recommendation.id) {
    showNotification("Recommendation is required", "error");
    return;
  }
  if (!props.recommendation.observationId) {
    showNotification("Observation is required", "error");
  }

  if (!validateForm()) return;

  try {
    const recommendationData = {
      id: props.recommendation.id,
      description: formData.value.description.trim(),
      observationId: props.recommendation.observationId,

      createdAt: props.recommendation.createdAt,
    };

    const result = await updateRecommendation(recommendationData);

    if (result?.success) {
      const updatedRecommendationData = {
        ...recommendationData,
        ...result.data,
      };

      emit("recommendation-updated", updatedRecommendationData);
      emit("close");
    } else {
      showNotification(result?.message || "Failed to update Recommendation", "error");
    }
  } catch (err) {
    console.error("Error updating Recommendation:", err);
    showNotification("Failed to update Recommendation. Please try again.", "error");
  }
};
</script>