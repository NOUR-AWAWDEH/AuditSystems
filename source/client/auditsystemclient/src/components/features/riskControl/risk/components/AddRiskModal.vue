<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 flex items-center justify-center p-4">
        <div
          class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity"
          @click="$emit('close')"
        ></div>

        <!-- Modal -->
        <div class="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all">
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
              <!-- Risk Name Field -->
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
                <p v-if="errors.riskName" class="text-error text-sm mt-1">
                  {{ errors.riskName }}
                </p>
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
                <p v-if="errors.description" class="text-error text-sm mt-1">
                  {{ errors.description }}
                </p>
              </div>

              <!-- Rate Selector -->
              <RateSelector
                v-if="!isLoadingRates"
                :rates="rates"
                :selectedRateId="selectedRateId"
                label="Risk Level"
                placeholder="Select a risk level"
                :error="errors.rate"
                @update:selectedRateId="handleRateChange"
              />
              <p v-if="errors.rate" class="text-error text-sm mt-1">
                {{ errors.rate }}
              </p>

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
                  :disabled="!formData.riskName.trim() || isCreating"
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
import { ref, watch } from 'vue';
import BaseInput from '@/components/base/BaseInput.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import { useCreateRisk } from '@/composables/riskControl/risk/useCreateRisk.js';
import { useNotification } from '@/composables/notification/useNotification';
import RateSelector from '@/components/common/rate/RateSelector.vue';
import { useSearchRates } from '@/composables/common/rate/useSearchRates';

const props = defineProps({
  isVisible: { type: Boolean, default: false },
  objectiveId: { type: [String, Number], default: null }
});

const emit = defineEmits(['close', 'risk-added']);

const { showNotification } = useNotification();
const { isCreating, error, createRisk, resetState } = useCreateRisk();
const rates = ref([]);
const isLoadingRates = ref(true);

const { searchRates, searchResults } = useSearchRates();
const selectedRateId = ref('');

const formData = ref({
  riskName: '',
  description: '',
  rate: ''
});

const errors = ref({
  riskName: '',
  description: '',
  rate: ''
});

watch(() => props.isVisible, async (newValue) => {
  if (newValue) {
    resetForm();
    isLoadingRates.value = true; // Start loading
    await searchRates(); // Fetch rates
    rates.value = searchResults.value; // Assign fetched rates to the reactive rates ref
    isLoadingRates.value = false; // End loading
  }
});

const resetForm = () => {
  formData.value = { riskName: '', description: '', rate: '' };

  errors.value = { riskName: '', description: '', rate: '' };
  selectedRateId.value = '';
  resetState();
};



const handleRateChange = (value) => {
  selectedRateId.value = value;
};

const validateForm = () => {
  errors.value = { riskName: '', description: '', rate: '' };
  let isValid = true;

  if (!formData.value.riskName.trim()) {
    errors.value.riskName = 'Risk name is required';
    isValid = false;
  }
  if (!formData.value.description.trim()) {
    errors.value.description = 'Risk description is required';
    isValid = false;
  }
  if (!selectedRateId.value) {
    errors.value.rate = 'Risk rate is required';
    isValid = false;
  }

  return isValid;
};

const handleSubmit = async () => {
  if (!props.objectiveId) {
    showNotification('Objective is required', 'error');
    return;
  }

  if (!validateForm()) return;

  try {
    const riskData = {
      riskName: formData.value.riskName.trim(),
      description: formData.value.description.trim(),
      rateId: selectedRateId.value,
      objectiveId: props.objectiveId
    };

    const result = await createRisk(riskData);

    if (result?.success) {
      const newRisk = {
        id: result.data.riskId,
        ...riskData,
        createdAt: new Date().toISOString(),
      };

      showNotification(`Risk "${newRisk.riskName}" has been added successfully!`, 'success');
      emit('risk-added', newRisk);
      emit('close');

    } else {
      showNotification(result?.message || 'Failed to create risk', 'error');
    }
  } catch (err) {
    console.error('Error creating risk:', err);
    showNotification('Failed to create risk. Please try again.', 'error');
  }
};
</script>
