<template>
  <Teleport to="body">
    <Transition name="modal">
      <div v-if="isVisible" class="fixed inset-0 z-50 flex items-center justify-center p-4">
        <div
          class="absolute inset-0 bg-gray-500 bg-opacity-80 transition-opacity"
          @click="$emit('close')"
        ></div>

        <!-- Modal -->
        <div
          class="relative bg-white rounded-lg shadow-xl max-w-2xl w-full mx-4 transform transition-all max-h-[90vh] overflow-y-auto"
        >
          <!-- Header -->
          <div
            class="flex items-center justify-between p-4 border-b border-gray-tint sticky top-0 bg-white z-10"
          >
            <h3 class="text-base font-medium text-secondary-shade font-poppins">Add New User</h3>
            <button
              @click="$emit('close')"
              class="text-gray-base hover:text-gray-shade focus:outline-none"
            >
              <CloseIcon size="md" custom-class="text-gray-400 hover:text-gray-600" />
            </button>
          </div>

          <!-- Content -->
          <div class="p-4">
            <form @submit.prevent="handleSubmit" class="space-y-4">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <!-- User Name -->
                <div>
                  <label for="userName" class="block text-sm font-medium text-gray-700"
                    >User Name *</label
                  >
                  <BaseInput
                    id="userName"
                    v-model="formData.userName"
                    placeholder="Enter User Name"
                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.userName ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  />
                  <p v-if="errors.userName" class="text-error text-sm mt-1">
                    {{ errors.userName }}
                  </p>
                </div>

                <!-- Email -->
                <div>
                  <label for="email" class="block text-sm font-medium text-gray-700">Email *</label>
                  <BaseInput
                    id="email"
                    type="email"
                    v-model="formData.email"
                    placeholder="Enter Email"
                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.email ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  />
                  <p v-if="errors.email" class="text-error text-sm mt-1">{{ errors.email }}</p>
                </div>

                <!-- First Name -->
                <div>
                  <label for="firstName" class="block text-sm font-medium text-gray-700"
                    >First Name *</label
                  >
                  <BaseInput
                    id="firstName"
                    v-model="formData.firstName"
                    placeholder="Enter First Name"
                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.firstName ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  />
                  <p v-if="errors.firstName" class="text-error text-sm mt-1">
                    {{ errors.firstName }}
                  </p>
                </div>

                <!-- Last Name -->
                <div>
                  <label for="lastName" class="block text-sm font-medium text-gray-700"
                    >Last Name *</label
                  >
                  <BaseInput
                    id="lastName"
                    v-model="formData.lastName"
                    placeholder="Enter Last Name"
                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.lastName ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  />
                  <p v-if="errors.lastName" class="text-error text-sm mt-1">
                    {{ errors.lastName }}
                  </p>
                </div>

                <!-- Password -->
                <div>
                  <label for="password" class="block text-sm font-medium text-gray-700"
                    >Password *</label
                  >
                  <BaseInput
                    id="password"
                    type="password"
                    v-model="formData.password"
                    placeholder="Enter Password"
                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.password ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  />
                  <p v-if="errors.password" class="text-error text-sm mt-1">
                    {{ errors.password }}
                  </p>
                </div>

                <!-- Confirm Password -->
                <div>
                  <label for="confirmPassword" class="block text-sm font-medium text-gray-700"
                    >Confirm Password *</label
                  >
                  <BaseInput
                    id="confirmPassword"
                    type="password"
                    v-model="formData.confirmPassword"
                    placeholder="Confirm Password"
                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.confirmPassword ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  />
                  <p v-if="errors.confirmPassword" class="text-error text-sm mt-1">
                    {{ errors.confirmPassword }}
                  </p>
                </div>

                <!-- Role Name -->
                <div>
                  <label for="roleName" class="block text-sm font-medium text-gray-700"
                    >Role *</label
                  >
                  <select
                    id="roleName"
                    v-model="formData.roleName"
                    :class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.roleName ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  >
                    <option value="" disabled>Select a role</option>
                    <option v-for="role in roles" :key="role" :value="role">{{ role }}</option>
                  </select>
                  <p v-if="errors.roleName" class="text-error text-sm mt-1">
                    {{ errors.roleName }}
                  </p>
                </div>

                <!-- Company ID -->
                <div>
                  <label for="companyId" class="block text-sm font-medium text-gray-700"
                    >Company ID *</label
                  >
                  <BaseInput
                    id="companyId"
                    v-model="formData.companyId"
                    placeholder="Enter Company ID"
                    :input-class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.companyId ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  />
                  <p v-if="errors.companyId" class="text-error text-sm mt-1">
                    {{ errors.companyId }}
                  </p>
                </div>

                <!-- Job Title -->
                <div>
                  <label for="jobTitleId" class="block text-sm font-medium text-gray-700"
                    >Job Title *</label
                  >
                  <select
                    id="jobTitleId"
                    v-model="formData.jobTitleId"
                    :class="`w-full px-4 py-2 text-secondary-shade border rounded-md focus:outline-none focus:ring-2 ${
                      errors.jobTitleId ? 'border-error' : 'border-gray-tint'
                    } focus:ring-primary-base`"
                  >
                    <option value="" disabled>Select a job title</option>
                    <option v-for="jobTitle in jobTitles" :key="jobTitle.id" :value="jobTitle.id">
                      {{ jobTitle.name }}
                    </option>
                  </select>
                  <p v-if="errors.jobTitleId" class="text-error text-sm mt-1">
                    {{ errors.jobTitleId }}
                  </p>
                </div>
              </div>

              <!-- API Error -->
              <div v-if="error" class="p-3 bg-soft-error border border-error rounded-md">
                <p class="text-error text-sm">{{ error }}</p>
              </div>

              <!-- Action Buttons -->
              <div class="flex justify-end space-x-3 pt-4 border-t border-gray-tint mt-6">
                <button
                  type="button"
                  @click="$emit('close')"
                  :disabled="isCreating"
                  class="px-4 py-2 text-sm font-medium text-white bg-error border border-gray-tint rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  Cancel
                </button>

                <button
                  type="submit"
                  :disabled="isCreating"
                  class="px-4 py-2 text-sm font-medium text-white bg-primary-base border border-transparent rounded-md hover:bg-primary-shade focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary-base disabled:opacity-50"
                >
                  <span v-if="isCreating">Creating...</span>
                  <span v-else>Create User</span>
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
import { ref, watch, onMounted } from "vue";
import BaseInput from "@/components/base/BaseInput.vue";
import CloseIcon from "@/components/common/icons/CloseIcon.vue";
import { useCreateUser } from "@/composables/userManagement/userRegistration/useCreateUser";
import { useNotification } from "@/composables/notification/useNotification";
import { useJobTitles } from "@/composables/organization/useJobTitles";

const props = defineProps({
  isVisible: { type: Boolean, default: false },
});

const emit = defineEmits(["close", "user-created"]);

const { showNotification } = useNotification();
const { isCreating, error, createUser } = useCreateUser();
const { jobTitles, fetchJobTitles } = useJobTitles();
const roles = ["ActionOwner"];

const formData = ref({
  userName: "",
  email: "",
  password: "",
  confirmPassword: "",
  firstName: "",
  lastName: "",
  roleName: "",
  companyId: "",
  jobTitleId: "",
});

const errors = ref({});

watch(
  () => props.isVisible,
  (newValue) => {
    if (newValue) {
      resetForm();
    }
  }
);

onMounted(async () => {
  await fetchJobTitles();
});

const resetForm = () => {
  formData.value = {
    userName: "",
    email: "",
    password: "",
    confirmPassword: "",
    firstName: "",
    lastName: "",
    roleName: "",
    companyId: "",
    jobTitleId: "",
  };
  errors.value = {};
};

const validateForm = () => {
  errors.value = {};
  let isValid = true;

  // Username validation: 3-50 characters, alphanumeric with ._- allowed
  if (!formData.value.userName.trim()) {
    errors.value.userName = "Username is required";
    isValid = false;
  } else if (formData.value.userName.length < 3 || formData.value.userName.length > 50) {
    errors.value.userName = "Username must be between 3 and 50 characters";
    isValid = false;
  } else if (!/^[a-zA-Z0-9._-]+$/.test(formData.value.userName)) {
    errors.value.userName = "Username contains invalid characters";
    isValid = false;
  }

  // Email validation: valid email format, max 100 characters
  if (!formData.value.email.trim()) {
    errors.value.email = "Email is required";
    isValid = false;
  } else if (formData.value.email.length > 100) {
    errors.value.email = "Email cannot exceed 100 characters";
    isValid = false;
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.value.email)) {
    errors.value.email = "Please enter a valid email address";
    isValid = false;
  }

  // Password validation: 6-50 characters minimum
  if (!formData.value.password) {
    errors.value.password = "Password is required";
    isValid = false;
  } else if (formData.value.password.length < 6) {
    errors.value.password = "Password must be at least 6 characters";
    isValid = false;
  } else if (formData.value.password.length > 50) {
    errors.value.password = "Password cannot exceed 50 characters";
    isValid = false;
  }

  // Confirm Password validation: must match password
  if (!formData.value.confirmPassword) {
    errors.value.confirmPassword = "Confirm Password is required";
    isValid = false;
  } else if (formData.value.password !== formData.value.confirmPassword) {
    errors.value.confirmPassword = "Passwords do not match";
    isValid = false;
  }

  // First Name validation: 1-50 characters
  if (!formData.value.firstName.trim()) {
    errors.value.firstName = "First Name is required";
    isValid = false;
  } else if (formData.value.firstName.length > 50) {
    errors.value.firstName = "First Name cannot exceed 50 characters";
    isValid = false;
  }

  // Last Name validation: 1-50 characters
  if (!formData.value.lastName.trim()) {
    errors.value.lastName = "Last Name is required";
    isValid = false;
  } else if (formData.value.lastName.length > 50) {
    errors.value.lastName = "Last Name cannot exceed 50 characters";
    isValid = false;
  }

  // Role Name validation: required, max 50 characters
  if (!formData.value.roleName.trim()) {
    errors.value.roleName = "Role is required";
    isValid = false;
  } else if (formData.value.roleName.length > 50) {
    errors.value.roleName = "Role Name cannot exceed 50 characters";
    isValid = false;
  }

  // Company ID validation: required
  if (!formData.value.companyId.trim()) {
    errors.value.companyId = "Company ID is required";
    isValid = false;
  }

  // Job Title ID validation: required
  if (!formData.value.jobTitleId.trim()) {
    errors.value.jobTitleId = "Job Title ID is required";
    isValid = false;
  }

  return isValid;
};

const handleSubmit = async () => {
  if (!validateForm()) return;

  try {
    const result = await createUser(formData.value);

    if (result?.success) {
      showNotification("User created successfully!", "success");
      emit("user-created", result.data);
      emit("close");
    } else {
      showNotification(result?.message || "Failed to create user", "error");
    }
  } catch (err) {
    console.error("Error creating user:", err);
    showNotification("Failed to create user. Please try again.", "error");
  }
};
</script>
