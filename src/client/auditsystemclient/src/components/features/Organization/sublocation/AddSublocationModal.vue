<template>
  <div v-if="isVisible" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg shadow-xl w-full max-w-md mx-4">
      <!-- Modal Header -->
      <div class="flex items-center justify-between p-6 border-b border-gray-200">
        <h3 class="text-lg font-semibold text-gray-900">Add New Sublocation</h3>
        <button
          @click="$emit('close')"
          class="text-gray-400 hover:text-gray-600 transition-colors"
        >
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
          </svg>
        </button>
      </div>

      <!-- Modal Body -->
      <form @submit.prevent="handleSubmit" class="p-6">
        <!-- Name Field -->
        <div class="mb-4">
          <label for="sublocation-name" class="block text-sm font-medium text-gray-700 mb-2">
            Sublocation Name *
          </label>
          <input
            id="sublocation-name"
            v-model="formData.name"
            type="text"
            required
            class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            :class="{ 'border-red-500': errors.name }"
            placeholder="Enter sublocation name"
          />
          <p v-if="errors.name" class="mt-1 text-sm text-red-600">{{ errors.name }}</p>
        </div>

        <!-- Address Field -->
        <div class="mb-6">
          <label for="sublocation-address" class="block text-sm font-medium text-gray-700 mb-2">
            Address
          </label>
          <textarea
            id="sublocation-address"
            v-model="formData.address"
            rows="3"
            class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            :class="{ 'border-red-500': errors.address }"
            placeholder="Enter sublocation address (optional)"
          ></textarea>
          <p v-if="errors.address" class="mt-1 text-sm text-red-600">{{ errors.address }}</p>
        </div>

        <!-- Error Message -->
        <div v-if="error" class="mb-4 p-3 bg-red-50 border border-red-200 rounded-md">
          <p class="text-sm text-red-600">{{ error }}</p>
        </div>

        <!-- Action Buttons -->
        <div class="flex justify-end space-x-3">
          <button
            type="button"
            @click="$emit('close')"
            class="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
            :disabled="isCreating"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="px-4 py-2 text-sm font-medium text-white bg-blue-600 border border-transparent rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
            :disabled="isCreating"
          >
            <span v-if="isCreating" class="flex items-center">
              <svg class="animate-spin -ml-1 mr-2 h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Creating...
            </span>
            <span v-else>Create Sublocation</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { reactive, watch } from 'vue';
import { useCreateSubLocation } from '@/composables/organization/sublocation/useCreateSubLocation';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  locationId: {
    type: String,
    required: true
  }
});

const emit = defineEmits(['close', 'sublocation-added']);

const { isCreating, error, createdSubLocation, createSubLocation } = useCreateSubLocation();

const formData = reactive({
  name: '',
  address: ''
});

const errors = reactive({
  name: '',
  address: ''
});

// Reset form when modal is opened/closed
watch(() => props.isVisible, (newValue, oldValue) => {
  if (newValue) {
    resetForm();
  }
});

// Watch for successful creation
watch(createdSubLocation, (newSubLocation, oldSubLocation) => {
  if (newSubLocation) {
    emit('sublocation-added', newSubLocation);
    resetForm();
  }
});

// Watch for errors
watch(error, (newError, oldError) => {
});

// Watch for loading state
watch(isCreating, (newState, oldState) => {
});

const resetForm = () => {
  formData.name = '';
  formData.address = '';
  errors.name = '';
  errors.address = '';
};

const validateForm = () => {
  let isValid = true;
  
  // Reset errors
  errors.name = '';
  errors.address = '';
  
  // Validate name (required)
  if (!formData.name.trim()) {
    errors.name = 'Sublocation name is required';
    isValid = false;
  } else if (formData.name.trim().length < 2) {
    errors.name = 'Sublocation name must be at least 2 characters';
    isValid = false;
  } else {
  }
  
  // Validate address (optional, but if provided, must be valid)
  if (formData.address.trim() && formData.address.trim().length < 5) {
    errors.address = 'Address must be at least 5 characters if provided';
    isValid = false;
  } else {
  }
  
  return isValid;
};

const handleSubmit = async () => {
  if (!validateForm()) {
    return;
  }
  
  const sublocationData = {
    name: formData.name.trim(),
    address: formData.address.trim() || '', // Allow empty address
    locationId: props.locationId
  };
  
  try {
    const result = await createSubLocation(sublocationData);
  } catch (err) {
    console.error('Error in handleSubmit:', err);
  }
};

</script>

<style scoped>
/* Modal backdrop animation */
.fixed {
  animation: fadeIn 0.2s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

/* Modal content animation */
.bg-white {
  animation: slideIn 0.2s ease-out;
}

@keyframes slideIn {
  from {
    transform: translateY(-20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

/* Focus styles */
input:focus,
textarea:focus {
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

/* Error state styles */
.border-red-500:focus {
  box-shadow: 0 0 0 3px rgba(239, 68, 68, 0.1);
}
</style>