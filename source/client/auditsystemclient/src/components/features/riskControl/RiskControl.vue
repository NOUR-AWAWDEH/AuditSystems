<template>
  <div class="min-h-[calc(100vh-4rem)] bg-gray-50 p-4 md:p-6">
    <!-- Header Section -->
    <div class="mb-8">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between">
        <div>
          <h1 class="text-3xl md:text-4xl font-bold text-secondary-base mb-2">
            Risk & Control Management
          </h1>
          <p class="text-gray-600 text-lg">
            Manage processes, risks, controls, and observations
          </p>
          <!-- Selected SubDepartment Info -->
          <div v-if="selectedSubDepartment" class="mt-4 p-3 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg border border-blue-200">
            <div class="flex items-center">
              <LocationIcon size="md" custom-class="text-blue-600 mr-2" />
              <p class="text-sm text-gray-700">
                The Selected SubDepartment: &nbsp;&nbsp;
                <span class="font-semibold text-blue-800">
                  <span v-if="isLoadingSubDepartment">Loading...</span>
                  <span v-else>{{ selectedSubDepartment?.name || 'Unknown SubDepartment' }}</span>
                </span>
              </p>
            </div>
          </div>
        </div>
        <div class="mt-4 md:mt-0 flex gap-3">
          <button class="bg-primary-base text-white px-6 py-3 rounded-lg hover:bg-primary-shade transition-colors duration-200 font-medium">
            Export Report
          </button>
          <button class="bg-secondary-base text-white px-6 py-3 rounded-lg hover:bg-secondary-shade transition-colors duration-200 font-medium">
            Add New Process
          </button>
        </div>
      </div>
    </div>

    <!-- Navigation Tabs -->
    <div class="mb-6">
      <div class="border-b border-gray-200">
        <nav class="-mb-px flex space-x-8 overflow-x-auto">
          <button
            v-for="tab in tabs"
            :key="tab.id"
            @click="activeTab = tab.id"
            :class="[
              'whitespace-nowrap py-2 px-1 border-b-2 font-medium text-sm transition-colors duration-200',
              activeTab === tab.id
                ? 'border-primary-base text-primary-base'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            ]"
          >
            {{ tab.name }}
          </button>
        </nav>
      </div>
    </div>

    <!-- Tab Content -->
    <div class="bg-white rounded-xl shadow-sm border border-gray-200">
      <!-- Process Management -->
      <div v-if="activeTab === 'processes'" class="p-6">
        <div class="flex items-center justify-between mb-6">
          <h2 class="text-xl font-semibold text-secondary-base">Process Management</h2>
          <button class="bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200">
            Add Process
          </button>
        </div>
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Process Name</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Description</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="process in processes" :key="process.id">
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ process.name }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{{ process.description }}</td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span :class="[
                    'inline-flex px-2 py-1 text-xs font-semibold rounded-full',
                    process.status === 'Active' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                  ]">
                    {{ process.status }}
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <button class="text-primary-base hover:text-primary-shade mr-3">Edit</button>
                  <button class="text-red-600 hover:text-red-900">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Sub Process Management -->
      <div v-if="activeTab === 'subprocesses'" class="p-6">
        <div class="flex items-center justify-between mb-6">
          <h2 class="text-xl font-semibold text-secondary-base">Sub Process Management</h2>
          <button class="bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200">
            Add Sub Process
          </button>
        </div>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div v-for="subprocess in subProcesses" :key="subprocess.id" class="border border-gray-200 rounded-lg p-4 hover:shadow-md transition-shadow duration-200">
            <h3 class="font-semibold text-secondary-base mb-2">{{ subprocess.name }}</h3>
            <p class="text-gray-600 text-sm mb-3">{{ subprocess.description }}</p>
            <div class="flex items-center justify-between">
              <span class="text-xs text-gray-500">Parent: {{ subprocess.parentProcess }}</span>
              <div class="flex gap-2">
                <button class="text-primary-base hover:text-primary-shade text-sm">Edit</button>
                <button class="text-red-600 hover:text-red-900 text-sm">Delete</button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Risk Control Matrix -->
      <div v-if="activeTab === 'matrix'" class="p-6">
        <div class="flex items-center justify-between mb-6">
          <h2 class="text-xl font-semibold text-secondary-base">Risk Control Matrix</h2>
          <button class="bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200">
            Add Matrix Entry
          </button>
        </div>
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Risk ID</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Risk Description</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Control</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Risk Level</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="matrix in riskControlMatrix" :key="matrix.id">
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{{ matrix.riskId }}</td>
                <td class="px-6 py-4 text-sm text-gray-500">{{ matrix.riskDescription }}</td>
                <td class="px-6 py-4 text-sm text-gray-500">{{ matrix.control }}</td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span :class="[
                    'inline-flex px-2 py-1 text-xs font-semibold rounded-full',
                    matrix.riskLevel === 'High' ? 'bg-red-100 text-red-800' :
                    matrix.riskLevel === 'Medium' ? 'bg-yellow-100 text-yellow-800' :
                    'bg-green-100 text-green-800'
                  ]">
                    {{ matrix.riskLevel }}
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <button class="text-primary-base hover:text-primary-shade mr-3">Edit</button>
                  <button class="text-red-600 hover:text-red-900">Delete</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Objectives -->
      <div v-if="activeTab === 'objectives'" class="p-6">
        <div class="flex items-center justify-between mb-6">
          <h2 class="text-xl font-semibold text-secondary-base">Objectives Management</h2>
          <button class="bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200">
            Add Objective
          </button>
        </div>
        <div class="space-y-4">
          <div v-for="objective in objectives" :key="objective.id" class="border border-gray-200 rounded-lg p-4">
            <div class="flex items-start justify-between">
              <div class="flex-1">
                <h3 class="font-semibold text-secondary-base mb-2">{{ objective.title }}</h3>
                <p class="text-gray-600 mb-3">{{ objective.description }}</p>
                <div class="flex items-center gap-4 text-sm text-gray-500">
                  <span>Target: {{ objective.target }}</span>
                  <span>Progress: {{ objective.progress }}%</span>
                  <span>Due: {{ objective.dueDate }}</span>
                </div>
              </div>
              <div class="flex gap-2 ml-4">
                <button class="text-primary-base hover:text-primary-shade">Edit</button>
                <button class="text-red-600 hover:text-red-900">Delete</button>
              </div>
            </div>
            <div class="mt-3">
              <div class="w-full bg-gray-200 rounded-full h-2">
                <div class="bg-primary-base h-2 rounded-full" :style="{ width: objective.progress + '%' }"></div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Risks -->
      <div v-if="activeTab === 'risks'" class="p-6">
        <div class="flex items-center justify-between mb-6">
          <h2 class="text-xl font-semibold text-secondary-base">Risk Management</h2>
          <button class="bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200">
            Add Risk
          </button>
        </div>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div v-for="risk in risks" :key="risk.id" class="border border-gray-200 rounded-lg p-4">
            <div class="flex items-start justify-between mb-3">
              <h3 class="font-semibold text-secondary-base">{{ risk.title }}</h3>
              <span :class="[
                'inline-flex px-2 py-1 text-xs font-semibold rounded-full',
                risk.severity === 'High' ? 'bg-red-100 text-red-800' :
                risk.severity === 'Medium' ? 'bg-yellow-100 text-yellow-800' :
                'bg-green-100 text-green-800'
              ]">
                {{ risk.severity }}
              </span>
            </div>
            <p class="text-gray-600 text-sm mb-3">{{ risk.description }}</p>
            <div class="flex items-center justify-between text-sm">
              <span class="text-gray-500">Probability: {{ risk.probability }}%</span>
              <div class="flex gap-2">
                <button class="text-primary-base hover:text-primary-shade">Edit</button>
                <button class="text-red-600 hover:text-red-900">Delete</button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Exit Risks -->
      <div v-if="activeTab === 'exitrisks'" class="p-6">
        <div class="flex items-center justify-between mb-6">
          <h2 class="text-xl font-semibold text-secondary-base">Exit Risk Management</h2>
          <button class="bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200">
            Add Exit Risk
          </button>
        </div>
        <div class="space-y-4">
          <div v-for="exitRisk in exitRisks" :key="exitRisk.id" class="border border-gray-200 rounded-lg p-4">
            <div class="flex items-start justify-between">
              <div class="flex-1">
                <h3 class="font-semibold text-secondary-base mb-2">{{ exitRisk.title }}</h3>
                <p class="text-gray-600 mb-3">{{ exitRisk.description }}</p>
                <div class="flex items-center gap-4 text-sm text-gray-500">
                  <span>Impact: {{ exitRisk.impact }}</span>
                  <span>Mitigation: {{ exitRisk.mitigation }}</span>
                </div>
              </div>
              <div class="flex gap-2 ml-4">
                <button class="text-primary-base hover:text-primary-shade">Edit</button>
                <button class="text-red-600 hover:text-red-900">Delete</button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Control Design -->
      <div v-if="activeTab === 'controls'" class="p-6">
        <div class="flex items-center justify-between mb-6">
          <h2 class="text-xl font-semibold text-secondary-base">Control Design</h2>
          <button class="bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200">
            Add Control
          </button>
        </div>
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <div v-for="control in controlDesigns" :key="control.id" class="border border-gray-200 rounded-lg p-4">
            <div class="flex items-start justify-between mb-3">
              <h3 class="font-semibold text-secondary-base">{{ control.name }}</h3>
              <span :class="[
                'inline-flex px-2 py-1 text-xs font-semibold rounded-full',
                control.effectiveness === 'Effective' ? 'bg-green-100 text-green-800' :
                control.effectiveness === 'Partially Effective' ? 'bg-yellow-100 text-yellow-800' :
                'bg-red-100 text-red-800'
              ]">
                {{ control.effectiveness }}
              </span>
            </div>
            <p class="text-gray-600 text-sm mb-3">{{ control.description }}</p>
            <div class="text-sm text-gray-500 mb-3">
              <div>Type: {{ control.type }}</div>
              <div>Frequency: {{ control.frequency }}</div>
            </div>
            <div class="flex items-center justify-between">
              <span class="text-xs text-gray-500">Last Review: {{ control.lastReview }}</span>
              <div class="flex gap-2">
                <button class="text-primary-base hover:text-primary-shade text-sm">Edit</button>
                <button class="text-red-600 hover:text-red-900 text-sm">Delete</button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Observations -->
      <div v-if="activeTab === 'observations'" class="p-6">
        <div class="flex items-center justify-between mb-6">
          <h2 class="text-xl font-semibold text-secondary-base">Observations</h2>
          <button class="bg-primary-base text-white px-4 py-2 rounded-lg hover:bg-primary-shade transition-colors duration-200">
            Add Observation
          </button>
        </div>
        <div class="space-y-4">
          <div v-for="observation in observations" :key="observation.id" class="border border-gray-200 rounded-lg p-4">
            <div class="flex items-start justify-between mb-3">
              <div class="flex-1">
                <h3 class="font-semibold text-secondary-base mb-2">{{ observation.title }}</h3>
                <p class="text-gray-600 mb-3">{{ observation.description }}</p>
                <div class="flex items-center gap-4 text-sm text-gray-500">
                  <span>Category: {{ observation.category }}</span>
                  <span>Priority: {{ observation.priority }}</span>
                  <span>Date: {{ observation.date }}</span>
                </div>
              </div>
              <div class="flex gap-2 ml-4">
                <button class="text-primary-base hover:text-primary-shade">Edit</button>
                <button class="text-red-600 hover:text-red-900">Delete</button>
              </div>
            </div>
            <div v-if="observation.recommendations" class="mt-3 p-3 bg-gray-50 rounded">
              <h4 class="text-sm font-medium text-gray-700 mb-1">Recommendations:</h4>
              <p class="text-sm text-gray-600">{{ observation.recommendations }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useGetSubDepartmentById } from '@/composables/organization/subdepartment/useGetSubDepartmentById';
