<template>
    <!--EditRiskModal.vue-->
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
                  <span v-if="isUpdating">Updating...</span>
                  <span v-else>Update Risk</span>
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
import { onMounted, ref } from 'vue';
import BaseInput from '@/components/base/BaseInput.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import { useUpdateRiskById } from '@/composables/riskControl/risk/useUpdateRiskById';
import { useNotification } from '@/composables/notification/useNotification';
import RateSelector from '@/components/common/rate/RateSelector.vue';
import { useSearchRates } from '@/composables/common/rate/useSearchRates';

const props = defineProps({
  isVisible: { type: Boolean, default: false },
  risk: { type: Object, default: () => ({}) }
});

const emit = defineEmits(['close', 'risk-updated']);

const { showNotification } = useNotification();
const { isUpdating, error, updateRisk, resetState } = useUpdateRiskById();
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

onMounted( async () => {
    resetForm();
    isLoadingRates.value = true; 
    await searchRates(); 
    rates.value = searchResults.value; 
    isLoadingRates.value = false; 
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
  if (!props.risk.id) {
    showNotification('Risk is required', 'error');
    return;
  }
  if (!props.risk.objectiveId) {
    showNotification('Objective is requerd', 'error');
  }

  if (!validateForm()) return;

  try {
    const riskData = {
      id: props.risk.id,
      riskName: formData.value.riskName.trim(),
      description: formData.value.description.trim(),
      objectiveId: props.risk.objectiveId,
      rateId: selectedRateId.value,
      createdAt: props.risk.createdAt,
    };

    const result = await updateRisk(riskData);

    if (result?.success) {
      const updatedRiskData = {
        ...riskData,
        ...result.data
      };

      showNotification(`Risk "${updatedRiskData.riskName}" has been updated successfully!`, 'success');
      emit('risk-updated', updatedRiskData);
      emit('close');
    } else {
      showNotification(result?.message || 'Failed to update risk', 'error');
    }
  } catch (err) {
    console.error('Error updating risk:', err);
    showNotification('Failed to update risk. Please try again.', 'error');
  }
};
</script>
