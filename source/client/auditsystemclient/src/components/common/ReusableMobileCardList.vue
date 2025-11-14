<template>
  <!-- ReusableMobileCardList.vue -->
  <div class="flex flex-col gap-4">
    <!-- Loading State -->
    <div v-if="isLoading" class="text-center py-8">
      <div class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-primary-base"></div>
      <p class="mt-2 text-gray-600 text-sm">Loading data...</p>
    </div>

    <!-- No Data -->
    <div v-else-if="items.length === 0" class="text-gray-500 text-center py-6">
      {{ noDataMessage }}
    </div>

    <!-- Card List -->
    <div
      v-else
      v-for="(item, index) in items"
      :key="item[itemKey] || index"
      class="bg-white rounded-lg shadow p-4 border border-gray-200 relative"
    >
      <!-- Serial Number Badge -->
      <span
        v-if="showNumber"
        class="absolute -top-2 left-4 bg-secondary-base text-white text-xs font-bold px-2 py-1 rounded-full"
      >
        {{ index + 1 }}
      </span>

      <!-- Card Content -->
      <div class="flex justify-between items-start" :class="{ 'pt-2': showNumber }">
        <div class="flex-1 min-w-0">
          <slot name="card-content" :item="item" :index="index">
            <!-- Default Content Fallback -->
            <div v-for="column in columns" :key="column.key" class="mb-2">
              <span class="text-xs font-semibold text-gray-600 uppercase tracking-wider">
                {{ column.label }}
              </span>
              <p class="text-sm text-gray-900 mt-1 wrap-break-word">
                {{ item[column.key] ?? "—" }}
              </p>
            </div>
          </slot>
        </div>

        <!-- Actions -->
        <div class="flex flex-col gap-2 ml-4 shrink-0">
          <slot name="card-actions" :item="item" :index="index"></slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
defineProps({
  items: { type: Array, required: true },
  columns: { type: Array, default: () => [] },
  itemKey: { type: String, default: "id" },
  showNumber: { type: Boolean, default: true },
  isLoading: { type: Boolean, default: false },
  noDataMessage: { type: String, default: "No data available." },
});

defineEmits(["row-click"]);
</script>
