<template>
  <!--ActionOwnersSelector.vue-->
  <div class="relative" ref="containerRef">
    <label class="block text-sm font-medium text-secondary-shade mb-1"> Select Action Owner </label>

    <div class="relative">
      <!-- Trigger Button -->
      <button
        type="button"
        @click="toggleDropdown"
        class="w-full bg-white border rounded-md px-4 py-2 text-left focus:outline-none focus:ring-2 focus:ring-primary-base transition-all duration-200 flex items-center justify-between"
        :class="[
          actionOwnerError ? 'border-error' : 'border-gray-tint',
          isOpen ? 'ring-2 ring-primary-base border-primary-base' : '',
        ]"
      >
        <span v-if="selectedOwner" class="block truncate">
          <span class="font-medium text-secondary-shade">
            {{ selectedOwner.firstName }} {{ selectedOwner.lastName }}
          </span>
        </span>
        <span v-else class="text-gray-base">Choose an action owner</span>

        <span class="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-2">
          <svg
            class="h-5 w-5 text-gray-400 transition-transform duration-200"
            :class="{ 'rotate-180': isOpen }"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 20 20"
            fill="currentColor"
            aria-hidden="true"
          >
            <path
              fill-rule="evenodd"
              d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
              clip-rule="evenodd"
            />
          </svg>
        </span>
      </button>

      <!-- Dropdown Menu -->
      <transition
        enter-active-class="transition ease-out duration-100"
        enter-from-class="transform opacity-0 scale-95"
        enter-to-class="transform opacity-100 scale-100"
        leave-active-class="transition ease-in duration-75"
        leave-from-class="transform opacity-100 scale-100"
        leave-to-class="transform opacity-0 scale-95"
      >
        <div
          v-if="isOpen"
          class="absolute z-10 mt-1 w-full bg-white shadow-xl max-h-80 rounded-md py-1 text-base ring-1 ring-black ring-opacity-5 overflow-auto focus:outline-none sm:text-sm"
        >
          <ul tabindex="-1" role="listbox">
            <li
              v-for="owner in actionOwners"
              :key="owner.id"
              class="cursor-pointer select-none relative py-4 pl-4 pr-9 hover:bg-primary-soft transition-colors duration-150 border-b border-gray-50 last:border-0"
              :class="{ 'bg-primary-soft': isSelected(owner) }"
              @click="selectOwner(owner)"
            >
              <div class="flex flex-col space-y-1">
                <span
                  class="font-semibold block truncate text-lg"
                  :class="isSelected(owner) ? 'text-primary-shade' : 'text-gray-900'"
                >
                  {{ owner.firstName }} {{ owner.lastName }}
                </span>
                <span
                  class="text-sm block truncate"
                  :class="isSelected(owner) ? 'text-primary-base' : 'text-gray-600'"
                >
                  <span class="font-medium text-gray-500 mr-1">Job Title:</span>
                  {{ owner.jobTitle?.name || "No Job Title" }}
                </span>
                <!-- Sub Department -->
                <span v-if="owner.subDepartment" class="text-xs text-gray-500 block truncate">
                  <span class="font-medium text-gray-400 mr-1">Sub-Dept:</span>
                  {{ owner.subDepartment.name }}
                </span>
              </div>

              <span
                v-if="isSelected(owner)"
                class="text-primary-base absolute inset-y-0 right-0 flex items-center pr-4"
              >
                <svg
                  class="h-6 w-6"
                  xmlns="http://www.w3.org/2000/svg"
                  viewBox="0 0 20 20"
                  fill="currentColor"
                  aria-hidden="true"
                >
                  <path
                    fill-rule="evenodd"
                    d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                    clip-rule="evenodd"
                  />
                </svg>
              </span>
            </li>

            <div v-if="actionOwners.length === 0" class="py-4 px-3 text-gray-base text-center">
              No action owners found.
            </div>
          </ul>
        </div>
      </transition>
    </div>

    <!-- Error Message -->
    <p v-if="actionOwnerError" class="text-error text-sm mt-1">
      {{ actionOwnerError }}
    </p>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from "vue";

const props = defineProps({
  actionOwners: {
    type: Array,
    default: () => [],
  },
  selectedActionOwnerId: {
    type: String,
    default: "",
  },
  actionOwnerError: {
    type: String,
    default: "",
  },
});

const emit = defineEmits(["update:selectedActionOwner", "action-owner-changed"]);

const isOpen = ref(false);
const containerRef = ref(null);

const selectedOwner = computed(() => {
  return props.actionOwners.find((owner) => owner.id === props.selectedActionOwnerId);
});

const isSelected = (owner) => {
  return owner.id === props.selectedActionOwnerId;
};

const toggleDropdown = () => {
  isOpen.value = !isOpen.value;
};

const selectOwner = (owner) => {
  emit("update:selectedActionOwner", owner.id);
  emit("action-owner-changed", owner.id);
  isOpen.value = false;
};

// Close dropdown when clicking outside
const handleClickOutside = (event) => {
  if (containerRef.value && !containerRef.value.contains(event.target)) {
    isOpen.value = false;
  }
};

onMounted(() => {
  document.addEventListener("click", handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener("click", handleClickOutside);
});
</script>
