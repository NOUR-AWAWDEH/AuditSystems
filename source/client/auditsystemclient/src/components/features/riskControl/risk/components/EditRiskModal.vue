<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 flex items-center justify-center p-4">
        <!-- Overlay -->
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
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Edit Risk</h3>
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
              <!-- Risk Name -->
              <div>
                <label for="risk-name" class="block text-sm font-medium text-gray-700">
                  Risk Name *
                </label>
                <BaseInput
                  id="risk-name"
                  v-model="formData.riskName"
                  placeholder="Enter Risk Name"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                    errors.riskName ? 'border-error' : 'border-gray-tint'
                  } focus:ring-primary-base`"
                />
                <p v-if="errors.riskName" class="text-error text-sm mt-1">{{ errors.riskName }}</p>
              </div>

              <!-- Risk Description -->
              <div>
                <label for="risk-description" class="block text-sm font-medium text-gray-700">
                  Risk Description *
                </label>
                <BaseInput
                  id="risk-description"
                  v-model="formData.description"
                  placeholder="Enter Risk Description"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                    errors.description ? 'border-error' : 'border-gray-tint'
                  } focus:ring-primary-base`"
                />
                <p v-if="errors.description" class="text-error text-sm mt-1">{{ errors.description }}</p>
              </div>

              <!-- Rate Selector -->
              <RateSelector
                :rates="rates"
                :selectedRateId="selectedRateId"
                label="Risk Level"
                placeholder="Select a risk level"
                :error="errors.rate"
                @update:selectedRateId="handleRateChange"
              />
              <p v-if="errors.rate" class="text-error text-sm mt-1">{{ errors.rate }}</p>

              <!-- API Error -->
              <div v-if="error" class="p-3 bg-soft-error border border-error rounded-md">
                <p class="text-error text-sm">{{ error }}</p>
              </div>

              <!-- Actions -->
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
                  :disabled="!formData.riskName.trim() || isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  <span v-if="isUpdating">Editing...</span>
                  <span v-else>Edit Risk</span>
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
import { ref, watch } from 'vue';
import BaseInput from '@/components/base/BaseInput.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import RateSelector from '@/components/common/rate/RateSelector.vue';
import { useUpdateRiskById } from '@/composables/riskControl/risk/useUpdateRiskById.js';
import { useSearchRates } from '@/composables/common/rate/useSearchRates';

// Props
const props = defineProps({
  isVisible: { type: Boolean, default: false },
  objectiveId: { type: [String, Number], default: null },
  risk: { type: Object, default: null }
});

const emit = defineEmits(['close', 'risk-updated']);

const { isUpdating, error, updateRisk, resetState } = useUpdateRiskById();

// Form state
const formData = ref({
  riskName: '',
  description: ''
});
const errors = ref({
  riskName: '',
  description: '',
  rate: ''
});

// Rates from API
const { searchResults: rates } = useSearchRates();
const selectedRateId = ref('');

// Prefill form when modal opens
watch(
  () => props.isVisible,
  (visible) => {
    if (visible) resetForm();
  }
);

const resetForm = () => {
  formData.value = {
    riskName: props.risk?.riskName || '',
    description: props.risk?.description || ''
  };
  selectedRateId.value = props.risk?.rateId || '';
  errors.value = { riskName: '', description: '', rate: '' };
  resetState();
};

const handleRateChange = (value) => {
  selectedRateId.value = value;
};

const validateForm = () => {
  errors.value = { riskName: '', description: '', rate: '' };
  let valid = true;

  if (!formData.value.riskName.trim()) {
    errors.value.riskName = 'Risk name is required';
    valid = false;
  }
  if (!formData.value.description.trim()) {
    errors.value.description = 'Risk description is required';
    valid = false;
  }
  if (!selectedRateId.value) {
    errors.value.rate = 'Risk rate is required';
    valid = false;
  }

  return valid;
};

const handleSubmit = async () => {
  if (!props.objectiveId) return;

  if (!validateForm()) return;

  const riskData = {
    id: props.risk?.id,
    riskName: formData.value.riskName.trim(),
    description: formData.value.description.trim(),
    rateId: selectedRateId.value,
    objectiveId: props.objectiveId
  };

  const result = await updateRisk(riskData);

  if (result?.success) {
    emit('risk-updated', result.data);
    emit('close');
  }
};
</script>