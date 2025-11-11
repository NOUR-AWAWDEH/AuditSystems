<template>
  <!--EditObservationModal.vue-->
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 z-50 flex items-center justify-center p-4">
        <!-- Backdrop -->
        <div class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity" @click="$emit('close')"></div>

        <!-- Modal -->
        <div class="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all">
          <!-- Header -->
          <div class="flex items-center justify-between p-4 border-b border-gray-tint">
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Edit Observation</h3>
            <button @click="$emit('close')" class="text-gray-base hover:text-gray-shade focus:outline-none">
              <CloseIcon size="md" />
            </button>
          </div>

          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">
              <div v-for="field in fields" :key="field.key">
                <label :for="field.id" class="block text-sm font-medium text-gray-700">
                  {{ field.label }} *
                </label>

                <component
                  :is="field.component"
                  v-model="formData[field.key]"
                  :id="field.id"
                  :placeholder="field.placeholder"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors[field.key] ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                />

                <p v-if="errors[field.key]" class="text-error text-sm mt-1">{{ errors[field.key] }}</p>
              </div>

              <!--Error Display -->
              <div v-if="error" class="p-3 bg-soft-error border border-error rounded-md">
                <p class="text-error text-sm">{{ error }}</p>
              </div>

              <!-- Action Buttons -->
              <div class="flex justify-end space-x-3 pt-4">
                <button type="button" @click="$emit('close')" :disabled="isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-error border border-gray-tint rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                  Cancel
                </button>

                <button type="submit" :disabled="isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                  <span v-if="isUpdating">Updating...</span>
                  <span v-else>Update Observation</span>
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
import { ref, watch, onMounted } from 'vue';
import BaseInput from '@/components/base/BaseInput.vue';
import BaseDateInput from '@/components/base/BaseDateInput.vue';
import { useUpdateObservationById } from '@/composables/riskControl/observation/useUpdateObservationById';
import { useNotification } from '@/composables/notification/useNotification';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';

const props = defineProps({
  isVisible: { type: Boolean, default: false },
  observation: { type: Object, default: () => ({}) }
});

const emit = defineEmits(['close', 'observation-updated']);

const { showNotification } = useNotification();
const { isUpdating, error, updateObservation, resetState } = useUpdateObservationById();
const { authStatus, checkAuthStatus } = useAuthStatus();

const formData = ref({
  description: '',
  background: '',
  actionOwner: '',
  actionPlan: '',
  actionDate: '',
  note: ''
});

const errors = ref({});

const fields = [
  { key: 'description', label: 'Observation Description', id: 'observation-description', placeholder: 'Enter Observation Description', component: BaseInput },
  { key: 'background', label: 'Observation Background', id: 'observation-background', placeholder: 'Enter Observation Background', component: BaseInput },
  { key: 'actionOwner', label: 'Observation Action Owner', id: 'observation-action-owner', placeholder: 'Enter Observation Action Owner', component: BaseInput },
  { key: 'actionPlan', label: 'Observation Action Plan', id: 'observation-action-plan', placeholder: 'Enter Observation Action Plan', component: BaseInput },
  { key: 'actionDate', label: 'Observation Action Date', id: 'observation-action-date', placeholder: 'Select Observation Action Date', component: BaseDateInput },
  { key: 'note', label: 'Observation Note', id: 'observation-note', placeholder: 'Enter Observation Note', component: BaseInput }
];

const resetForm = () => {
  formData.value = {
    description: '',
    background: '',
    actionOwner: '',
    actionPlan: '',
    actionDate: '',
    note: ''
  };
  errors.value = {};
  resetState();
};

const populateForm = () => {
  if (props.observation?.id) {
    formData.value = {
      description: props.observation.description || '',
      background: props.observation.background || '',
      actionOwner: props.observation.actionOwner || '',
      actionPlan: props.observation.actionPlan || '',
      actionDate: props.observation.actionDate || '',
      note: props.observation.note || ''
    };
  }
};

watch([() => props.isVisible, () => props.observation], ([visible, obs]) => {
  if (visible && obs?.id) {
    resetForm();
    populateForm();
  }
});

onMounted(async () => {
  if (!authStatus.value) await checkAuthStatus();
});

const validateForm = () => {
  errors.value = {};
  let isValid = true;

  Object.keys(formData.value).forEach(key => {
    if (key === 'note') return;
    if (!formData.value[key] || !formData.value[key].toString().trim()) {
      errors.value[key] = `${fields.find(f => f.key === key).label} is required`;
      isValid = false;
    }
  });

  return isValid;
};

const handleSubmit = async () => {
  if (!authStatus.value) {
    showNotification('Authentication required. Please refresh the page and try again.', 'error');
    return;
  }

  if (!props.observation?.id) {
    showNotification('Observation ID is required', 'error');
    return;
  }

  if (!validateForm()) return;

  try {
    const observationData = {
      id: props.observation.id,
      description: formData.value.description.trim(),
      background: formData.value.background.trim(),
      actionOwner: formData.value.actionOwner.trim(),
      actionPlan: formData.value.actionPlan.trim(),
      actionDate: new Date(formData.value.actionDate).toISOString(),
      note: formData.value.note.trim(),
      auditProgramId: props.observation.auditProgramId,
      createdAt: props.observation.createdAt
    };

    const result = await updateObservation(observationData);

    if (result?.success) {
      showNotification(`Observation "${observationData.description}" updated successfully!`, 'success');
      emit('observation-updated', observationData);
      emit('close');
    } else {
      showNotification(result?.message || error.value || 'Failed to update observation.', 'error');
    }
  } catch (err) {
    showNotification(err.response?.data?.message || err.message || 'Failed to update observation.', 'error');
  }
};
</script>

<style scoped>
.modal-enter-active,
.modal-leave-active { transition: opacity 0.3s ease; }
.modal-enter-from,
.modal-leave-to { opacity: 0; }
.modal-enter-active .relative,
.modal-leave-active .relative { transition: transform 0.3s ease; }
.modal-enter-from .relative,
.modal-leave-to .relative { transform: scale(0.95); }
</style>
