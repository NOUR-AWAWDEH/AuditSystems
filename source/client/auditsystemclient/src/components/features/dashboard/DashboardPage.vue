<script setup>
import { ref, computed } from 'vue';

const currentDate = computed(() => {
  return new Date().toLocaleDateString('en-US', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
});

const currentTime = ref(new Date().toLocaleTimeString());

// Update time every second
setInterval(() => {
  currentTime.value = new Date().toLocaleTimeString();
}, 1000);

// Sample dashboard data
const dashboardStats = ref([
  {
    title: 'Total Audits',
    value: '24',
    change: '+12%',
    changeType: 'positive',
    icon: 'M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z'
  },
  {
    title: 'Pending Reviews',
    value: '8',
    change: '-3%',
    changeType: 'negative',
    icon: 'M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z'
  },
  {
    title: 'Completed',
    value: '16',
    change: '+8%',
    changeType: 'positive',
    icon: 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z'
  },
  {
    title: 'Documents',
    value: '142',
    change: '+25%',
    changeType: 'positive',
    icon: 'M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z'
  }
]);

const recentActivities = ref([
  {
    id: 1,
    action: 'New audit created',
    description: 'Financial Audit Q4 2024',
    time: '2 hours ago',
    type: 'create'
  },
  {
    id: 2,
    action: 'Document uploaded',
    description: 'Balance Sheet.pdf',
    time: '4 hours ago',
    type: 'upload'
  },
  {
    id: 3,
    action: 'Audit completed',
    description: 'Compliance Review',
    time: '1 day ago',
    type: 'complete'
  },
  {
    id: 4,
    action: 'Review assigned',
    description: 'Internal Controls Assessment',
    time: '2 days ago',
    type: 'assign'
  }
]);

const quickActions = ref([
  {
    title: 'New Audit',
    description: 'Start a new audit process',
    icon: 'M12 4v16m8-8H4',
    color: 'bg-primary-base',
    route: '/audits/new'
  },
  {
    title: 'Upload Documents',
    description: 'Add supporting documents',
    icon: 'M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M9 19l3 3m0 0l3-3m-3 3V10',
    color: 'bg-secondary-base',
    route: '/supporting-docs'
  },
  {
    title: 'View Reports',
    description: 'Access audit reports',
    icon: 'M9 17v-2m3 2v-4m3 4v-6m2 10H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z',
    color: 'bg-tertiary-base',
    route: '/reports'
  }
]);
</script>

<template>
  <div class="min-h-[calc(100vh-4rem)] bg-gray-50 p-4 md:p-6">
    <!-- Header Section -->
    <div class="mb-8">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between">
        <div>
          <h1 class="text-3xl md:text-4xl font-bold text-secondary-base mb-2">
            Welcome to Audit System
          </h1>
          <p class="text-gray-600 text-lg">
            {{ currentDate }} • {{ currentTime }}
          </p>
        </div>
        <div class="mt-4 md:mt-0">
          <button class="bg-primary-base text-white px-6 py-3 rounded-lg hover:bg-primary-shade transition-colors duration-200 font-medium">
            Generate Report
          </button>
        </div>
      </div>
    </div>

    <!-- Statistics Cards -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
      <div 
        v-for="stat in dashboardStats" 
        :key="stat.title"
        class="bg-white rounded-xl shadow-sm border border-gray-200 p-6 hover:shadow-md transition-shadow duration-200"
      >
        <div class="flex items-center justify-between">
          <div>
            <p class="text-gray-600 text-sm font-medium mb-1">{{ stat.title }}</p>
            <p class="text-3xl font-bold text-secondary-base">{{ stat.value }}</p>
            <div class="flex items-center mt-2">
              <span 
                :class="[
                  'text-sm font-medium',
                  stat.changeType === 'positive' ? 'text-green-600' : 'text-red-600'
                ]"
              >
                {{ stat.change }}
              </span>
              <span class="text-gray-500 text-sm ml-1">vs last month</span>
            </div>
          </div>
          <div class="bg-primary-tint p-3 rounded-lg">
            <svg class="w-6 h-6 text-primary-base" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" :d="stat.icon"></path>
            </svg>
          </div>
        </div>
      </div>
    </div>

    <!-- Main Content Grid -->
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <!-- Quick Actions -->
      <div class="lg:col-span-1">
        <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
          <h3 class="text-xl font-semibold text-secondary-base mb-4">Quick Actions</h3>
          <div class="space-y-4">
            <div 
              v-for="action in quickActions" 
              :key="action.title"
              class="flex items-center p-4 rounded-lg border border-gray-200 hover:border-primary-base hover:shadow-sm transition-all duration-200 cursor-pointer group"
            >
              <div :class="[action.color, 'p-3 rounded-lg mr-4 group-hover:scale-105 transition-transform duration-200']">
                <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" :d="action.icon"></path>
                </svg>
              </div>
              <div class="flex-1">
                <h4 class="font-medium text-secondary-base group-hover:text-primary-base transition-colors duration-200">{{ action.title }}</h4>
                <p class="text-sm text-gray-600">{{ action.description }}</p>
              </div>
              <svg class="w-5 h-5 text-gray-400 group-hover:text-primary-base transition-colors duration-200" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7"></path>
              </svg>
            </div>
          </div>
        </div>
      </div>

      <!-- Recent Activities -->
      <div class="lg:col-span-2">
        <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
          <div class="flex items-center justify-between mb-4">
            <h3 class="text-xl font-semibold text-secondary-base">Recent Activities</h3>
            <button class="text-primary-base hover:text-primary-shade text-sm font-medium">
              View All
            </button>
          </div>
          <div class="space-y-4">
            <div 
              v-for="activity in recentActivities" 
              :key="activity.id"
              class="flex items-start p-4 rounded-lg hover:bg-gray-50 transition-colors duration-200"
            >
              <div class="flex-shrink-0 mr-4">
                <div 
                  :class="[
                    'w-10 h-10 rounded-full flex items-center justify-center',
                    activity.type === 'create' ? 'bg-green-100' :
                    activity.type === 'upload' ? 'bg-blue-100' :
                    activity.type === 'complete' ? 'bg-purple-100' :
                    'bg-orange-100'
                  ]"
                >
                  <svg 
                    class="w-5 h-5" 
                    :class="[
                      activity.type === 'create' ? 'text-green-600' :
                      activity.type === 'upload' ? 'text-blue-600' :
                      activity.type === 'complete' ? 'text-purple-600' :
                      'text-orange-600'
                    ]"
                    fill="none" 
                    stroke="currentColor" 
                    viewBox="0 0 24 24"
                  >
                    <path 
                      stroke-linecap="round" 
                      stroke-linejoin="round" 
                      stroke-width="2" 
                      :d="activity.type === 'create' ? 'M12 4v16m8-8H4' :
                          activity.type === 'upload' ? 'M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M9 19l3 3m0 0l3-3m-3 3V10' :
                          activity.type === 'complete' ? 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z' :
                          'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z'"
                    ></path>
                  </svg>
                </div>
              </div>
              <div class="flex-1">
                <div class="flex items-center justify-between">
                  <h4 class="font-medium text-secondary-base">{{ activity.action }}</h4>
                  <span class="text-sm text-gray-500">{{ activity.time }}</span>
                </div>
                <p class="text-sm text-gray-600 mt-1">{{ activity.description }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Progress Section -->
    <div class="mt-8">
      <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
        <h3 class="text-xl font-semibold text-secondary-base mb-4">Monthly Progress</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div class="text-center">
            <div class="relative w-24 h-24 mx-auto mb-4">
              <svg class="w-24 h-24 transform -rotate-90" viewBox="0 0 100 100">
                <circle cx="50" cy="50" r="40" stroke="#e5e7eb" stroke-width="8" fill="none"></circle>
                <circle cx="50" cy="50" r="40" stroke="#8b5cf6" stroke-width="8" fill="none" 
                        stroke-dasharray="251.2" stroke-dashoffset="75.36" stroke-linecap="round"></circle>
              </svg>
              <div class="absolute inset-0 flex items-center justify-center">
                <span class="text-2xl font-bold text-secondary-base">70%</span>
              </div>
            </div>
            <p class="font-medium text-secondary-base">Audits Completed</p>
            <p class="text-sm text-gray-600">16 of 23 audits</p>
          </div>
          <div class="text-center">
            <div class="relative w-24 h-24 mx-auto mb-4">
              <svg class="w-24 h-24 transform -rotate-90" viewBox="0 0 100 100">
                <circle cx="50" cy="50" r="40" stroke="#e5e7eb" stroke-width="8" fill="none"></circle>
                <circle cx="50" cy="50" r="40" stroke="#10b981" stroke-width="8" fill="none" 
                        stroke-dasharray="251.2" stroke-dashoffset="50.24" stroke-linecap="round"></circle>
              </svg>
              <div class="absolute inset-0 flex items-center justify-center">
                <span class="text-2xl font-bold text-secondary-base">80%</span>
              </div>
            </div>
            <p class="font-medium text-secondary-base">Documents Reviewed</p>
            <p class="text-sm text-gray-600">114 of 142 docs</p>
          </div>
          <div class="text-center">
            <div class="relative w-24 h-24 mx-auto mb-4">
              <svg class="w-24 h-24 transform -rotate-90" viewBox="0 0 100 100">
                <circle cx="50" cy="50" r="40" stroke="#e5e7eb" stroke-width="8" fill="none"></circle>
                <circle cx="50" cy="50" r="40" stroke="#f59e0b" stroke-width="8" fill="none" 
                        stroke-dasharray="251.2" stroke-dashoffset="100.48" stroke-linecap="round"></circle>
              </svg>
              <div class="absolute inset-0 flex items-center justify-center">
                <span class="text-2xl font-bold text-secondary-base">60%</span>
              </div>
            </div>
            <p class="font-medium text-secondary-base">Compliance Rate</p>
            <p class="text-sm text-gray-600">Above target</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Custom animations */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-fade-in {
  animation: fadeIn 0.6s ease-out;
}
</style>
