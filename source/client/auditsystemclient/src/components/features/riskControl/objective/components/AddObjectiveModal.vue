<template>
    <!--AddObjectiveModal.vue-->
    <Teleport to="body">
        <Transition name="modal">
            <div v-if="isVisible" class="fixed inset-0 flex items-center justify-center p-4">
                <div class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity" @click="$emit('close')"></div>

                <!-- Modal -->
                <div class="relative bg-white rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all">
                    <!-- Header -->
                    <div class="flex items-center justify-between p-4 border-b border-gray-tint">
                        <h3 class="text-base font-medium text-secondary-shade font-poppins">Add Objective</h3>
                        <button
                            @click="$emit('close')"
                            class="text-gray-base hover:text-gray-shade focus:outline-none">
                            <CloseIcon size="md" custom-class="text-gray-400 hover:text-gray-600" />
                        </button>
                    </div>

                    <!-- Content -->
                    <div class="p-4">
                        <form @submit.prevent="handleSubmit" class="space-y-4">
                            <!--  Objective description Field -->
                            <div>
                                <label for="objective-description" class="block text-sm font-medium text-gray-700">
                                    Objective Description *
                                </label>
                                <BaseInput
                                    id="objective-description"
                                    v-model="formData.description"
                                    label="Objective Description"
                                    placeholder="Enter Objective Description"
                                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${errors.description ? 'border-error' : 'border-gray-tint'} focus:ring-primary-base`"
                                />
                                <p v-if="errors.description" class="text-error text-sm mt-1">{{ errors.description }}</p>
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
                                    <span v-else>Add Objective</span>
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
import { useCreateObjective } from '@/composables/riskControl/objective/useCreateObjective.js';
import { useNotification } from '@/composables/notification/useNotification';

const props = defineProps({
    isVisible: {
        type: Boolean,
        default: false
    },
    riskControlMatrixId: {
        type: [String, Number],
        default: null
    }
});

const emit = defineEmits(['close', 'objective-added']);

const { showNotification } = useNotification();
const { isCreating, error, createObjective, resetState } = useCreateObjective();

const formData = ref({
    description: ''
});

const errors = ref({
    description: ''
});

watch(() => props.isVisible, (newValue) => {
    if (newValue) {
        resetForm();
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

const handleSubmit = async () => {
    if (!props.riskControlMatrixId) {
        showNotification('risk control matrix is required', 'error');
        return;
    }

    if (!validateForm()) {
        return;
    }

    try {
        const objectiveData = {
            description: formData.value.description.trim(),
            riskControlMatrixId: props.riskControlMatrixId
        };

        const result = await createObjective(objectiveData);

        if (result?.success) {
            const newObjective = {
                id: result.data.objectiveId,
                ...objectiveData,
                createdAt: new Date().toISOString(),
            };

            showNotification(`Objective "${newObjective.description}" has been added successfully!`, 'success');
            emit('objective-added', newObjective);
            emit('close');
        } else {
             showNotification(result?.message || 'Failed to create objective', 'error');
        }

    } catch (error) {
        console.error('Error creating objective:', error);
        showNotification('Failed to create objective. Please try again.', 'error');
    }
};

const validateForm = () => {
    errors.value = {
        description: ''
    };

    let isValid = true;

    if (!formData.value.description.trim()) {
        errors.value.description = 'Objective description is required';
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
