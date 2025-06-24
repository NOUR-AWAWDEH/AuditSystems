<template>
  <div>
    <!-- Mobile Card View -->
    <div class="block md:hidden">
      <div v-if="isLoading" class="text-center py-8">
        <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-primary-base"></div>
        <p class="mt-2 text-gray-600 text-sm">Loading data...</p>
      </div>
      <div v-else-if="items.length === 0" class="text-center py-8 text-gray-base">
        <div class="text-gray-400 mb-2">
          <svg class="mx-auto h-12 w-12" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
        </div>
        <p class="text-sm">{{ noDataMessage }}</p>
      </div>
      <div v-else class="space-y-3">
        <div 
          v-for="(item, index) in items" 
          :key="item[itemKey] || index"
          class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 hover:shadow-md transition-shadow"
          @click="$emit('row-click', item)"
          :class="{'cursor-pointer': !!$attrs['onRow-click']}"
        >
          <div v-if="showSrNo" class="text-xs text-gray-500 mb-3 font-medium">
            {{ index + 1 }}
          </div>
          <div class="space-y-3">
            <div v-for="column in columns" :key="column.key" class="">
              <div class="flex flex-col space-y-1">
                <span class="text-xs font-semibold text-gray-600 uppercase tracking-wider">{{ column.label }}</span>
                <slot :name="`cell-${column.key}`" :item="item" :value="item[column.key]" :column="column">
                  <span v-if="column.type === 'date'" class="text-sm text-gray-900 font-medium">
                    {{ formatDate(item[column.key]) }}
                  </span>
                  <span v-else class="text-sm text-gray-900 font-medium break-words">
                    {{ item[column.key] }}
                  </span>
                </slot>
              </div>
            </div>
          </div>
          <div v-if="actions.length > 0" class="mt-4 pt-3 border-t border-gray-100 flex flex-wrap gap-2">
            <button
              v-for="action in actions"
              :key="action.label"
              @click.stop="$emit(action.event, item)"
              :class="action.class || 'px-3 py-2 text-xs font-medium bg-primary-base text-white rounded-md hover:bg-primary-shade transition-colors'"
            >
              {{ action.label }}
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Desktop Table View -->
    <div class="hidden md:block">
      <div class="overflow-x-auto shadow-md rounded-lg border border-gray-200">
        <table class="w-full text-sm text-left text-gray-base">
          <thead class="text-xs text-white uppercase bg-secondary-base">
            <tr>
              <th v-if="showSrNo" scope="col" class="px-6 py-4 w-16 border-r border-gray-300">Sr. No.</th>
              <th v-for="column in columns" :key="column.key" scope="col" class="px-6 py-4 border-r border-gray-300 last:border-r-0">
                {{ column.label }}
              </th>
              <th v-if="actions.length > 0" scope="col" class="px-6 py-4">Actions</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-if="isLoading">
              <td :colspan="columns.length + (showSrNo ? 1 : 0) + (actions.length > 0 ? 1 : 0)" class="px-6 py-8 text-center text-gray-base">
                <div class="flex items-center justify-center space-x-2">
                  <div class="inline-block animate-spin rounded-full h-4 w-4 border-b-2 border-primary-base"></div>
                  <span>Loading data...</span>
                </div>
              </td>
            </tr>
            <tr v-else-if="items.length === 0">
              <td :colspan="columns.length + (showSrNo ? 1 : 0) + (actions.length > 0 ? 1 : 0)" class="px-6 py-8 text-center text-gray-base">
                {{ noDataMessage }}
              </td>
            </tr>
            <tr
              v-for="(item, index) in items"
              :key="item[itemKey] || index"
              class="hover:bg-gray-50 transition-colors"
              @click="$emit('row-click', item)"
              :class="{'cursor-pointer': !!$attrs['onRow-click']}"
            >
              <td v-if="showSrNo" class="px-6 py-4 border-r border-gray-200 text-gray-600">{{ index + 1 }}</td>
              <td v-for="column in columns" :key="column.key" class="px-6 py-4 border-r border-gray-200 last:border-r-0">
                <slot :name="`cell-${column.key}`" :item="item" :value="item[column.key]" :column="column">
                  <span v-if="column.type === 'date'" class="text-gray-900 font-medium">
                    {{ formatDate(item[column.key]) }}
                  </span>
                  <span v-else class="text-gray-900 font-medium break-words">
                    {{ item[column.key] }}
                  </span>
                </slot>
              </td>
              <td v-if="actions.length > 0" class="px-6 py-4">
                <div class="flex flex-wrap gap-2">
                  <button
                    v-for="action in actions"
                    :key="action.label"
                    @click.stop="$emit(action.event, item)"
                    :class="action.class || 'font-medium text-primary-base hover:text-primary-shade transition-colors'"
                  >
                    {{ action.label }}
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
// Add this line
defineOptions({
  inheritAttrs: false
});

defineProps({
  columns: {
    type: Array,
    required: true, // Example: [{ key: 'name', label: 'Name' }, { key: 'email', label: 'Email' }]
  },
  items: {
    type: Array,
    required: true,
  },
  itemKey: {
    type: String,
    default: 'id', // Unique key for each item in the items array
  },
  showSrNo: {
    type: Boolean,
    default: true,
  },
  isLoading: {
    type: Boolean,
    default: false,
  },
  noDataMessage: {
    type: String,
    default: 'No data available.',
  },
  actions: {
    type: Array,
    default: () => [], // Example: [{label: 'Edit', event: 'edit-item', class: 'text-blue-500'}, {label: 'Delete', event: 'delete-item', class: 'text-red-500'}]
  }
});

defineEmits(['row-click', 'edit-item', 'delete-item', 'view-item', 'download-item']);

// Helper function for date formatting
function formatDate(dateString) {
  if (!dateString) return 'N/A';
  try {
    return new Date(dateString).toLocaleDateString();
  } catch {
    return 'Invalid Date';
  }
}
</script>

<style scoped>
/* Enhanced mobile responsiveness */
@media (max-width: 640px) {
  .space-y-3 > * + * {
    margin-top: 0.75rem;
  }
}
</style>
