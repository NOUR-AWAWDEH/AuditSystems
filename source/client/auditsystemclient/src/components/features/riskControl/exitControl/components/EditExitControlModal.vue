<template>
    <!--EditExitControlModal.vue-->
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 z-50 flex items-center justify-center p-4">
        <!-- Backdrop -->
        <div class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity" @click="$emit('close')"></div>

        <!-- Modal -->
        <div class="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all">
          <!-- Header -->
          <div class="flex items-center justify-between p-4 border-b border-gray-tint">
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Edit Exit Control</h3>
            <button @click="$emit('close')" class="text-gray-base hover:text-gray-shade focus:outline-none">
              <CloseIcon size="md" />
            </button>
          </div>

          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">
              <!-- Exit Control Description Field -->
              <div>
                <label for="edit-ExitControl-description" class="block text-sm font-medium text-secondary-shade mb-1">
                  Exit Control Description *
                </label>
                <BaseInput 
                  id="edit-ExitControl-description"
                  v-model="formData.description"
                  placeholder="Enter ExitControl description"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.description ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`" />
                <p v-if="errors.description" class="text-error text-sm mt-1">{{ errors.description }}</p>
              </div>

              <!-- Inadequate -->
                <div>
                    <label for="exit-control-inadequate" class="block text-sm font-medium text-gray-700">
                        Exit Control Inadequate *
                    </label>
                    <select
                        id="exit-control-inadequate"
                        v-model="formData.inadequate"
                        class="w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2"
                        :class="errors.inadequate ? 'border-error focus:ring-error' : 'border-gray-tint focus:ring-primary-base'"
                    >
                        <option disabled value="">-- Select an option --</option>
                        <option :value="true">Inadequate</option>
                        <option :value="false">Adequate</option>
                    </select>
                    <p v-if="errors.inadequate" class="text-error text-sm mt-1">{{ errors.inadequate }}</p>
                </div>

              <!-- Error Display -->
              <div v-if="error" class="p-3 bg-soft-error border border-error rounded-md">
                <p class="text-error text-sm">{{ error }}</p>
              </div>

              <div class="flex justify-end space-x-3 pt-4">
                <button type="button" @click="$emit('close')" :disabled="isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-error border border-gray-tint rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                  Cancel
                </button>

                <button type="submit" :disabled="!formData.description.trim() || isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                  <span v-if="isUpdating">Updating...</span>
                  <span v-else>Update ExitControl</span>
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
import { useUpdateExitControlById } from '@/composables/riskControl/exitControl/useUpdateExitControlById';
import { useNotification } from '@/composables/notification/useNotification';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  exitControl: {
    type: Object,
    default: () => ({})
  }
});

const emit = defineEmits(['close', 'exit-control-updated']);

const { showNotification } = useNotification();
const { isUpdating, error, updateExitControl, resetState } = useUpdateExitControlById();
const { authStatus, checkAuthStatus } = useAuthStatus();

const formData = ref({
  description: '',
  inadequate: null
});

const errors = ref({
  description: '',
  inadequate: null
});

// Reset form when modal opens/closes or Exit Control changes
watch([() => props.isVisible, () => props.exitControl], ([newVisible, newExitControl]) => {
  if (newVisible && newExitControl) {
    resetForm();
    populateForm();
  }
});

const resetForm = () => {
  formData.value = {
    description: '',
    inadequate: null
  };
  errors.value = {
    description: '',
    inadequate: null
  };

  resetState();
};

const populateForm = () => {
  if (props.exitControl) {
    formData.value = {
      description: props.exitControl.description || '',
      inadequate: props.exitControl.inadequate || null
    };
  }
};

// Initialize auth status when component mounts
onMounted(async () => {
  if (!authStatus.value) {
    await checkAuthStatus();
  }
});

const handleSubmit = async () => {
  // Check if user is authenticated
  if (!authStatus.value) {
    console.error('Authentication status not loaded');
    showNotification('Authentication required. Please refresh the page and try again.', 'error');
    return;
  }

  if (!props.exitControl?.id) {
    console.error('Missing Exit Control ID:', props.exitControl);
    showNotification('Exit Control ID is required', 'error');
    return;
  }

  if (!validateForm()) {
    console.error('Form validation failed:', errors.value);
    return;
  }

  try {
    const exitControlData = {
        id: props.exitControl?.id,
        objectiveId: props.exitControl?.objectiveId,
        description: formData.value.description.trim(),
        inadequate: formData.value.inadequate
      };


    const result = await updateExitControl(exitControlData);

    if (result?.success) {
      const updatedExitControlData = {
        ...exitControlData,
        ...result.data
      };

      showNotification(`Exit Control "${updatedExitControlData.description}" has been updated successfully!`, 'success');
      emit('exit-control-updated', updatedExitControlData);
      emit('close');
    } else {
      const errorMessage = result?.message || error.value || 'Failed to update Exit Control. Please try again.';
      showNotification(errorMessage, 'error');
    }
  } catch (err) {
    console.error('Error in handleSubmit:', err);
    const errorMessage = err.response?.data?.message || err.message || 'Failed to update Exit Control. Please try again.';
    showNotification(errorMessage, 'error');
  }
};

const validateForm = () => {
  errors.value = {
    description: '',
    inadequate: null
  };

  let isValid = true;

  if (!formData.value.description.trim()) {
    errors.value.description = 'Exit Control description is required';
    isValid = false;
  }

  if (formData.value.inadequate === null) {
    errors.value.inadequate = 'Please select an option';
    isValid = false;
  }

  return isValid;
};


</script>

<style scoped>
.modal-enter-active,
.modal-leave-active {
  transition: opacity 0.3s ease;
}

.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

.modal-enter-active .relative,
.modal-leave-active .relative {
  transition: transform 0.3s ease;
}

.modal-enter-from .relative,
.modal-leave-to .relative {
  transform: scale(0.95);
}
</style>