import LocationIcon from '@/components/common/icons/LocationIcon.vue';

const route = useRoute();

// SubDepartment management
const { 
  subdepartment: selectedSubDepartment, 
  isLoading: isLoadingSubDepartment, 
  fetchSubDepartmentById 
} = useGetSubDepartmentById();

// Active tab state
const activeTab = ref('processes');

// Watch for route query changes to load subdepartment
watch(
  () => route.query.subDepartmentId,
  async (newSubDepartmentId) => {
    if (newSubDepartmentId) {
      await fetchSubDepartmentById(newSubDepartmentId);
    }
  },
  { immediate: true }
);

// Load subdepartment on component mount if ID is in query
onMounted(async () => {
  const subDepartmentId = route.query.subDepartmentId;
  if (subDepartmentId) {
    await fetchSubDepartmentById(subDepartmentId);
  }
});

// Tab configuration
const tabs = ref([
  { id: 'processes', name: 'Processes' },
  { id: 'subprocesses', name: 'Sub Processes' },
  { id: 'matrix', name: 'Risk Control Matrix' },
  { id: 'objectives', name: 'Objectives' },
  { id: 'risks', name: 'Risks' },
  { id: 'exitrisks', name: 'Exit Risks' },
  { id: 'controls', name: 'Control Design' },
  { id: 'observations', name: 'Observations' }
]);

