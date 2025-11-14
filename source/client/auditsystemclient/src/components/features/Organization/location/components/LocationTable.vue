<template>
  <!--LocationTable.vue-->
  <div>
    <!-- Location Table Container -->
    <div class="bg-white rounded-xl shadow border border-gray-200 overflow-hidden">
      <div class="p-3 sm:p-4 md:p-6">
        <!-- Header with Title and Filters -->
        <div class="flex flex-col gap-4 mb-6">
          <LocationHeader />

          <div class="flex flex-col sm:flex-row sm:items-end gap-3">
            <SearchFilters
              :search="searchQuery"
              :searchPlaceholder="'Search locations...'"
              @update:search="handleSearch"
              class="w-full sm:w-auto md:w-1/2"
            />
          </div>
        </div>

        <div class="hidden md:block rounded-lg border border-gray-200">
          <ReusableTable
            :items="locations"
            :columns="enhancedColumns"
            :is-loading="isLoading"
            :actions="[]"
            :no-data-message="noDataMessage"
            class="location-table min-w-full"
          >
            <template #cell-actions="{ item }">
              <!-- Constrain width & enable horizontal scrolling when needed -->
              <div class="">
                <div class="flex flex-col  2xl:flex-row gap-2 min-w-max">
                  <button
                    @click.stop="handleSelectLocation(item)"
                    class="inline-flex items-center px-2 py-1 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                    :class="{ 'bg-green-200': selectedLocationId === item.id }"
                  >
                    <CheckIcon v-if="selectedLocationId === item.id" size="sm" class="mr-1" />
                    <SelectIcon v-else size="sm" class="mr-1" />
                    <span >{{
                      selectedLocationId === item.id ? "Selected" : "Select"
                    }}</span>
                  </button>

                  <button
                    @click.stop="$emit('edit-location', item)"
                    class="inline-flex items-center px-2 py-1 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                  >
                    <EditIcon size="sm" class="mr-1" />
                    <span>Edit</span>
                  </button>

                  <button
                    @click.stop="$emit('delete-location', item)"
                    class="inline-flex items-center px-2 py-1 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200 whitespace-nowrap"
                  >
                    <DeleteIcon size="sm" class="mr-1" />
                    <span>Delete</span>
                  </button>
                </div>
              </div>
            </template>
          </ReusableTable>
        </div>

        <!-- Mobile Card View -->
        <ReusableMobileCardList
          :items="locations"
          :showNumber="true"
          :isLoading="isLoading"
          :noDataMessage="noDataMessage"
          class="md:hidden"
        >
          <template #card-content="{ item }">
            <div class="space-y-4">
              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  {{ locationColumns[0].label }}
                </div>
                <div class="text-gray-900 font-bold text-base leading-snug wrap-break-word">
                  {{ item.name }}
                </div>
              </div>

              <div class="border-b border-gray-100 pb-3">
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  {{ locationColumns[1].label }}
                </div>
                <div class="text-gray-600 text-sm leading-relaxed wrap-break-word">
                  {{ item.address }}
                </div>
              </div>

              <div>
                <div class="text-[11px] font-semibold text-gray-500 uppercase tracking-wide mb-2">
                  {{ locationColumns[2].label }}
                </div>
                <div class="text-gray-400 text-xs font-medium">
                  {{ formatDate(item.createdAt) }}
                </div>
              </div>
            </div>
          </template>

          <template #card-actions="{ item }">
            <button
              @click.stop="handleSelectLocation(item)"
              class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-green-700 bg-green-100 rounded-md hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200"
              :class="{ 'bg-green-200': selectedLocationId === item.id }"
            >
              <CheckIcon v-if="selectedLocationId === item.id" size="sm" class="mr-1" />
              <SelectIcon v-else size="sm" class="mr-1" />
              {{ selectedLocationId === item.id ? "Selected" : "Select" }}
            </button>

            <button
              @click.stop="$emit('edit-location', item)"
              class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-blue-700 bg-blue-100 rounded-md hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200"
            >
              <EditIcon size="sm" class="mr-1" />
              Edit
            </button>

            <button
              @click.stop="$emit('delete-location', item)"
              class="inline-flex items-center px-3 py-1.5 text-sm font-medium text-red-700 bg-red-100 rounded-md hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200"
            >
              <DeleteIcon size="sm" class="mr-1" />
              Delete
            </button>
          </template>
        </ReusableMobileCardList>

        <!-- No data message -->
        <div v-if="!locations.length" class="text-gray-500 text-center py-6">
          {{ noDataMessage }}
        </div>
      </div>

      <!-- Loading -->
      <div v-if="isLoading" class="text-center py-8">
        <div
          class="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-primary-base"
        ></div>
        <p class="mt-2 text-gray-600 text-sm">Loading data...</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, watch } from "vue";
import ReusableTable from "@/components/common/ReusableTable.vue";
import ReusableMobileCardList from "@/components/common/ReusableMobileCardList.vue";
import SearchFilters from "@/components/common/SearchFilter.vue";
import EditIcon from "@/components/common/icons/EditIcon.vue";
import DeleteIcon from "@/components/common/icons/DeleteIcon.vue";
import CheckIcon from "@/components/common/icons/CheckIcon.vue";
import SelectIcon from "@/components/common/icons/SelectIcon.vue";
import LocationHeader from "./LocationHeader.vue";

const props = defineProps({
  locations: Array,
  isLoading: Boolean,
  searchQuery: String,
  handleSearch: Function,
});

const emit = defineEmits(["add-location", "edit-location", "delete-location", "location-selected"]);

const selectedLocationId = ref(null);

const handleSelectLocation = (location) => {
  selectedLocationId.value = location.id;
  emit("location-selected", location.id);
};

const locationColumns = [
  { key: "name", label: "Name" },
  { key: "address", label: "Address" },
  { key: "createdAt", label: "Created Date", type: "date" },
];

const enhancedColumns = computed(() => [...locationColumns, { key: "actions", label: "Actions" }]);

const noDataMessage = computed(() =>
  props.searchQuery
    ? `No Locations found for "${props.searchQuery}".`
    : "No Locations found yet. Please add a location."
);

watch(
  () => props.locations,
  (newLocations) => {
    if (selectedLocationId.value && newLocations) {
      const exists = newLocations.some((loc) => loc.id === selectedLocationId.value);
      if (!exists) {
        selectedLocationId.value = null;
        emit("location-selected", null);
      }
    }
  },
  { deep: true }
);

const formatDate = (dateStr) => {
  const date = new Date(dateStr);
  return date.toLocaleDateString();
};
</script>

<style scoped>
.location-table .table-row {
  transition: all 0.2s ease;
  cursor: pointer;
}

.location-table .table-row:hover {
  background-color: #f8fafc;
  transform: translateY(-1px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.location-table .table-row.selected {
  background-color: #eff6ff;
  border-left: 4px solid #3b82f6;
}

@media (max-width: 767px) {
  .location-table .table-row:hover {
    transform: none;
    box-shadow: none;
  }
}
</style>
