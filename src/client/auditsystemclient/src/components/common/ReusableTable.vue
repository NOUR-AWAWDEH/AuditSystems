<template>
  <div class="overflow-x-auto shadow-md sm:rounded-lg">
    <table class="w-full text-sm text-left text-gray-base border border-gray-300">
      <thead class="text-xs text-white uppercase bg-secondary-base">
        <tr>
          <th v-if="showSrNo" scope="col" class="px-6 py-3 w-16 border border-gray-300">Sr. No.</th>
          <th v-for="column in columns" :key="column.key" scope="col" class="px-6 py-3 border border-gray-300">
            {{ column.label }}
          </th>
          <th v-if="actions.length > 0" scope="col" class="px-6 py-3 border border-gray-300">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="isLoading" class="bg-white">
          <td :colspan="columns.length + (showSrNo ? 1 : 0) + (actions.length > 0 ? 1 : 0)" class="px-6 py-4 text-center text-gray-base border border-gray-300">
            Loading data...
          </td>
        </tr>
        <tr v-else-if="items.length === 0" class="bg-white">
          <td :colspan="columns.length + (showSrNo ? 1 : 0) + (actions.length > 0 ? 1 : 0)" class="px-6 py-4 text-center text-gray-base border border-gray-300">
            {{ noDataMessage }}
          </td>
        </tr>
        <tr
          v-for="(item, index) in items"
          :key="item[itemKey] || index"
          class="bg-white hover:bg-gray-tint/50"
          @click="$emit('row-click', item)"
          :class="{'cursor-pointer': !!$attrs['onRow-click']}"
        >
          <td v-if="showSrNo" class="px-6 py-4 border border-gray-300">{{ index + 1 }}</td>
          <td v-for="column in columns" :key="column.key" class="px-6 py-4 font-medium text-gray-shade whitespace-nowrap border border-gray-300">
            <!-- Use named slot if available -->
            <slot :name="`cell-${column.key}`" :item="item" :value="item[column.key]" :column="column">
              <!-- Default formatting based on column type -->
              <span v-if="column.type === 'date'">
                {{ formatDate(item[column.key]) }}
              </span>
              <span v-else>
                {{ item[column.key] }}
              </span>
            </slot>
          </td>
          <td v-if="actions.length > 0" class="px-6 py-4 border border-gray-300">
            <button
              v-for="action in actions"
              :key="action.label"
              @click.stop="$emit(action.event, item)"
              :class="action.class || 'font-medium text-primary-base dark:text-primary-tint hover:underline mr-2 last:mr-0'"
            >
              {{ action.label }}
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { defineProps, defineEmits } from 'vue';

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
</style>
