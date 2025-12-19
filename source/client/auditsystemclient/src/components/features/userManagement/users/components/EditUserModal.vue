<template>
  <!--EditUserModal.vue-->
  <div
    v-if="isVisible"
    class="fixed inset-0 bg-gray-base bg-opacity-50 flex items-center justify-center z-50"
  >
    <div
      class="bg-white rounded-lg p-3 sm:p-4 md:p-6 w-full max-w-md mx-2 sm:mx-3 md:mx-4 max-h-[90vh] overflow-y-auto"
    >
      <h2 class="text-xl font-semibold mb-4 text-secondary-shade">Edit User</h2>

      <form @submit.prevent="handleSubmit">
        <div v-if="updateError" class="mb-4 p-3 bg-red-100 text-red-700 rounded-md text-sm">
          {{ updateError }}
        </div>

        <!-- First Name -->
        <div class="mb-4">
          <label for="edit-firstName" class="block text-sm font-medium text-secondary-base mb-2">
            First Name
          </label>
          <BaseInput
            id="edit-firstName"
            v-model="formData.firstName"
            type="text"
            placeholder="Enter first name"
            :input-class="'w-full px-3 py-2 border border-gray-tint rounded-md focus:outline-none focus:ring-2 focus:ring-secondary-base text-secondary-shade'"
          />
        </div>

        <!-- Last Name -->
        <div class="mb-4">
          <label for="edit-lastName" class="block text-sm font-medium text-secondary-base mb-2">
            Last Name
          </label>
          <BaseInput
            id="edit-lastName"
            v-model="formData.lastName"
            placeholder="e.g. Doe"
            :input-class="'w-full px-3 py-2 border border-gray-tint rounded-md focus:outline-none focus:ring-2 focus:ring-secondary-base text-secondary-shade'"
          />
        </div>

        <!-- User Name -->
        <div class="mb-4">
          <label for="edit-userName" class="block text-sm font-medium text-secondary-base mb-2">
            User Name
          </label>
          <BaseInput
            id="edit-userName"
            v-model="formData.userName"
            placeholder="e.g. johndoe"
            :input-class="'w-full px-3 py-2 border border-gray-tint rounded-md focus:outline-none focus:ring-2 focus:ring-secondary-base text-secondary-shade'"
          />
        </div>

        <!-- Email -->
        <div class="mb-4">
          <label for="edit-email" class="block text-sm font-medium text-secondary-base mb-2">
            Email
          </label>
          <BaseInput
            id="edit-email"
            v-model="formData.email"
            placeholder="e.g. johndoe@example.com"
            :input-class="'w-full px-3 py-2 border border-gray-tint rounded-md focus:outline-none focus:ring-2 focus:ring-secondary-base text-secondary-shade'"
          />
        </div>

        <!-- Password -->
        <div class="mb-4">
          <label for="edit-password" class="block text-sm font-medium text-secondary-base mb-2">
            Password (leave empty to keep current)
          </label>
          <PasswordInput id="edit-password" v-model="formData.password" class="shadow-sm" />
        </div>

        <!-- Confirm Password -->
        <div class="mb-4">
          <label
            for="edit-confirmPassword"
            class="block text-sm font-medium text-secondary-base mb-2"
          >
            Confirm Password
          </label>
          <PasswordInput id="edit-confirmPassword" v-model="formData.confirmPassword" />
        </div>

        <!-- Role -->
        <div class="mb-4">
          <RoleVue v-model="formData.roleName" />
        </div>

        <!-- Job Title -->
        <div class="mb-4">
          <JobTitleSelector
            v-model="formData.jobTitleId"
            placeholder="e.g. Select Job Title"
            class="mt-0"
          />
        </div>

        <!-- Sub Department -->
        <div class="mb-4">
          <SubDepartmentSelector
            v-model="formData.subDepartmentId"
            placeholder="e.g. Select Sub Department"
            class="mt-0"
          />
        </div>

        <div class="flex justify-end space-x-3">
          <BaseButton
            @click="$emit('close')"
            :disabled="isUpdating"
            class="px-4 py-2 text-white border border-error rounded-md hover:bg-red-800 bg-error"
          >
            Cancel
          </BaseButton>
          <BaseButton
            @click="handleSubmit"
            :disabled="isButtonDisabled"
            :is-loading="isUpdating"
            class="px-4 py-2 bg-primary-base text-white rounded-md hover:bg-primary-shade disabled:opacity-50"
          >
            <template #loading>Updating...</template>
            Update User
          </BaseButton>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, computed } from "vue";
import BaseInput from "@/components/base/BaseInput.vue";
import BaseButton from "@/components/base/BaseButton.vue";
import PasswordInput from "@/components/base/PasswordInput.vue";
import RoleVue from "@/components/features/userManagement/roles/Role.vue";
import JobTitleSelector from "@/components/features/userManagement/jobTitle/JobTitleSelector.vue";
import SubDepartmentSelector from "@/components/features/userManagement/subDepartments/SubDepartmentSelector.vue";
import { useUpdateUserById } from "@/composables/organization/user/useUpdateUserById";

const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false,
  },
  user: {
    type: Object,
    default: null,
  },
});

const emit = defineEmits(["close", "user-updated"]);

const { isUpdating, updateUser, error: updateError } = useUpdateUserById();

const formData = ref({
  firstName: "",
  lastName: "",
  userName: "",
  email: "",
  password: "",
  confirmPassword: "",
  roleName: "",
  jobTitleId: "",
  subDepartmentId: "",
});

watch(
  () => props.user,
  (newUser) => {
    if (newUser) {
      formData.value = {
        firstName: newUser.firstName || "",
        lastName: newUser.lastName || "",
        userName: newUser.userName || "",
        email: newUser.email || "",
        password: "",
        confirmPassword: "",
        roleName: newUser.roleName || "",
        jobTitleId: newUser.jobTitleId || "",
        subDepartmentId: newUser.subDepartmentId || "",
      };
    }
  },
  { immediate: true }
);

const isButtonDisabled = computed(() => {
  const isFirstNameEmpty = !formData.value.firstName.trim();
  const isLastNameEmpty = !formData.value.lastName.trim();
  const isUserNameEmpty = !formData.value.userName.trim();
  const isEmailEmpty = !formData.value.email.trim();
  const passwordMismatch = formData.value.password !== formData.value.confirmPassword;

  return (
    isUpdating.value ||
    isFirstNameEmpty ||
    isLastNameEmpty ||
    isUserNameEmpty ||
    isEmailEmpty ||
    passwordMismatch
  );
});

const handleSubmit = async () => {
  if (!props.user?.id) return;

  const payload = {
    id: props.user.id,
    firstName: formData.value.firstName.trim(),
    lastName: formData.value.lastName.trim(),
    userName: formData.value.userName.trim(),
    email: formData.value.email.trim(),
    roleName: formData.value.roleName,
    jobTitleId: formData.value.jobTitleId,
    subDepartmentId: formData.value.subDepartmentId,
  };

  // Only include password if it was provided
  if (formData.value.password) {
    payload.password = formData.value.password;
  }

  const result = await updateUser(payload);

  if (result.success) {
    emit("user-updated", {
      ...props.user,
      ...payload,
    });

    emit("close");
  }
};
</script>
