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
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Edit Process</h3>
            <button @click="$emit('close')" class="text-gray-base hover:text-gray-shade focus:outline-none">
              <CloseIcon size="md" />
            </button>
          </div>

          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">
              <!-- Audit Process Name Field -->
              <div>
                <label for="edit-audit-process-name" class="block text-sm font-medium text-secondary-shade mb-1">
                  Process Name *
                </label>
                <BaseInput id="edit-audit-process-name" v-model="formData.name" placeholder="Enter Audit Process name"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.name ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`" />
                <p v-if="errors.name" class="text-error text-sm mt-1">{{ errors.name }}</p>
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

                <button type="submit" :disabled="!formData.name.trim() || isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                  <span v-if="isUpdating">Updating...</span>
                  <span v-else>Update Process</span>
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
import { useUpdateAuditProcessById } from '@/composables/riskControl/auditProcess/useUpdateAuditProcessById';
import { useNotification } from '@/composables/notification/useNotification';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  auditProcess: {
    type: Object,
    default: () => ({})
  }
});

const emit = defineEmits(['close', 'audit-process-updated']);

const { showNotification } = useNotification();
const { isUpdating, error, updateAuditProcess, resetState } = useUpdateAuditProcessById();
const { authStatus, checkAuthStatus } = useAuthStatus();

const formData = ref({
  name: ''
});

const errors = ref({
  name: ''
});

// Reset form when modal opens/closes or audit process changes
watch([() => props.isVisible, () => props.auditProcess], ([newVisible, newAuditProcess]) => {
  if (newVisible && newAuditProcess) {
    resetForm();
    populateForm();
  }
});

const resetForm = () => {
  formData.value = {
    name: ''
  };
  errors.value = {
    name: ''
  };

  resetState();
};

const populateForm = () => {
  if (props.auditProcess) {
    formData.value = {
      name: props.auditProcess.processName || '',
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

  if (!props.auditProcess?.id) {
    console.error('Missing process ID:', props.auditProcess);
    showNotification('Process ID is required', 'error');
    return;
  }

  if (!validateForm()) {
    console.error('Form validation failed:', errors.value);
    return;
  }

  try {
    const auditProcessData = {
      id: props.auditProcess?.id,
      processName: formData.value.name.trim(),
      subDepartmentId: props.auditProcess?.subDepartmentId
    };

    const result = await updateAuditProcess(auditProcessData);

    if (result?.success) {
      const updatedAuditProcessData = {
        ...auditProcessData,
        ...result.data
      };

      emit('audit-process-updated', updatedAuditProcessData);
      emit('close');
    } else {
      const errorMessage = result?.message || error.value || 'Failed to update audit process. Please try again.';
      showNotification(errorMessage, 'error');
    }
  } catch (err) {
    console.error('Error in handleSubmit:', err);
    const errorMessage = err.response?.data?.message || err.message || 'Failed to update audit process. Please try again.';
    showNotification(errorMessage, 'error');
  }
};

const validateForm = () => {
  errors.value = {
    name: ''
  };

  let isValid = true;

  if (!formData.value.name.trim()) {
    errors.value.name = 'Audit Process name is required';
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
