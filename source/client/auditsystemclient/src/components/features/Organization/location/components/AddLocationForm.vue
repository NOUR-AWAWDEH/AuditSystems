<template>
  <!--AddLocationForm.vue-->
  <div class="bg-white border border-gray-200 shadow-sm rounded-lg p-3 sm:p-4 md:p-6 mb-6">
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-4">
      <h2 class="text-lg font-semibold text-secondary-base">Add New Location</h2>

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
          class="w-4 h-4 mr-2"
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
        {{ showAddForm ? "Cancel" : "Add Location" }}
      </BaseButton>
    </div>

    <!-- Add Location Form -->
    <Transition name="slide-down">
      <div v-if="showAddForm" class="mt-4">
        <form @submit.prevent="handleAddLocation" class="space-y-4">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label
                for="location-name"
                class="block text-sm font-semibold text-secondary-base mb-2"
              >
                Location Name
              </label>
              <BaseInput
                id="location-name"
                v-model="newLocation.name"
                placeholder="Enter location name"
                required
              />
            </div>
            <div>
              <label
                for="location-address"
                class="block text-sm font-semibold text-secondary-base mb-2"
              >
                Address
              </label>
              <BaseInput
                id="location-address"
                v-model="newLocation.address"
                placeholder="Enter location address"
              />
            </div>
          </div>

          <div class="flex flex-col sm:flex-row justify-end gap-3">
            <BaseButton
              type="submit"
              :loading="isCreating"
              :disabled="!newLocation.name.trim()"
              class="w-full sm:w-auto px-6 py-2 bg-secondary-base hover:bg-secondary-shade rounded"
            >
              {{ isCreating ? "Adding..." : "Add Location" }}
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
import { ref, onMounted } from "vue";
import BaseButton from "@/components/base/BaseButton.vue";
import BaseInput from "@/components/base/BaseInput.vue";
import { useCreateLocation } from "@/composables/organization/location/useCreateLocation";
import { useNotification } from "@/composables/notification/useNotification";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";

const emit = defineEmits(["location-added"]);

const {initCheck } = useAuthStatus();

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
const newLocation = ref({
  name: "",
  address: "",
});

const { showNotification } = useNotification();
const { isCreating, error, createLocation, clearState } = useCreateLocation();

const toggleAddForm = () => {
  showAddForm.value = !showAddForm.value;
  if (!showAddForm.value) {
    resetForm();
  }
};

const resetForm = () => {
  newLocation.value = {
    name: "",
    address: "",
  };
  clearState();
};

const handleAddLocation = async () => {
  if (!props.companyId) {
    error.value = "Company is required. Please make sure you are logged in.";
    return;
  }

  const locationData = {
    name: newLocation.value.name,
    address: newLocation.value.address,
    companyId: props.companyId,
    createdAt: new Date().toISOString(),
  };

  const result = await createLocation(locationData);

  if (result.success) {
    const fullLocation = {
      id: result.data.result.locationId,
      ...locationData,
    };

    showNotification(`Location "${fullLocation.name}" has been added successfully!`, "success");
    emit("location-added", fullLocation);
    resetForm();
    showAddForm.value = false;
  } else {
    showNotification(`Failed to add Location. ${result.message || ""}`, "error");
    error.value =result?.message || "Failed to create location";
  }
};
</script>