// Sample data - replace with actual API calls
const processes = ref([
  { id: 1, name: 'Financial Reporting', description: 'Monthly financial reporting process', status: 'Active' },
  { id: 2, name: 'Procurement', description: 'Vendor procurement and approval process', status: 'Active' },
  { id: 3, name: 'HR Onboarding', description: 'Employee onboarding process', status: 'Inactive' }
]);

const subProcesses = ref([
  { id: 1, name: 'Invoice Processing', description: 'Process vendor invoices', parentProcess: 'Procurement' },
  { id: 2, name: 'Budget Preparation', description: 'Prepare monthly budgets', parentProcess: 'Financial Reporting' },
  { id: 3, name: 'Document Verification', description: 'Verify employee documents', parentProcess: 'HR Onboarding' }
]);

const riskControlMatrix = ref([
  { id: 1, riskId: 'R001', riskDescription: 'Unauthorized access to financial data', control: 'Access control system', riskLevel: 'High' },
  { id: 2, riskId: 'R002', riskDescription: 'Data loss during backup', control: 'Automated backup verification', riskLevel: 'Medium' },
  { id: 3, riskId: 'R003', riskDescription: 'Vendor fraud', control: 'Vendor verification process', riskLevel: 'High' }
]);

const objectives = ref([
  { id: 1, title: 'Improve Data Accuracy', description: 'Reduce data entry errors by 50%', target: '50% reduction', progress: 75, dueDate: '2024-12-31' },
  { id: 2, title: 'Enhance Security', description: 'Implement multi-factor authentication', target: '100% coverage', progress: 60, dueDate: '2024-11-30' }
]);

