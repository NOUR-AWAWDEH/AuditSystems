<template>
  <div class="w-full">
    <!-- Desktop Table View -->
    <div class="hidden md:block">
      <div class="overflow-x-auto shadow-md rounded-lg border border-gray-200">
        <table class="w-full text-sm text-left text-gray-base table-fixed">
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
                :colspan="columns.length + (showSrNo ? 1 : 0) + (actions.length ? 1 : 0)"
                class="px-6 py-8 text-center text-gray-base"
              >
                <div class="flex items-center justify-center space-x-2">
                  <div
                    class="inline-block animate-spin rounded-full h-4 w-4 border-b-2 border-primary-base"
                  ></div>
                  <span>Loading data...</span>
                </div>
              </td>
            </tr>

            <!-- No Data -->
            <tr v-else-if="items.length === 0">
              <td
                :colspan="columns.length + (showSrNo ? 1 : 0) + (actions.length ? 1 : 0)"
                class="px-6 py-8 text-center text-gray-base font-bold "
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
              <td v-if="showSrNo" class="px-6 py-4 border-r border-gray-200 border-t text-gray-600">
                {{ index + 1 }}
              </td>

              <td
                v-for="column in columns"
                :key="column.key"
                class="px-6 py-4 border-r border-gray-200 border-t last:border-r-0 align-top min-w-0 wrap-break-word"
              >
                <slot
                  :name="`cell-${column.key}`"
                  :item="item"
                  :value="item[column.key]"
                  :column="column"
                >
                  <span
                    v-if="column.type === 'date'"
                    class="text-gray-900 font-medium wrap-break-word min-w-0"
                  >
                    {{ formatDate(item[column.key]) }}
                  </span>
                  <span v-else class="text-gray-900 font-medium wrap-break-word min-w-0">
                    {{ item[column.key] ?? "—" }}
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
                    :class="
                      action.class ||
                      'font-medium text-primary-base hover:text-primary-shade transition-colors'
                    "
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
import { formatDate } from "@/utils/dateUtils";
defineOptions({ inheritAttrs: false });

defineProps({
  items: { type: Array, required: true },
  columns: { type: Array, default: () => [] },
  itemKey: { type: String, default: "id" },
  showSrNo: { type: Boolean, default: true },
  isLoading: { type: Boolean, default: false },
  noDataMessage: { type: String, default: "No data available." },
  actions: { type: Array, default: () => [] },
});

defineEmits(["row-click"]);
</script>

<style scoped>
.wrap-break-word {
  word-break: break-word;
  overflow-wrap: break-word;
}
</style>
