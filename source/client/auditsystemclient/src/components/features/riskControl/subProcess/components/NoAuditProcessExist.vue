<template>
  <div class="text-center py-20">
    <div class="max-w-lg mx-auto">
      <div
        class="bg-linear-to-br from-primary-soft to-primary-tint rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-primary-base"
      >
        <ProcessIcon size="xxxl" custom-class="text-primary-shade" />
      </div>
      <h3 class="text-2xl font-bold text-secondary-shade mb-4">No Audit Processes Available</h3>
      <p class="text-gray-base mb-6 text-sm leading-relaxed">
        You need to create Audit processes first before you can manage sub-processes
      </p>
      <div class="bg-white border border-tertiary-shade rounded-lg p-6">
        <div class="flex items-start space-x-3">
          <div class="shrink-0">
            <svg
              class="w-6 h-6 text-tertiary-shade"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z"
              ></path>
            </svg>
          </div>
          <div class="text-left">
            <h4 class="text-sm font-medium text-tertiary-shade mb-2">Getting Started</h4>
            <ol class="text-sm text-gray-base space-y-1 list-decimal list-inside">
              <li>Navigate to the Risk Control section</li>
              <li>Create or select an audit process</li>
              <li>Return here to manage sub-processes</li>
            </ol>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import ProcessIcon from "@/components/common/icons/ProcessIcon.vue";
</script>
