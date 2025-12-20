<template>
  <!--UsersTable.vue-->
  <div>
    <!-- Users Container -->
    <div class="bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
      <div class="p-3 sm:p-4 md:p-6">
        <!-- Header with title and Filters -->
        <div class="flex flex-col gap-4 mb-6">
          <UserHeader />

          <div class="flex flex-col sm:flex-row sm:items-end gap-3">
            <SearchFilter
              :search="searchQuery"
              :searchPlaceholder="'Search users...'"
              @update:search="handleSearch"
              class="w-full sm:w-auto md:w-1/2"
            />
          </div>
        </div>

        <div class="hidden lg:block rounded-lg border border-gray-200">
          <ReusableTable
            :items="users"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="users-table min-w-full"
          >
            <!-- Custom cell for SubDepartment -->
            <template #cell-subDepartment="{ item }">
              <span v-if="item.subDepartment">{{ item.subDepartment.name }}</span>
              <span v-else class="text-gray-400 italic">Not assigned</span>
            </template>

            <!-- Custom cell for JobTitle -->
            <template #cell-jobTitle="{ item }">
              <span v-if="item.jobTitle">{{ item.jobTitle.name }}</span>
              <span v-else class="text-gray-400 italic">Not assigned</span>
            </template>

            <template #cell-actions="{ item }">
              <div class="flex flex-col items-start justify-start xl:flex-row gap-2 min-w-max">
                <button
                  @click.stop="$emit('edit-user', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <EditIcon size="sm" custom-class="mr-1" />
                  Edit
                </button>

                <button
                  @click.stop="$emit('delete-user', item)"
                  class="inline-flex items-center min-w-22 px-2 py-1 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                >
                  <DeleteIcon size="sm" custom-class="mr-1" />
                  <span>Delete</span>
                </button>
              </div>
            </template>
          </ReusableTable>
        </div>

        <!-- Mobile Card View -->
        <ReusableMobileCardList
          :items="users"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="lg:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <!-- User Name -->
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  User Name
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.userName }}
                </div>
              </div>

              <!-- Full Name -->
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Full Name
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.firstName }} {{ item.lastName }}
                </div>
              </div>

              <!-- Email -->
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Email
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.email }}
                </div>
              </div>

              <!-- Role -->
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Role
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.role }}
                </div>
              </div>

              <!-- Job Title -->
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Job Title
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  <span v-if="item.jobTitle">{{ item.jobTitle.name }}</span>
                  <span v-else class="text-gray-400 italic">Not assigned</span>
                </div>
              </div>

              <!-- Sub Department -->
              <div>
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  Sub Department
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  <span v-if="item.subDepartment">{{ item.subDepartment.name }}</span>
                  <span v-else class="text-gray-400 italic">Not assigned</span>
                </div>
              </div>
            </div>
          </template>

          <template #card-actions="{ item }">
            <div class="flex w-full justify-start gap-2 mt-2 flex-wrap">
              <button
                @click.stop="$emit('edit-user', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 transition-all duration-200"
              >
                <EditIcon size="sm" class="mr-1" /> Edit
              </button>

              <button
                @click.stop="$emit('delete-user', item)"
                class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 transition-all duration-200"
              >
                <DeleteIcon size="sm" class="mr-1" />
                Delete
              </button>
            </div>
          </template>
        </ReusableMobileCardList>
      </div>

      <!-- Loading -->
      <div v-if="isLoading" class="text-center py-8">
        <div
          class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-primary-base"
        ></div>
        <p class="mt-2 text-gray-600 text-sm">Loading users...</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from "vue";
import UserHeader from "./UserHeader.vue";
import ReusableTable from "@/components/common/ReusableTable.vue";
import ReusableMobileCardList from "@/components/common/ReusableMobileCardList.vue";
import SearchFilter from "@/components/common/SearchFilter.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";

const props = defineProps({
  users: {
    type: Array,
    default: () => [],
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  searchQuery: {
    type: String,
    default: "",
  },
  handleSearch: {
    type: Function,
    default: () => {},
  },
});

defineEmits(["add-user", "edit-user", "delete-user"]);

// Columns based on UserModel structure from backend:
// Id, UserName, FirstName, LastName, Email, Role, SubDepartment, JobTitle
const userColumns = [
  { key: "userName", label: "User Name" },
  { key: "firstName", label: "First Name" },
  { key: "lastName", label: "Last Name" },
  { key: "email", label: "Email" },
  { key: "role", label: "Role" },
  { key: "subDepartment", label: "Sub Department" },
  { key: "jobTitle", label: "Job Title" },
];

const enhancedColumns = computed(() => {
  return [...userColumns, { key: "actions", label: "Actions" }];
});

const noDataMessage = computed(() =>
  props.searchQuery
    ? `No users found for "${props.searchQuery}".`
    : "No users found yet. Please add a user."
);
</script>

<style scoped>
.users-table .table-row {
  transition: all 0.2s ease;
  cursor: pointer;
}

.users-table .table-row:hover {
  background-color: #f8fafc;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.users-table .table-row.selected {
  background-color: #eff6ff;
  border-left: 4px solid #3b82f6;
}

@media (max-width: 767px) {
  .users-table .table-row:hover {
    transform: none;
    box-shadow: none;
  }
}
</style>
