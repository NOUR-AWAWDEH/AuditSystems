<template>
  <!-- AddJobTitle.vue -->
  <div class="bg-white border border-gray-200 shadow-md rounded-lg p-3 sm:p-4 md:p-6 my-6 ">
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-4">
      <h2 class="text-lg font-semibold text-secondary-base">Add New Job Title</h2>

      <!-- Toggle Button -->
      <BaseButton
        @click="toggleAddForm"
        :class="
          showAddForm
            ? 'w-full sm:w-auto px-3 py-2 rounded-md text-sm font-medium bg-red-600 hover:bg-red-700 text-white'
            : 'w-full sm:w-auto px-3 py-2 rounded-md text-sm font-medium bg-secondary-base hover:bg-secondary-shade text-white'
        "
      >
        <svg
          v-if="!showAddForm"
          class="w-4 h-4"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M12 6v6m0 0v6m0-6h6m-6 0H6"
          ></path>
        </svg>

        {{ showAddForm ? "Cancel" : "Add Job Title" }}
      </BaseButton>
    </div>

    <!-- Add Job Title Form -->
    <Transition name="slide-down">
      <div v-if="showAddForm" class="mt-4">
        <form @submit.prevent="handleAddJobTitle" class="space-y-4">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label
                for="job-title-name"
                class="block text-sm font-semibold text-secondary-base mb-2"
              >
                Job Title Name
              </label>
              <BaseInput
                id="job-title-name"
                v-model="newJobTitle.name"
                placeholder="e.g. Senior Auditor"
                required
              />
            </div>
          </div>

          <!-- Actions -->
          <div class="flex flex-col sm:flex-row justify-end gap-3">
            <BaseButton
              type="submit"
              :loading="isCreating"
              :disabled="!newJobTitle.name.trim()"
              class="w-full sm:w-auto px-6 py-2 bg-secondary-base hover:bg-secondary-shade rounded"
            >
              {{ isCreating ? "Adding..." : "Add Job Title" }}
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
import { ref, onMounted, watch } from "vue";
import BaseButton from "@/components/base/BaseButton.vue";
import BaseInput from "@/components/base/BaseInput.vue";
import { useCreateJobTitle } from "@/composables/organization/jobTitle/useCreateJobTitle";
import { useNotification } from "@/composables/notification/useNotification";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";

const emit = defineEmits(["job-title-added"]);

const { initCheck } = useAuthStatus();

onMounted(() => {
  initCheck();
});

const props = defineProps({
  companyId: {
    type: String,
    required: false,
    default: null,
  },
});

const showAddForm = ref(false);
const newJobTitle = ref({
  name: "",
});

const { showNotification } = useNotification();
const { isCreating, error, createJobTitle, clearState } = useCreateJobTitle();

const toggleAddForm = () => {
  showAddForm.value = !showAddForm.value;
  if (!showAddForm.value) {
    resetForm();
  }
};

const resetForm = () => {
  newJobTitle.value = {
    name: "",
  };
  clearState();
};

watch(error, (newValue) => {
  if (newValue) {
    setTimeout(() => {
      error.value = null;
    }, 3000);
  }
});

const handleAddJobTitle = async () => {
  if (!props.companyId) {
    error.value = "Company is required. Please make sure you are logged in.";
    return;
  }

  const jobTitleData = {
    name: newJobTitle.value.name.trim(),
    companyId: props.companyId,
    createdAt: new Date().toISOString(),
  };

  const result = await createJobTitle(jobTitleData);
  if (result.success) {
    const fullJobTitle = {
      id: result.data.result.jobTitleId,
      ...jobTitleData,
    };

    showNotification(`Job Title "${fullJobTitle.name}" has been added successfully!`, "success");
    emit("job-title-added", fullJobTitle);
    resetForm();
    showAddForm.value = false;
  } else {
    showNotification(`Failed to add Job Title. ${result?.message || ""}`, "error");
    error.value = result?.message || "Failed to add Job Title.";
  }
};
</script>