const risks = ref([
  { id: 1, title: 'Cybersecurity Breach', description: 'Potential unauthorized access to systems', severity: 'High', probability: 30 },
  { id: 2, title: 'Regulatory Compliance', description: 'Risk of non-compliance with regulations', severity: 'Medium', probability: 45 },
  { id: 3, title: 'Operational Disruption', description: 'System downtime affecting operations', severity: 'Medium', probability: 25 }
]);

const exitRisks = ref([
  { id: 1, title: 'Key Personnel Departure', description: 'Loss of critical knowledge and skills', impact: 'High', mitigation: 'Knowledge transfer documentation' },
  { id: 2, title: 'Vendor Contract Termination', description: 'Sudden loss of critical vendor services', impact: 'Medium', mitigation: 'Alternative vendor identification' }
]);

const controlDesigns = ref([
  { id: 1, name: 'Access Control Review', description: 'Monthly review of user access rights', type: 'Preventive', frequency: 'Monthly', effectiveness: 'Effective', lastReview: '2024-01-15' },
  { id: 2, name: 'Data Backup Verification', description: 'Daily verification of backup processes', type: 'Detective', frequency: 'Daily', effectiveness: 'Effective', lastReview: '2024-01-20' },
  { id: 3, name: 'Vendor Due Diligence', description: 'Background checks for new vendors', type: 'Preventive', frequency: 'As needed', effectiveness: 'Partially Effective', lastReview: '2024-01-10' }
]);

const observations = ref([
  {
    id: 1,
    title: 'Inadequate Password Policy',
    description: 'Current password requirements do not meet security standards',
    category: 'Security',
    priority: 'High',
    date: '2024-01-15',
    recommendations: 'Implement stronger password requirements including special characters and minimum length of 12 characters'
  },
  {
    id: 2,
    title: 'Manual Data Entry Errors',
    description: 'High frequency of errors in manual data entry processes',
    category: 'Process',
    priority: 'Medium',
    date: '2024-01-18',
    recommendations: 'Implement automated data validation and double-entry verification for critical data'
  }
]);
</script>

<style scoped>
.min-h-\[calc\(100vh-4rem\)\] {
  min-height: calc(100vh - 4rem);
  min-height: calc(100dvh - 4rem);
}

@media (max-width: 640px) {
  .p-4 {
    padding: 0.75rem;
  }
  
  .gap-6 {
    gap: 1rem;
  }
  
  .mb-8 {
    margin-bottom: 1.5rem;
  }
}

@media (max-width: 480px) {
  .p-4 {
    padding: 0.5rem;
  }
  
  .gap-6 {
    gap: 0.75rem;
  }
  
  .mb-8 {
    margin-bottom: 1rem;
  }
}
</style>