<template>
  <div class="w-full">
    <!-- Mobile Card View -->
    <div class="block md:hidden">
      <!-- Loading -->
      <div v-if="isLoading" class="text-center py-8">
        <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-primary-base"></div>
        <p class="mt-2 text-gray-600 text-sm">Loading data...</p>
      </div>

      <!-- No data -->
      <div v-else-if="items.length === 0" class="text-center py-8 text-gray-base">
        <p class="text-sm">{{ noDataMessage }}</p>
      </div>

      <!-- Data -->
      <div v-else class="space-y-3 px-2 sm:px-4">
        <div
          v-for="(item, index) in items"
          :key="item[itemKey] || index"
          class="rounded-lg shadow-sm border border-gray-200 p-4 bg-white hover:shadow-md transition-shadow"
          :class="{ 'cursor-pointer': !!$attrs['onRow-click'] }"
          @click="$emit('row-click', item)"
        >
          <!-- Serial Number -->
          <div v-if="showSrNo" class="text-xs text-gray-500 mb-3 font-medium">
            {{ index + 1 }}
          </div>

          <!-- Columns -->
          <div class="space-y-3">
            <div v-for="column in columns" :key="column.key">
              <div class="flex flex-col space-y-1">
                <span class="text-xs font-semibold text-gray-600 uppercase tracking-wider">
                  {{ column.label }}
                </span>
                <slot
                  :name="`cell-${column.key}`"
                  :item="item"
                  :value="item[column.key]"
                  :column="column"
                >
                  <span
                    v-if="column.type === 'date'"
                    class="text-sm text-gray-900 font-medium break-words"
                  >
                    {{ formatDate(item[column.key]) }}
                  </span>
                  <span v-else class="text-sm text-gray-900 font-medium break-words">
                    {{ item[column.key] ?? '—' }}
                  </span>
                </slot>
              </div>
            </div>
          </div>

          <!-- Mobile Actions -->
          <div
            v-if="actions.length > 0"
            class="mt-4 pt-3 border-t border-gray-100 flex flex-wrap gap-2"
          >
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
        <table class="w-full text-sm text-left text-gray-base min-w-max">
          <thead class="text-xs text-white uppercase bg-secondary-base">
            <tr>
              <th v-if="showSrNo" class="px-6 py-4 w-16 border-r border-gray-300">#</th>
              <th
                v-for="column in columns"
                :key="column.key"
                class="px-6 py-4 border-r border-gray-300 last:border-r-0 whitespace-nowrap"
              >
                {{ column.label }}
              </th>
              <th v-if="actions.length > 0" class="px-6 py-4">Actions</th>
            </tr>
          </thead>

          <tbody class="bg-white divide-y divide-gray-200">
            <!-- Loading -->
            <tr v-if="isLoading">
              <td
                :colspan="columns.length + (showSrNo ? 1 : 0) + (actions.length > 0 ? 1 : 0)"
                class="px-6 py-8 text-center text-gray-base"
              >
                <div class="flex items-center justify-center space-x-2">
                  <div class="inline-block animate-spin rounded-full h-4 w-4 border-b-2 border-primary-base"></div>
                  <span>Loading data...</span>
                </div>
              </td>
            </tr>

            <!-- No Data -->
            <tr v-else-if="items.length === 0">
              <td
                :colspan="columns.length + (showSrNo ? 1 : 0) + (actions.length > 0 ? 1 : 0)"
                class="px-6 py-8 text-center text-gray-base"
              >
                {{ noDataMessage }}
              </td>
            </tr>

            <!-- Rows -->
            <tr
              v-for="(item, index) in items"
              :key="item[itemKey] || index"
              class="hover:bg-gray-50 transition-colors"
              :class="{ 'cursor-pointer': !!$attrs['onRow-click'] }"
              @click="$emit('row-click', item)"
            >
              <td v-if="showSrNo" class="px-6 py-4 border-r border-gray-200 text-gray-600">
                {{ index + 1 }}
              </td>

              <td
                v-for="column in columns"
                :key="column.key"
                class="px-6 py-4 border-r border-gray-200 last:border-r-0 align-top"
              >
                <slot
                  :name="`cell-${column.key}`"
                  :item="item"
                  :value="item[column.key]"
                  :column="column"
                >
                  <span
                    v-if="column.type === 'date'"
                    class="text-gray-900 font-medium whitespace-nowrap"
                  >
                    {{ formatDate(item[column.key]) }}
                  </span>
                  <span v-else class="text-gray-900 font-medium break-words">
                    {{ item[column.key] ?? '—' }}
                  </span>
                </slot>
              </td>

              <!-- Desktop Actions -->
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
defineOptions({ inheritAttrs: false });

defineProps({
  columns: { type: Array, required: true },
  items: { type: Array, required: true },
  itemKey: { type: String, default: 'id' },
  showSrNo: { type: Boolean, default: true },
  isLoading: { type: Boolean, default: false },
  noDataMessage: { type: String, default: 'No data available.' },
  actions: { type: Array, default: () => [] }
});

defineEmits(['row-click']);

function formatDate(value) {
  if (!value) return '—';
  const d = new Date(value);
  if (isNaN(d.getTime())) return 'Invalid Date';
  return d.toLocaleDateString('en-GB', {
    year: 'numeric',
    month: 'short',
    day: '2-digit'
  });
}
</script>

<style scoped>
@media (max-width: 640px) {
  .space-y-3 > * + * {
    margin-top: 0.75rem;
  }
}
table {
  border-collapse: separate;
  border-spacing: 0;
}
th,
td {
  word-break: break-word;
}
</style>
