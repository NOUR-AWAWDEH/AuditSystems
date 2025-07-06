<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 z-50 flex items-center justify-center p-4">
        <!-- Backdrop -->
        <div 
          class="absolute inset-0 bg-gray-shade bg-opacity-80 transition-opacity"
          @click="onCancel"
        ></div>
        
        <!-- Dialog -->
        <div class="relative bg-white rounded-lg shadow-xl max-w-sm sm:max-w-md w-full mx-4 transform transition-all">
          <!-- Header -->
          <div class="flex items-center p-3 sm:p-4 border-b border-gray-tint">
            <div class="flex-shrink-0 w-6 h-6 sm:w-8 sm:h-8 bg-soft-error rounded-full flex items-center justify-center">
              <svg class="w-4 h-4 sm:w-5 sm:h-5 text-error" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z" />
              </svg>
            </div>
            <h3 class="ml-2 sm:ml-3 text-sm sm:text-base font-medium text-gray-shade font-poppins">{{ title }}</h3>
          </div>
          
          <!-- Content -->
          <div class="p-3 sm:p-4">
            <p class="text-xs sm:text-sm text-gray-base font-poppins leading-relaxed" v-html="formattedMessage"></p>
          </div>
          
          <!-- Actions -->
          <div class="flex flex-col gap-2 p-3 sm:p-4 border-t border-gray-tint">
            <button
              @click="onConfirm"
              class="w-full px-3 py-2.5 text-sm font-medium text-white bg-error border border-transparent rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-error transition-colors font-poppins"
            >
              {{ confirmText }}
            </button>
            <button
              @click="onCancel"
              class="w-full px-3 py-2.5 text-sm font-medium text-gray-shade bg-white border border-gray-tint rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base transition-colors font-poppins"
            >
              {{ cancelText }}
            </button>
          </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<script setup>
import { ref, watch, computed } from 'vue';

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  title: {
    type: String,
    default: 'Confirm Action'
  },
  message: {
    type: String,
    required: true
  },
  confirmText: {
    type: String,
    default: 'Delete'
  },
  cancelText: {
    type: String,
    default: 'Cancel'
  },
  highlightText: {
    type: String,
    default: ''
  }
});

const emit = defineEmits(['update:modelValue', 'confirm', 'cancel']);

const isVisible = ref(props.modelValue);

// Format message to highlight specific text in red
const formattedMessage = computed(() => {
  if (!props.highlightText) {
    return props.message;
  }
  
  // Escape HTML and highlight the specified text
  const escapedMessage = props.message.replace(/</g, '&lt;').replace(/>/g, '&gt;');
  const escapedHighlight = props.highlightText.replace(/</g, '&lt;').replace(/>/g, '&gt;');
  
  return escapedMessage.replace(
    new RegExp(`"${escapedHighlight}"`, 'g'),
    `"<span class="text-error font-medium">${escapedHighlight}</span>"`
  );
});

watch(() => props.modelValue, (newValue) => {
  isVisible.value = newValue;
});

const onConfirm = () => {
  emit('confirm');
  emit('update:modelValue', false);
};

const onCancel = () => {
  emit('cancel');
  emit('update:modelValue', false);
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

/* Ensure text doesn't break awkwardly */
.leading-relaxed {
  word-break: break-word;
  hyphens: auto;
}

/* Improve button touch targets on mobile */
@media (max-width: 640px) {
  button {
    min-height: 44px; /* iOS recommended touch target */
  }
}
</style>