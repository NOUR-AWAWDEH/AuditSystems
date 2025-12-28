<template>
  <!--AddRecommendationModal.vue-->
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
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Add Recommendation</h3>
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
              <!-- Recommendation Title Field -->
              <div>
                <label for="recommendation-title" class="block text-sm font-medium text-gray-700">
                  Title *
                </label>
                <BaseInput
                  id="recommendation-title"
                  v-model="formData.title"
                  placeholder="Enter Recommendation Title"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                    errors.title ? 'border-error' : 'border-gray-tint'
                  } focus:ring-primary-base`"
                />
                <p v-if="errors.title" class="text-error text-sm mt-1">
                  {{ errors.title }}
                </p>
              </div>

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
                  :disabled="isCreating"
                  class="px-4 py-2 text-sm font-medium text-white bg-error border border-gray-tint rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  Cancel
                </button>

                <button
                  type="submit"
                  :disabled="!formData.title.trim() || isCreating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  <span v-if="isCreating">Adding...</span>
                  <span v-else>Add Recommendation</span>
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
import { ref, watch } from "vue";
import BaseInput from "@/components/base/BaseInput.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import { useCreateRecommendation } from "@/composables/riskControl/recommendation/useCreateRecommendation.js";
import { useNotification } from "@/composables/notification/useNotification";

const props = defineProps({
  isVisible: { type: Boolean, default: false },
  observationId: { type: [String, Number], default: null },
});

const emit = defineEmits(["close", "recommendation-added"]);

const { showNotification } = useNotification();
const { isCreating, error, createRecommendation, resetState } = useCreateRecommendation();

const formData = ref({
  title: "",
  description: "",
});

const errors = ref({
  title: "",
  description: "",
});

watch(
  () => props.isVisible,
  async (newValue) => {
    if (newValue) {
      resetForm();
    }
  }
);

const resetForm = () => {
  formData.value = { title: "", description: "" };

  errors.value = { title: "", description: "" };
  resetState();
};

const validateForm = () => {
  errors.value = { title: "", description: "" };
  let isValid = true;

  if (!formData.value.title.trim()) {
    errors.value.title = "Recommendation Title is required";
    isValid = false;
  }
  if (!formData.value.description.trim()) {
    errors.value.description = "Recommendation description is required";
    isValid = false;
  }

  return isValid;
};

const handleSubmit = async () => {
  if (!props.observationId) {
    showNotification("Observation is required", "error");
    return;
  }

  if (!validateForm()) return;

  try {
    const recommendationData = {
      title: formData.value.title.trim(),
      description: formData.value.description.trim(),

      observationId: props.observationId,
    };

    const result = await createRecommendation(recommendationData);

    if (result?.success) {
      const newRecommendation = {
        id: result.data.recommendationId,
        ...recommendationData,
        createdAt: new Date().toISOString(),
      };

      showNotification(`Recommendation "${newRecommendation.title}" has been added successfully!`, "success");
      emit("recommendation-added", newRecommendation);
      emit("close");
    } else {
      showNotification(result?.message || "Failed to create recommendation", "error");
    }
  } catch (err) {
    console.error("Error creating recommendation:", err);
    showNotification("Failed to create recommendation. Please try again.", "error");
  }
};
</script>