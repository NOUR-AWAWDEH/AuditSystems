<template>
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 md:p-6 mb-6">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between mb-4">
            <h2 class="text-lg font-semibold text-secondary-base mb-2 sm:mb-0">
                Add New Location
            </h2>
            <BaseButton
                @click="toggleAddForm"
                :class="showAddForm ? 'px-4 py-2 rounded-md text-sm font-medium bg-red-600 hover:bg-red-700 text-white' : 'px-4 py-2 rounded-md text-sm font-medium bg-primary-base'"
            >
                {{ showAddForm ? 'Cancel' : 'Add Location' }}
            </BaseButton>
        </div>

        <!-- Add Location Form -->
        <Transition name="slide-down">
            <div v-if="showAddForm" class="mt-4">
                <form @submit.prevent="handleAddLocation" class="space-y-4">
                    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <BaseInput
                            id="location-name"
                            v-model="newLocation.name"
                            label="Location Name"
                            placeholder="Enter location name"
                            required
                        />
                        <BaseInput
                            id="location-address"
                            v-model="newLocation.address"
                            label="Address"
                            placeholder="Enter location address"
                        />
                    </div>
                    
                    <div class="flex justify-end space-x-3">
                        <BaseButton
                            type="submit"
                            :loading="isCreating"
                            :disabled="!newLocation.name.trim()"
                            class="px-6 py-2 bg-primary-base rounded"
                        >
                            {{ isCreating ? 'Adding...' : 'Add Location' }}
                        </BaseButton>
                    </div>
                </form>
                
                <!-- Error Display -->
                <div v-if="error" class="mt-4 p-3 bg-red-50 border border-red-200 rounded-md">
                    <p class="text-red-600 text-sm">{{ error }}</p>
                </div>
            </div>
        </Transition>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import BaseButton from '@/components/base/BaseButton.vue';
import BaseInput from '@/components/base/BaseInput.vue';
import { useCreateLocation } from '@/composables/organization/location/useCreateLocation';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';

const emit = defineEmits(['location-added']);

const showAddForm = ref(false);
const newLocation = ref({
    name: '',
    address: ''
});

const { isCreating, error, createLocation, resetState } = useCreateLocation();
const { authStatus, checkAuthStatus, initCheck } = useAuthStatus();

// Initialize authentication check on component mount
onMounted(async () => {
    await initCheck();
});

const toggleAddForm = () => {
    showAddForm.value = !showAddForm.value;
    if (!showAddForm.value) {
        resetForm();
    }
};

const resetForm = () => {
    newLocation.value = {
        name: '',
        address: ''
    };
    resetState();
};

const handleAddLocation = async () => {
    if (!authStatus.value) {
        await checkAuthStatus();
    }
    
    if (!authStatus.value?.userId) {
        error.value = 'User not authenticated';
        return;
    }
    
    if (!authStatus.value?.companyId) {
        error.value = 'Company ID is required';
        return;
    }

    const locationData = {
        name: newLocation.value.name,
        address: newLocation.value.address,
        userId: authStatus.value.userId,
        companyId: authStatus.value.companyId
    };

    const result = await createLocation(locationData);

    if (result.success) {
        emit('location-added', {
            id: result.data.id,
            ...newLocation.value
        });
        resetForm();
        showAddForm.value = false;
    }
};
</script>

<style scoped>
.slide-down-enter-active,
.slide-down-leave-active {
    transition: all 0.3s ease;
    max-height: 500px;
    overflow: hidden;
}

.slide-down-enter-from,
.slide-down-leave-to {
    max-height: 0;
    opacity: 0;
    transform: translateY(-10px);
}
</style>