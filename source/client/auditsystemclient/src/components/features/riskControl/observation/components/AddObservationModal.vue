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
                        <form @submit.prevent="handleSubmit" class="space-y-4">
                            <!--  Observation Description Field -->
                            <div>
                                <label for="observation-description" class="block text-sm font-medium text-gray-700">
                                    Observation Description *
                                </label>
                                <BaseInput
                                    id="observation-description"
                                    v-model="formData.description"
                                    label="Observation Description"
                                    placeholder="Enter Observation Description"
                                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.description ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                                />
                                <p v-if="errors.description" class="text-error text-sm mt-1">{{ errors.description }}</p>
                            </div>

                            <!--  Observation Background Field -->
                            <div>
                                <label for="observation-background" class="block text-sm font-medium text-gray-700">
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
                            <!--  Observation Action Owner Field -->
                            <div>
                                <label for="observation-action-owner" class="block text-sm font-medium text-gray-700">
                                    Observation Action Owner *
                                </label>
                                <BaseInput
                                    id="observation-action-owner"
                                    v-model="formData.actionOwner"
                                    label="Observation Action Owner"
                                    placeholder="Enter Observation Action Owner"
                                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.actionOwner ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                                />
                                <p v-if="errors.actionOwner" class="text-error text-sm mt-1">{{ errors.actionOwner }}</p>
                            </div>

                            <!--  Observation Action plan Field -->
                            <div>
                                <label for="observation-action-plan" class="block text-sm font-medium text-gray-700">
                                    Observation Action Plan *
                                </label>
                                <BaseInput
                                    id="observation-action-plan"
                                    v-model="formData.actionPlan"
                                    label="Observation Action Plan"
                                    placeholder="Enter Observation Action Plan"
                                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.actionPlan ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                                />
                                <p v-if="errors.actionPlan" class="text-error text-sm mt-1">{{ errors.actionPlan }}</p>
                            </div>

                            <!--  Observation Action date Field -->
                            <div>
                                 <label for="observation-action-date" class="block text-sm font-medium text-gray-700">
                                    Observation Action Date *
                                </label>
                                <BaseDateInput
                                    id="observation-action-date"
                                    v-model="formData.actionDate"
                                    placeholder="Select Observation Action Date"
                                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.actionDate ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                                />
                                <p v-if="errors.actionDate" class="text-error text-sm mt-1">{{ errors.actionDate }}</p>
                            </div>

                            <!--  Observation Note Field -->
                            <div>
                                 <label for="observation-note" class="block text-sm font-medium text-gray-700">
                                    Observation Note *
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
                                    :disabled="!formData.description.trim() || isCreating"
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
import { ref, watch } from 'vue';
import BaseInput from '@/components/base/BaseInput.vue';
import BaseDateInput from '@/components/base/BaseDateInput.vue';
import CloseIcon from '@/components/common/icons/CloseIcon.vue';
import { useCreateObservation } from '@/composables/riskControl/observation/useCreateObservation';
import { useNotification } from '@/composables/notification/useNotification';

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

const formData = ref({
    description: '',
    background: '',
    actionOwner: '',
    actionPlan: '',
    actionDate: '',
    note: ''
});

const errors = ref({
    description: '',
    background: '',
    actionOwner: '',
    actionPlan: '',
    actionDate: '',
    note: ''
});

watch(() => props.isVisible, (newValue) => {
    if (newValue) {
        resetForm();
    }
});

const resetForm = () => {
    formData.value = {
        description: '',
        background: '',
        actionOwner: '',
        actionPlan: '',
        actionDate: '',
        note: ''
    };
    errors.value = {
        description: '',
        background: '',
        actionOwner: '',
        actionPlan: '',
        actionDate: '',
        note: ''
    };

    resetState();
};

const validateForm = () => {
    errors.value = {
        description: '',
        background: '',
        actionOwner: '',
        actionPlan: '',
        actionDate: '',
        note: ''
    };

    let isValid = true;

    if (!formData.value.description.trim()) {
        errors.value.description = 'Observation description is required';
        isValid = false;
    }
    if (!formData.value.background.trim()) {
        errors.value.background = 'Observation background is required';
        isValid = false;
    }
    if (!formData.value.actionOwner.trim()) {
        errors.value.actionOwner = 'Observation action owner is required';
        isValid = false;
    }
    if (!formData.value.actionPlan.trim()) {
        errors.value.actionPlan = 'Observation action plan is required';
        isValid = false;
    }
    if (!formData.value.actionDate) {
        errors.value.actionDate = 'Observation action date is required';
        isValid = false;
    }

    return isValid;
};

const handleSubmit = async () => {
    if (!props.auditProgramId) {
        showNotification('audit program is required', 'error');
        return;
    }

    if (!validateForm()) {
        return;
    }

    try {
        const observationData = {
            description: formData.value.description.trim(),
            background: formData.value.background.trim(),
            actionOwner: formData.value.actionOwner.trim(),
            actionPlan: formData.value.actionPlan.trim(),
            actionDate: new Date(formData.value.actionDate).toISOString(),
            note: formData.value.note ? formData.value.note.trim() : null,
            auditProgramId: props.auditProgramId
        };

        const result = await createObservation(observationData);

        if (result?.success) {

            const newObservation = {
                id: result.data.observationId,
                ...observationData,
                createdAt: new Date().toISOString()
            };

            showNotification(`Observation "${newObservation.description}" has been added successfully!`, 'success');
            emit('observation-added', newObservation);
            emit('close');
        } else {
             showNotification(result?.message || 'Failed to create observation', 'error');
        }

    } catch (error) {
        console.error('Error creating observation:', error);
        showNotification('Failed to create observation. Please try again.', 'error');
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
