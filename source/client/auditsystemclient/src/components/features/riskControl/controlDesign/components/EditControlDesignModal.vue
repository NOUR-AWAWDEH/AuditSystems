<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 z-50 flex items-center justify-center p-4">
        <!-- Backdrop -->
        <div class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity" @click="$emit('close')"></div>

        <!-- Modal -->
        <div class="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all">
          <!-- Header -->
          <div class="flex items-center justify-between p-4 border-b border-gray-tint">
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Edit Control Design</h3>
            <button @click="$emit('close')" class="text-gray-base hover:text-gray-shade focus:outline-none">
              <CloseIcon size="md" />
            </button>
          </div>

          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">
              <!-- Control Design Description Field -->
              <div>
                <label for="edit-control-design-description" class="block text-sm font-medium text-secondary-shade mb-1">
                  Control Design Description *
                </label>
                <BaseInput 
                  id="edit-control-design-description"
                  v-model="formData.description"
                  placeholder="Enter Control Design description"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.description ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`" />
                <p v-if="errors.description" class="text-error text-sm mt-1">{{ errors.description }}</p>
              </div>

              <!-- Inadequate -->
                <div>
                    <label for="control-design-inadequate" class="block text-sm font-medium text-gray-700">
                        Control Design Inadequate *
                    </label>
                    <select
                        id="control-design-inadequate"
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
                  <span v-else>Update Control Design</span>
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
import { useUpdateControlDesignById } from '@/composables/riskControl/controlDesign/useUpdateControlDesignById';
import { useNotification } from '@/composables/notification/useNotification';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  controlDesign: {
    type: Object,
    default: () => ({})
  }
});

const emit = defineEmits(['close', 'control-design-updated']);

const { showNotification } = useNotification();
const { isUpdating, error, updateControlDesign, resetState } = useUpdateControlDesignById();
const { authStatus, checkAuthStatus } = useAuthStatus();

const formData = ref({
  description: '',
  inadequate: null
});

const errors = ref({
  description: '',
  inadequate: null
});

// Reset form when modal opens/closes or control design changes
watch([() => props.isVisible, () => props.controlDesign], ([newVisible, newControlDesign]) => {
  if (newVisible && newControlDesign) {
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
  if (props.controlDesign) {
    formData.value = {
      description: props.controlDesign.description || '',
      inadequate: props.controlDesign.inadequate || null
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

  if (!props.controlDesign?.id) {
    console.error('Missing Control Design ID:', props.controlDesign);
    showNotification('Control Design ID is required', 'error');
    return;
  }

  if (!validateForm()) {
    console.error('Form validation failed:', errors.value);
    return;
  }

  try {
    const controlDesignData = {
        id: props.controlDesign?.id,
        exitControlId: props.controlDesign?.exitControlId,
        description: formData.value.description.trim(),
        inadequate: formData.value.inadequate
      };


    const result = await updateControlDesign(controlDesignData);

    if (result?.success) {
      const updatedControlDesignData = {
        ...controlDesignData,
        ...result.data
      };

      showNotification(`Control Design "${updatedControlDesignData.description}" has been updated successfully!`, 'success');
      emit('control-design-updated', updatedControlDesignData);
      emit('close');
    } else {
      const errorMessage = result?.message || error.value || 'Failed to update Control Design. Please try again.';
      showNotification(errorMessage, 'error');
    }
  } catch (err) {
    console.error('Error in handleSubmit:', err);
    const errorMessage = err.response?.data?.message || err.message || 'Failed to update Control Design. Please try again.';
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
    errors.value.description = 'Control Design description is required';
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