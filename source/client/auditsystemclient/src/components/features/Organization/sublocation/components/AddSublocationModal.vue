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
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Add Sub Location</h3>
            <button 
              @click="$emit('close')" 
              class="text-gray-base hover:text-gray-shade focus:outline-none">
              <CloseIcon size="md" custom-class="text-gray-400 hover:text-gray-600" />
            </button>
          </div>

          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">
              <!-- SubLocation Name Field -->
              <div>
                <label for="sub-location-name" class="block text-sm font-medium text-secondary-shade mb-1">
                  Sub Location Name *
                </label>
                <BaseInput 
                  id="sub-location-name" 
                  v-model="formData.name"
                  label="sub-location Name" 
                  placeholder="Enter sub location name"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.name ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`" 
                />
                <p v-if="errors.name" class="text-error text-sm mt-1">{{ errors.name }}</p>
              </div>

              <!-- Address Field -->
              <div>
                <label for="sub-location-address" class="block text-sm font-medium text-secondary-shade mb-1">
                  Address
                </label>
                <BaseInput id="sub-location-address" v-model="formData.address" placeholder="Enter sub-location address"
                  :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.address ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`" />
                <p v-if="errors.address" class="text-error text-sm mt-1">{{ errors.address }}</p>
              </div>

              <!-- Error Display -->
              <div v-if="error" class="p-3 bg-soft-error border border-error rounded-md">
                <p class="text-error text-sm">{{ error }}</p>
              </div>

              <div class="flex justify-end space-x-3 pt-4">
                <button type="button" @click="$emit('close')" :disabled="isCreating"
                  class="px-4 py-2 text-sm font-medium text-white bg-error border border-gray-tint rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                  Cancel
                </button>

                <button 
                  type="submit" 
                  :disabled="!formData.name.trim() || isCreating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                  <span v-if="isCreating">Adding...</span>
                  <span v-else>Add Sub Location</span>
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
import { useCreateSubLocation } from '@/composables/organization/sublocation/useCreateSubLocation';
import { useNotification } from '@/composables/notification/useNotification';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  locationId: {
    type: [String, Number],
    default: null
  }
});

const emit = defineEmits(['close', 'sub-location-added']);

const { showNotification } = useNotification();
const { isCreating, error, createSubLocation, resetState } = useCreateSubLocation();

const formData = ref({
  name: '',
  address: ''
});

const errors = ref({
  name: '',
  address: ''
});

watch(() => props.isVisible, (newValue) => {
  if (newValue) {
    resetForm();
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

const handleSubmit = async () => {
  if (!props.locationId) {
    showNotification('Location is required', 'error');
    return;
  }

  if (!validateForm()) {
    return;
  }

  try {
    const subLocationData = {
      name: formData.value.name.trim(),
      address: formData.value.address.trim(),
      locationId: props.locationId
    };

    const result = await createSubLocation(subLocationData);

    if (result?.success) {
      showNotification(`SubLocation "${subLocationData.name}" has been added successfully!`, 'success');
      emit('sub-location-added', result.data);
      emit('close');
    }
  } catch (error) {
    console.error('Error in handleSubmit:', error);
    showNotification('Failed to add sub location. Please try again.', 'error');
  }
};

const validateForm = () => {
  errors.value = {
    name: '',
    address: ''
  };

  let isValid = true;

  if (!formData.value.name.trim()) {
    errors.value.name = 'Sub Location name is required';
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
  transform: scale(0.95) translateY(-20px);
}

/* Enhanced mobile responsiveness */
@media (max-width: 375px) {
  .relative {
    margin: 0.5rem;
    max-width: calc(100vw - 1rem);
  }
}

/* Improve button touch targets on mobile */
@media (max-width: 640px) {
  button {
    min-height: 44px;
  }
}
</style>