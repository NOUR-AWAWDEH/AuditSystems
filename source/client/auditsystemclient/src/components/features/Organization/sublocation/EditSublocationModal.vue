<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 z-50 flex items-center justify-center p-4">
        <!-- Backdrop -->
        <div 
          class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity"
          @click="$emit('close')"
        ></div>
        
        <!-- Modal -->
        <div class="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all">
          <!-- Header -->
          <div class="flex items-center justify-between p-4 border-b border-gray-tint">
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Edit Sublocation</h3>
            <button 
              @click="$emit('close')" 
              class="text-gray-base hover:text-gray-shade focus:outline-none"
            >
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
          
          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">
              <!-- Sublocation Name Field -->
              <div>
                <label for="edit-sublocation-name" class="block text-sm font-medium text-secondary-shade mb-1">
                  Sublocation Name *
                </label>
                <BaseInput
                  id="edit-sublocation-name"
                  v-model="formData.name"
                  placeholder="Enter sublocation name"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.name ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                />
                <p v-if="errors.name" class="text-error text-sm mt-1">{{ errors.name }}</p>
              </div>
              
              <!-- Address Field -->
              <div>
                <label for="edit-sublocation-address" class="block text-sm font-medium text-secondary-shade mb-1">
                  Address
                </label>
                <BaseInput
                  id="edit-sublocation-address"
                  v-model="formData.address"
                  placeholder="Enter sublocation address"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.address ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                />
                <p v-if="errors.address" class="text-error text-sm mt-1">{{ errors.address }}</p>
              </div>
              
              <!-- Error Display -->
              <div v-if="error" class="p-3 bg-soft-error border border-error rounded-md">
                <p class="text-error text-sm">{{ error }}</p>
              </div>
              
              <div class="flex justify-end space-x-3 pt-4">
                <button
                  type="button"
                  @click="$emit('close')"
                  :disabled="isUpdating"
                  class="px-4 py-2 text-sm font-medium text-gray-shade bg-white border border-gray-tint rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  Cancel
                </button>
                
                <button
                  type="submit"
                  :disabled="!formData.name.trim() || isUpdating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  <span v-if="isUpdating">Updating...</span>
                  <span v-else>Update Sublocation</span>
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
import { useUpdateSubLocationById } from '@/composables/organization/sublocation/useUpdateSubLocationById';
import { useNotification } from '@/composables/notification/useNotification';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  sublocation: {
    type: Object,
    default: () => ({})
  }
});

const emit = defineEmits(['close', 'updated']);

const { showNotification } = useNotification();
const { isUpdating, error, updateSubLocation, resetState } = useUpdateSubLocationById();
const { authStatus, checkAuthStatus } = useAuthStatus();

const formData = ref({
  name: '',
  address: ''
});

const errors = ref({
  name: '',
  address: ''
});

// Reset form when modal opens/closes or sublocation changes
watch([() => props.isVisible, () => props.sublocation], ([newVisible, newSublocation]) => {
  if (newVisible && newSublocation) {
    resetForm();
    populateForm();
  }
});

const resetForm = () => {
  formData.value = {
    name: '',
    address: ''
  };
  errors.value = {
    name: '',
    address: ''
  };
  
  resetState();
};

const populateForm = () => {
  if (props.sublocation) {
    formData.value = {
      name: props.sublocation.name || '',
      address: props.sublocation.address || ''
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


  if (!props.sublocation?.id) {
    console.error('Missing sublocation ID:', props.sublocation);
    showNotification('Sublocation ID is required', 'error');
    return;
  }
  
  if (!validateForm()) {
    console.error('Form validation failed:', errors.value);
    return;
  }
  
  try {
    const sublocationData = {
      id: props.sublocation.id,
      name: formData.value.name.trim(),
      address: formData.value.address.trim(),
      locationId: props.sublocation.locationId
    };
   
    const result = await updateSubLocation(sublocationData);
    
    
    if (result?.success) {      
      // Pass the updated sublocation data with the correct structure
      const updatedSublocationData = {
        ...sublocationData,
        ...result.data
      };
      
      emit('updated', updatedSublocationData);
      emit('close');
    } else {
      console.error('❌ Update failed - result not successful:', result);
    }
  } catch (error) {
    console.error('❌ Error in handleSubmit:', error);
    console.error('Error details:', {
      message: error.message,
      response: error.response?.data,
      status: error.response?.status
    });
    showNotification('Failed to update sublocation. Please try again.', 'error');
  }
  
};

const validateForm = () => {
  errors.value = {
    name: '',
    address: ''
  };
  
  let isValid = true;
  
  if (!formData.value.name.trim()) {
    errors.value.name = 'Sublocation name is required';
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