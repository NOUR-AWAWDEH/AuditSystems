<template>
  <!-- AddUserForm.vue -->
  <div class="bg-white border border-gray-200 shadow-xl rounded-lg p-3 sm:p-4 md:p-6 my-6">
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3 mb-4">
      <h2 class="text-lg font-semibold text-secondary-base">Add New User</h2>

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

        {{ showAddForm ? "Cancel" : "Add User" }}
      </BaseButton>
    </div>

    <!-- Add User Form -->
    <Transition name="slide-down">
      <div v-if="showAddForm" class="mt-4">
        <form @submit.prevent="handleAddUser" class="space-y-6">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <!-- First Name -->
            <div>
              <label
                for="user-first-name"
                class="block text-sm font-semibold text-secondary-base mb-2"
              >
                First Name
              </label>
              <BaseInput
                id="user-first-name"
                v-model="newUser.firstName"
                placeholder="e.g. John"
                required
                class="shadow-sm"
              />
            </div>

            <!-- Last Name -->
            <div>
              <label
                for="user-last-name"
                class="block text-sm font-semibold text-secondary-base mb-2"
              >
                Last Name
              </label>
              <BaseInput
                id="user-last-name"
                v-model="newUser.lastName"
                placeholder="e.g. Doe"
                required
                class="shadow-sm"
              />
            </div>

            <!-- User Name -->
            <div>
              <label for="user-name" class="block text-sm font-semibold text-secondary-base mb-2">
                User Name
              </label>
              <BaseInput
                id="user-name"
                v-model="newUser.userName"
                placeholder="e.g. johndoe"
                required
                class="shadow-sm"
              />
            </div>

            <!-- Email -->
            <div>
              <label for="user-email" class="block text-sm font-semibold text-secondary-base mb-2">
                Email
              </label>
              <BaseInput
                id="user-email"
                v-model="newUser.email"
                placeholder="e.g. johndoe@example.com"
                required
                class="shadow-sm"
              />
            </div>

            <!-- Password -->
            <div>
              <label
                for="user-password"
                class="block text-sm font-semibold text-secondary-base mb-2"
              >
                Password
              </label>
              <PasswordInput id="user-password" v-model="newUser.password" required class="shadow-sm" />
            </div>

            <!-- Confirm Password -->
            <div>
              <label
                for="user-confirm-password"
                class="block text-sm font-semibold text-secondary-base mb-2"
              >
                Confirm Password
              </label>
              <PasswordInput
                id="user-confirm-password"
                v-model="newUser.confirmPassword"
                required
                class="shadow-sm"
              />
            </div>

            <!-- Role -->
            <div>
              <RoleVue v-model="newUser.roleName" />
            </div>

            <!-- Job Title -->
            <div>
              <JobTitleSelector
                v-model="newUser.jobTitleId"
                placeholder="e.g. Select Job Title"
                class="mt-0"
              />
            </div>

            <!-- Sub Department -->
            <div>
              <SubDepartmentSelector
                v-model="newUser.subDepartmentId"
                placeholder="e.g. Select Sub Department"
                class="mt-0"
              />
            </div>
          </div>

          <!-- Actions -->
          <div class="flex flex-col sm:flex-row justify-end gap-3">
            <BaseButton
              type="submit"
              :loading="isCreating"
              :disabled="!newUser.userName.trim()"
              class="w-full sm:w-auto px-6 py-2 bg-secondary-base hover:bg-secondary-shade rounded shadow-sm"
            >
              {{ isCreating ? "Adding..." : "Add User" }}
            </BaseButton>
          </div>
        </form>

        <!-- Error Display -->
        <div v-if="error" class="mt-4 p-3 bg-red-50 border border-red-200 rounded-md">
          <p class="text-red-600 text-sm">{{ error }}</p>
        </div>
      </div>
    </Transition>

    <!-- Actions
    <div class="flex flex-col sm:flex-row justify-end gap-3">
      <BaseButton
        type="submit"
        :loading="isCreating"
        :disabled="!newUser.userName.trim()"
        class="w-full sm:w-auto px-6 py-2 bg-secondary-base hover:bg-secondary-shade rounded"
      >
        {{ isCreating ? "Adding..." : "Add User" }}
      </BaseButton>
    </div> -->
  </div>
</template>

<script setup>
import { useCreateUser } from "@/composables/userManagement/users/useCreateUser";
import { useNotification } from "@/composables/notification/useNotification";
import SubDepartmentSelector from "@/components/features/Organization/subdepartment/components/SubDepartmentSelector.vue";
import JobTitleSelector from "@/components/features/userManagement/jobTitle/components/JobTitleSelector.vue";
import BaseButton from "@/components/base/BaseButton.vue";
import PasswordInput from "@/components/base/PasswordInput.vue";
import BaseInput from "@/components/base/BaseInput.vue";
import RoleVue from "./RoleSelector.vue";
import { ref, onMounted } from "vue";
import authService from "@/services/auth/authRepos/authService";

const emit = defineEmits(["user-added"]);

const newUser = ref({
  firstName: "",
  lastName: "",
  roleName: "",
  userName: "",
  email: "",
  password: "",
  confirmPassword: "",
  jobTitleId: null,
  subDepartmentId: null,
  companyId: "",
});

const { createUser, isCreating, error, clearState } = useCreateUser();
const { showNotification } = useNotification();

const showAddForm = ref(false);

const toggleAddForm = () => {
  showAddForm.value = !showAddForm.value;
  if (!showAddForm.value) {
    resetForm();
  }
};

const handleAddUser = async () => {
  const payload = {
    ...newUser.value,
    jobTitleId: newUser.value.jobTitleId || null,
    subDepartmentId: newUser.value.subDepartmentId || null,
  };

  const result = await createUser(payload);

  if (result.success) {
    showNotification("User created successfully", "success");
    emit("user-added", result.data); // Emit the created user data immediately
    toggleAddForm(); // Will close form and reset it
  } else {
    showNotification(result.message || "Failed to create user", "error");
  }
};

const resetForm = () => {
  newUser.value = {
    userName: "",
    email: "",
    password: "",
    confirmPassword: "",
    firstName: "",
    lastName: "",
    roleName: "",
    companyId: "",
    jobTitleId: null,
    subDepartmentId: null,
  };
  clearState();
  // Re-populate companyId after reset
  const companyId = authService.getLocalCompanyId();
  if (companyId) {
    newUser.value.companyId = companyId;
  }
};

onMounted(() => {
  const companyId = authService.getLocalCompanyId();
  if (companyId) {
    newUser.value.companyId = companyId;
  }
});
</script>
