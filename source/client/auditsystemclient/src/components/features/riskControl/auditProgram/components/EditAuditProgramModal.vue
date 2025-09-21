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
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Edit Audit Program </h3>
            <button @click="$emit('close')" class="text-gray-base hover:text-gray-shade focus:outline-none">
              <CloseIcon size="md" />
            </button>
          </div>

          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">
              <!-- Audit Program Description Field -->
              <div>
                <label for="edit-audit-program-description" class="block text-sm font-medium text-secondary-shade mb-1">
                  Audit Program Description *
                </label>
                <BaseInput 
                  id="edit-audit-program-description"
                  v-model="formData.description"
                  placeholder="Enter Audit Program description"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.description ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`" />
                <p v-if="errors.description" class="text-error text-sm mt-1">{{ errors.description }}</p>
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
                  <span v-else>Update Audit Program</span>
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
import { useUpdateAuditProgramById } from '@/composables/riskControl/auditProgram/useUpdateAuditProgramById';
import { useNotification } from '@/composables/notification/useNotification';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  auditProgram: {
    type: Object,
    // Remove the default value to ensure it's either passed or undefined
    // default: () => ({})
  }
});

const emit = defineEmits(['close', 'audit-program-updated']);

const { showNotification } = useNotification();
const { isUpdating, error, updateAuditProgram, resetState } = useUpdateAuditProgramById();
const { authStatus, checkAuthStatus } = useAuthStatus();

const formData = ref({
  description: ''
});

const errors = ref({
  description: ''
});

// Reset form when modal opens/closes or audit program changes
watch([() => props.isVisible, () => props.auditProgram], ([newVisible, newAuditProgram]) => {
  if (newVisible && newAuditProgram) {
    resetForm();
    populateForm();
  }
});

const resetForm = () => {
  formData.value = {
    description: ''
  };
  errors.value = {
    description: ''
  };

  resetState();
};

const populateForm = () => {
  if (props.auditProgram) {
    formData.value = {
      description: props.auditProgram.description || ''
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

  if (!props.auditProgram?.id) {
    console.error('Missing Audit Program ID:', props.auditProgram);
    showNotification('Audit Program ID is required', 'error');
    return;
  }

  if (!validateForm()) {
    console.error('Form validation failed:', errors.value);
    return;
  }

  try {
    const auditProgramData = {
        id: props.auditProgram?.id,
        exitControlId: props.auditProgram?.exitControlId,
        controlDesignId: props.auditProgram?.controlDesignId ?? null,
        description: formData.value.description.trim(),
        createdAt: props.auditProgram?.createdAt,
        updatedAt: new Date().toISOString()
      };


    const result = await updateAuditProgram(auditProgramData);

    if (result?.success) {
      const updatedAuditProgramData = {
        ...auditProgramData,
        ...result.data
      };

      showNotification(`Audit Program "${updatedAuditProgramData.description}" has been updated successfully!`, 'success');
      emit('audit-program-updated', updatedAuditProgramData);
      emit('close');
    } else {
      const errorMessage = result?.message || error.value || 'Failed to update Audit Program. Please try again.';
      showNotification(errorMessage, 'error');
    }
  } catch (err) {
    console.error('Error in handleSubmit:', err);
    const errorMessage = err.response?.data?.message || err.message || 'Failed to update Audit Program. Please try again.';
    showNotification(errorMessage, 'error');
  }
};

const validateForm = () => {
  errors.value = {
    description: ''
  };

  let isValid = true;

  if (!formData.value.description.trim()) {
    errors.value.description = 'Audit Program description is required';
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