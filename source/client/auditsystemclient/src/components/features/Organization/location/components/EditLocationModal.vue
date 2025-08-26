<template>
  <div v-if="isVisible" class="fixed inset-0 bg-gray-base bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white rounded-lg p-6 w-full max-w-md mx-4">
      <h2 class="text-xl font-semibold mb-4 text-secondary-shade">Edit Location</h2>

      <form @submit.prevent="handleSubmit">
        <div class="mb-4">
          <label for="edit-name" class="block text-sm font-medium text-secondary-base mb-2">
            Location Name
          </label>
          <BaseInput
            id="edit-name"
            v-model="formData.name"
            type="text"
            placeholder="Enter location name"
            :input-class="'w-full px-3 py-2 border border-gray-tint rounded-md focus:outline-none focus:ring-2 focus:ring-secondary-base text-secondary-shade'"
          />
        </div>

        <div class="mb-6">
          <label for="edit-address" class="block text-sm font-medium text-secondary-base mb-2">
            Address
          </label>
          <textarea
            id="edit-address"
            v-model="formData.address"
            rows="3"
            class="w-full px-3 py-2 border border-gray-tint rounded-md focus:outline-none focus:ring-2 focus:ring-secondary-base text-secondary-shade"
            placeholder="Enter address (optional)"
          ></textarea>
        </div>

        <div class="flex justify-end space-x-3">
          <BaseButton
            @click="$emit('close')"
            :disabled="isUpdating"
            class="px-4 py-2 text-white border border-error rounded-md hover:bg-red-800 bg-error"
          >
            Cancel
          </BaseButton>
          <BaseButton
            @click="handleSubmit"
            :disabled="isUpdating || !formData.name.trim() || !authStatus?.companyId"
            :is-loading="isUpdating"
            class="px-4 py-2 bg-primary-base text-white rounded-md hover:bg-primary-shade disabled:opacity-50"
          >
            <template #loading>Updating...</template>
            Update Location
          </BaseButton>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, onMounted } from 'vue';
import BaseInput from '@/components/base/BaseInput.vue';
import BaseButton from '@/components/base/BaseButton.vue';
import { useUpdateLocationById } from '@/composables/organization/location/useUpdateLocationById';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false
  },
  location: {
    type: Object,
    default: null
  }
});

const emit = defineEmits(['close', 'location-updated']);

const { isUpdating, updateLocation } = useUpdateLocationById();
const { authStatus, checkAuthStatus } = useAuthStatus();

const formData = ref({
  name: '',
  address: ''
});

// Initialize auth status when component mounts
onMounted(async () => {
  if (!authStatus.value) {
    await checkAuthStatus();
  }
});

watch(() => props.location, (newLocation) => {
  if (newLocation) {
    formData.value = {
      name: newLocation.name || '',
      address: newLocation.address || ''
    };
  }
}, { immediate: true });

const handleSubmit = async () => {
  if (!props.location?.id) return;

  // Ensure we have a valid companyId before proceeding
  const companyId = authStatus.value?.companyId;
  if (!companyId) {
    console.error('CompanyId is missing from auth status:', authStatus.value);
    // Try to refresh auth status
    await checkAuthStatus();
    const refreshedCompanyId = authStatus.value?.companyId;
    if (!refreshedCompanyId) {
      console.error('Still no companyId after refresh');
      return;
    }
  }

  const result = await updateLocation({
    id: props.location.id,
    name: formData.value.name.trim(),
    address: formData.value.address.trim() || null,
    companyId: authStatus.value.companyId
  });

  if (result.success) {

    emit('location-updated', {
      ...props.location,
      name: formData.value.name.trim(),
      address: formData.value.address.trim() || null
    });
    
    emit('close');
  }
};
</script>

<style scoped>
/* Enhanced mobile responsiveness matching LocationView.vue */
@media (max-width: 640px) {
  .mx-4 {
    margin-left: 0.75rem;
    margin-right: 0.75rem;
  }

  .p-6 {
    padding: 1rem;
  }
}

@media (max-width: 480px) {
  .mx-4 {
    margin-left: 0.5rem;
    margin-right: 0.5rem;
  }

  .p-6 {
    padding: 0.75rem;
  }
}
</style>
