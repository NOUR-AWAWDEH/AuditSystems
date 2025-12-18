<template>
  <div
    class="min-h-[calc(100vh)] bg-gray-50 p-2 sm:p-3 md:p-6 lg:p-8 xl:p-10 transition-all duration-300"
  >
    <!-- Header Section -->
    <header class="mb-3 sm:mb-5 md:mb-8">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-2 sm:gap-4">
        <UserManagementHeader />
      </div>
    </header>

    <!-- Navigation Tabs / Dropdown -->
    <div class="mb-4 sm:mb-6 border-b border-gray-200">
      <!-- Dropdown for small screens -->
      <div class="md:hidden mb-2">
        <select
          v-model="activeTab"
          @change="setActiveTab(activeTab)"
          class="w-full p-2 border border-gray-300 rounded-lg bg-white text-sm focus:outline-none focus:ring-2 focus:ring-primary-base"
        >
          <option v-for="tab in tabs" :key="tab.id" :value="tab.id">
            {{ tab.name }}
          </option>
        </select>
      </div>

      <!-- Tabs for medium+ screens -->
      <nav
        class="hidden md:flex space-x-2 sm:space-x-4 md:space-x-6 lg:space-x-8 overflow-x-auto scrollbar-hide"
      >
        <button
          v-for="tab in tabs"
          :key="tab.id"
          @click="setActiveTab(tab.id)"
          :class="[
            'whitespace-nowrap border-b-2 font-medium shrink-0 px-3 md:px-4 lg:px-6 py-2 text-sm md:text-base transition-colors duration-200',
            activeTab === tab.id
              ? 'border-primary-base text-primary-base'
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300',
          ]"
        >
          {{ tab.name }}
        </button>
      </nav>
    </div>

    <!-- Tab Content -->
    <section
      class="bg-white rounded-lg sm:rounded-xl md:rounded-2xl shadow-sm border border-gray-200"
    >
      <div
        v-if="isLoading"
        class="p-3 sm:p-5 md:p-8 text-center text-sm sm:text-base text-gray-500"
      >
        Loading...
      </div>

      <div
        v-else-if="errorMessage"
        class="p-3 sm:p-5 md:p-8 text-center text-sm sm:text-base text-red-500"
      >
        {{ errorMessage }}
      </div>

      <div v-else>
        <div v-if="activeTab === 'users'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <UserView />
        </div>
        <div v-if="activeTab === 'jobTitles'" class="p-3 sm:p-4 md:p-6 lg:p-8 overflow-x-auto">
          <JobTitleView />
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref } from "vue";
import JobTitleView from "./jobTitle/JobTitleView.vue";
import UserView from "./users/UserVue.vue";
import UserManagementHeader from "./components/UserManagementHeader.vue";

const activeTab = ref("users");
const isLoading = ref(false);
const errorMessage = ref(null);
const tabs = [
  { id: "users", name: "Users" },
  { id: "jobTitles", name: "Job Titles" },
];

const setActiveTab = (tabId) => {
  activeTab.value = tabId;
};
</script>
