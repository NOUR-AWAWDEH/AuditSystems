<template>
    <!--AddObservationModal.vue-->
    <Teleport to="body">
        <Transition name="modal">
            <div v-if="isVisible" class="fixed inset-0 z-50 flex items-center justify-center p-4">
                <div class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity" @click="$emit('close')"></div>

                <!-- Modal -->
                <div class="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all">
                    <!-- Header -->
                    <div class="flex items-center justify-between p-4 border-b border-gray-tint">
                        <h3 class="text-base font-medium text-secondary-shade font-poppins">Add Observation</h3>
                        <button
                            @click="$emit('close')"
                            class="text-gray-base hover:text-gray-shade focus:outline-none">
                            <CloseIcon size="md" custom-class="text-gray-400 hover:text-gray-600" />
                        </button>
                    </div>

                    <!-- Content -->
                    <div class="p-4">
                        <form @submit.prevent="handleSubmit" novalidate class="space-y-4">
                            <!--  Observation Highlight Field -->
                            <div>
                                <label for="observation-highlight" class="block text-sm font-medium text-gray-700 mb-3">
                                    Observation Highlight *
                                </label>
                                <BaseInput
                                    id="observation-highlight"
                                    v-model="formData.highlight"
                                    label="Observation Highlight"
                                    placeholder="Enter Observation Highlight"
                                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.highlight ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                                />
                                <p v-if="errors.highlight" class="text-error text-sm mt-1">{{ errors.highlight }}</p>
                            </div>

                            <!--  Observation Background Field -->
                            <div>
                                <label for="observation-background" class="block text-sm font-medium text-gray-700 mb-3">
                                    Observation Background *
                                </label>
                                <BaseInput
                                    id="observation-background"
                                    v-model="formData.background"
                                    label="Observation Background"
                                    placeholder="Enter Observation Background"
                                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.background ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                                />
                                <p v-if="errors.background" class="text-error text-sm mt-1">{{ errors.background }}</p>
                            </div>
                            <!--  Observation Is Active Field -->
                            <div>
                                <label for="observation-is-active" class="block text-sm font-medium text-gray-700 mb-3">
                                    Observation Is Active *
                                </label>
                                <div class="flex gap-6">
                                    <label class="flex items-center cursor-pointer">
                                        <input
                                            type="radio"
                                            id="is-active-yes"
                                            value="true"
                                            v-model="formData.isActive"
                                            class="w-4 h-4 text-primary-base border-gray-300 focus:ring-primary-base focus:ring-2"
                                        />
                                        <span class="ml-2 text-sm text-gray-700">Yes</span>
                                    </label>
                                    <label class="flex items-center cursor-pointer">
                                        <input
                                            type="radio"
                                            id="is-active-no"
                                            value="false"
                                            v-model="formData.isActive"
                                            class="w-4 h-4 text-primary-base border-gray-300 focus:ring-primary-base focus:ring-2"
                                        />
                                        <span class="ml-2 text-sm text-gray-700">No</span>
                                    </label>
                                </div>
                                <p v-if="errors.isActive" class="text-error text-sm mt-1">{{ errors.isActive }}</p>
                            </div>

                            <!--  Observation Rate Field -->
                            <div>
                                <label for="observation-rate" class="block text-sm font-medium text-gray-700 mb-3">
                                    Observation Rate *
                                </label>
                                <RateSelector
                                    :rates="rates"
                                    :selectedRateId="formData.rateId"
                                    @update:selectedRateId="handleRateChange"
                                />
                                <p v-if="errors.rateId" class="text-error text-sm mt-1">{{ errors.rateId }}</p>
                            </div>

                            <!--  Observation Note Field -->
                            <div>
                                 <label for="observation-note" class="block text-sm font-medium text-gray-700 mb-3">
                                    Observation Note
                                </label>
                                <BaseInput
                                    id="observation-note"
                                    v-model="formData.note"
                                    label="Observation Note"
                                    placeholder="Enter Observation Note"
                                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.note ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                                />
                                <p v-if="errors.note" class="text-error text-sm mt-1">{{ errors.note }}</p>
                            </div>


                            <!--Error Display -->
                            <div v-if="error" class="p-3 bg-soft-error border border-error rounded-md">
                                <p class="text-error text-sm">{{ error }}</p>
                            </div>

                            <!-- Action Buttons -->
                            <div class="flex justify-end space-x-3 pt-4">
                                <button
                                    type="button"
                                    @click="$emit('close')"
                                    :disabled="isCreating"
                                    class="px-4 py-2 text-sm font-medium text-white bg-error border border-gray-tint rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                                    Cancel
                                </button>

                                <button
                                    type="submit"
                                    :disabled="!isFormValid || isCreating"
                                    class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50">
                                    <span v-if="isCreating">Adding...</span>
                                    <span v-else>Add Observation</span>
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
import { ref, watch, computed, onMounted } from 'vue';
import BaseInput from '@/components/base/BaseInput.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import { useCreateObservation } from '@/composables/riskControl/observation/useCreateObservation';
import { useNotification } from '@/composables/notification/useNotification';
import RateSelector from '@/components/common/rate/RateSelector.vue';
import { useAuthStatus } from '@/composables/auth/useAuthStatus';
import { useSearchRates } from '@/composables/common/rate/useSearchRates';

