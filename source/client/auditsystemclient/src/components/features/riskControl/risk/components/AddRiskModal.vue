<template>
  <!--AddRiskModal.vue-->
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
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Add Risk</h3>
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
              <!-- Risk Factor Field -->
              <div>
                <label for="risk-factor" class="block text-sm font-medium text-gray-700">
                  Risk Factor *
                </label>
                <BaseInput
                  id="risk-factor"
                  v-model="formData.factor"
                  placeholder="Enter Risk Factor"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                    errors.factor ? 'border-error' : 'border-gray-tint'
                  } focus:ring-primary-base`"
                />
                <p v-if="errors.factor" class="text-error text-sm mt-1">
                  {{ errors.factor }}
                </p>
              </div>

              <!-- Risk Impact -->
              <div>
                <label for="risk-impact" class="block text-sm font-medium text-gray-700">
                  Risk Impact *
                </label>
                <BaseInput
                  id="risk-impact"
                  v-model="formData.impact"
                  placeholder="Enter Risk Impact"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                    errors.impact ? 'border-error' : 'border-gray-tint'
                  } focus:ring-primary-base`"
                />
                <p v-if="errors.impact" class="text-error text-sm mt-1">
                  {{ errors.impact }}
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
                  :disabled="!formData.factor.trim() || isCreating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  <span v-if="isCreating">Adding...</span>
                  <span v-else>Add Risk</span>
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
import { useCreateRisk } from "@/composables/riskControl/risk/useCreateRisk.js";
import { useNotification } from "@/composables/notification/useNotification";

const props = defineProps({
  isVisible: { type: Boolean, default: false },
  observationId: { type: [String, Number], default: null },
});

const emit = defineEmits(["close", "risk-added"]);

const { showNotification } = useNotification();
const { isCreating, error, createRisk, resetState } = useCreateRisk();

const formData = ref({
  factor: "",
  impact: "",
});

const errors = ref({
  factor: "",
  impact: "",
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
  formData.value = { factor: "", impact: "" };

  errors.value = { factor: "", impact: "" };
  resetState();
};

const validateForm = () => {
  errors.value = { factor: "", impact: "" };
  let isValid = true;

  if (!formData.value.factor.trim()) {
    errors.value.factor = "Risk factor is required";
    isValid = false;
  }
  if (!formData.value.impact.trim()) {
    errors.value.impact = "Risk impact is required";
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
    const riskData = {
      factor: formData.value.factor.trim(),
      impact: formData.value.impact.trim(),

      observationId: props.observationId,
    };

    const result = await createRisk(riskData);

    if (result?.success) {
      const newRisk = {
        id: result.data.riskId,
        ...riskData,
        createdAt: new Date().toISOString(),
      };

      showNotification(`Risk for Factor "${newRisk.factor}" has been added successfully!`, "success");
      emit("risk-added", newRisk);
      emit("close");
    } else {
      showNotification(result?.message || "Failed to create risk", "error");
    }
  } catch (err) {
    console.error("Error creating risk:", err);
    showNotification("Failed to create risk. Please try again.", "error");
  }
};
</script>
