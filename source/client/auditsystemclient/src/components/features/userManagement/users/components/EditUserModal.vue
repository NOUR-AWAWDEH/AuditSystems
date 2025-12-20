<template>
  <!--EditUserModal.vue-->
  <div
    v-if="isVisible"
    class="fixed inset-0 bg-gray-base bg-opacity-50 flex items-center justify-center z-50 p-2 sm:p-4"
  >
    <div
      class="bg-white rounded-lg p-4 sm:p-6 w-full max-w-2xl lg:max-w-3xl mx-auto max-h-[90vh] overflow-y-auto shadow-xl"
    >
      <h2 class="text-xl sm:text-2xl font-semibold mb-4 sm:mb-6 text-secondary-shade">Edit User</h2>

      <form @submit.prevent="handleSubmit">
        <div v-if="updateError" class="mb-4 p-3 bg-red-100 text-red-700 rounded-md text-sm">
          {{ updateError }}
        </div>

        <!-- Form Fields Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4 sm:gap-6 mb-4 sm:mb-6">
          <!-- First Name -->
          <div>
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
          <div>
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
          <div>
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
          <div>
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

          <!-- Role -->
          <div>
            <RoleSelector v-model="formData.role" />
          </div>

          <!-- Job Title -->
          <div>
            <JobTitleSelector
              v-model="formData.jobTitle"
              placeholder="e.g. Select Job Title"
              :emit-object="true"
              class="mt-0"
            />
          </div>

          <!-- Sub Department - Full Width on Medium+ screens -->
          <div class="md:col-span-2">
            <SubDepartmentSelector
              v-model="formData.subDepartment"
              placeholder="e.g. Select Sub Department"
              :emit-object="true"
              class="mt-0"
            />
          </div>
        </div>

        <!-- Action Buttons -->
        <div class="flex flex-col sm:flex-row justify-end gap-3 sm:space-x-3 sm:space-y-0">
          <BaseButton
            @click="$emit('close')"
            :disabled="isUpdating"
            class="w-full sm:w-auto px-4 py-2 text-white border border-error rounded-md hover:bg-red-800 bg-error"
          >
            Cancel
          </BaseButton>
          <BaseButton
            @click="handleSubmit"
            :disabled="isButtonDisabled"
            :is-loading="isUpdating"
            class="w-full sm:w-auto px-4 py-2 bg-primary-base text-white rounded-md hover:bg-primary-shade disabled:opacity-50"
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
import RoleSelector from "@/components/features/userManagement/users/components/RoleSelector.vue";
import JobTitleSelector from "@/components/features/userManagement/jobTitle/components/JobTitleSelector.vue";
import SubDepartmentSelector from "@/components/features/Organization/subdepartment/components/SubDepartmentSelector.vue";
import { useUpdateUser } from "@/composables/userManagement/users/useUpdateUser";

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

const { isUpdating, updateUser, error: updateError } = useUpdateUser();

const formData = ref({
  firstName: "",
  lastName: "",
  userName: "",
  email: "",
  role: "",
  jobTitle: null,
  subDepartment: null,
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
        role: newUser.role || "",
        jobTitle: newUser.jobTitle || null,
        subDepartment: newUser.subDepartment || null,
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

  return isUpdating.value || isFirstNameEmpty || isLastNameEmpty || isUserNameEmpty || isEmailEmpty;
});

const handleSubmit = async () => {
  if (!props.user?.id) return;

  const payload = {
    id: props.user.id,
    firstName: formData.value.firstName.trim(),
    lastName: formData.value.lastName.trim(),
    userName: formData.value.userName.trim(),
    email: formData.value.email.trim(),
    role: formData.value.role,
    jobTitle: formData.value.jobTitle,
    subDepartment: formData.value.subDepartment,
  };

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