const props = defineProps({
    isVisible: {
        type: Boolean,
        default: false
    },
    auditProgramId: {
        type: [String, Number],
        default: null
    }
});

const emit = defineEmits(['close', 'observation-added']);

const { showNotification } = useNotification();
const { isCreating, error, createObservation, resetState } = useCreateObservation();
const { authStatus, checkAuthStatus } = useAuthStatus();
const { searchResults: rates, searchRates } = useSearchRates();

const formData = ref({
    highlight: '',
    background: '',
    isActive: '',
    rateId: '',
    note: ''
});

const errors = ref({
    highlight: '',
    background: '',
    isActive: '',
    rateId: '',
    note: ''
});

// Computed property to check if form is valid
const isFormValid = computed(() => {
    return formData.value.highlight.trim() &&
           formData.value.background.trim() &&
           (formData.value.isActive === 'true' || formData.value.isActive === 'false' || formData.value.isActive === true || formData.value.isActive === false) &&
           formData.value.rateId;
});

const handleRateChange = (value) => {
    formData.value.rateId = value;
};

watch(() => props.isVisible, (newValue) => {
    if (newValue) {
        resetForm();
        loadRates();
    }
});

onMounted(async () => {
    if (!authStatus.value) await checkAuthStatus();
    await loadRates();
});

const loadRates = async () => {
    try {
        await searchRates({ searchTerm: '', pageNumber: 1, pageSize: 100 });
    } catch (err) {
        console.error('Error loading rates:', err);
    }
};

const resetForm = () => {
    formData.value = {
        highlight: '',
        background: '',
        isActive: '',
        rateId: '',
        note: ''
    };
    errors.value = {
        highlight: '',
        background: '',
        isActive: '',
        rateId: '',
        note: ''
    };

    resetState();
};

const validateForm = () => {
    errors.value = {
        highlight: '',
        background: '',
        isActive: '',
        rateId: '',
        note: ''
    };

    let isValid = true;

    if (!formData.value.highlight.trim()) {
        errors.value.highlight = 'Observation highlight is required';
        isValid = false;
    }
    if (!formData.value.background.trim()) {
        errors.value.background = 'Observation background is required';
        isValid = false;
    }
    if (formData.value.isActive === '' || formData.value.isActive === null || formData.value.isActive === undefined) {
        errors.value.isActive = 'Observation is active is required';
        isValid = false;
    }
    if (!formData.value.rateId) {
        errors.value.rateId = 'Observation rate is required';
        isValid = false;
    }

    return isValid;
};

const handleSubmit = async () => {
    if (!props.auditProgramId) {
        showNotification('Audit program is required', 'error');
        return;
    }

    if (!validateForm()) {
        return;
    }

    try {
        const observationData = {
            highlight: formData.value.highlight.trim(),
            background: formData.value.background.trim(),
            isActive: formData.value.isActive === 'true' || formData.value.isActive === true,
            rateId: String(formData.value.rateId),
            note: formData.value.note ? formData.value.note.trim() : null,
            auditProgramId: String(props.auditProgramId)
        };

        const result = await createObservation(observationData);

        if (result?.success) {

            const newObservation = {
                id: result.data.observationId,
                ...observationData,
                createdAt: new Date().toISOString()
            };

            showNotification(`Observation "${newObservation.highlight}" has been added successfully!`, 'success');
            emit('observation-added', newObservation);
            emit('close');
        } else {
            const errorMessage = result?.message || result?.errors?.[0]?.message || error.value || 'Failed to create observation';
            console.error('Create observation failed:', result);
            showNotification(errorMessage, 'error');
        }

    } catch (err) {
        console.error('Error creating observation:', err);
        const errorMessage = err.response?.data?.errors?.[0]?.message || err.response?.data?.message || err.message || 'Failed to create observation. Please try again.';
        showNotification(errorMessage, 'error');
    }
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
