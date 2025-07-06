<template>
    <div class="space-y-6">
        <!-- Location Selection -->
        <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 md:p-6">
            <div class="flex flex-col lg:flex-row lg:items-center lg:justify-between">
                <h2 class="text-lg font-semibold text-secondary-base mb-4 lg:mb-0">
                    Sublocation Management
                </h2>
                <div class="flex flex-col sm:flex-row gap-4">
                    <select 
                        v-model="selectedLocationId"
                        @change="handleLocationChange"
                        class="px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base"
                    >
                        <option value="">Select a Location</option>
                        <option 
                            v-for="location in locations" 
                            :key="location.id" 
                            :value="location.id"
                        >
                            {{ location.name }}
                        </option>
                    </select>
                    <BaseButton 
                        v-if="selectedLocationId"
                        @click="handleAddSublocation"
                        class="text-white bg-primary-base px-4 py-2 rounded-md"
                    >
                        Add Sublocation
                    </BaseButton>
                </div>
            </div>
        </div>

        <!-- Sublocation Filters -->
        <div v-if="selectedLocationId" class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 md:p-6">
            <div class="flex flex-col lg:flex-row lg:items-end gap-4">
                <!-- Search Input -->
                <div class="flex-1">
                    <label for="search" class="block text-lg font-bold text-secondary-base mb-2">
                        Search Sublocations
                    </label>
                    <BaseInput
                        id="search"
                        v-model="searchQuery"
                        placeholder="Search by name or address..."
                        :input-class="'w-full px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base'"
                    />
                </div>
                
                <!-- Sort Dropdown -->
                <div class="flex-shrink-0">
                    <label for="sort" class="block text-sm font-medium text-secondary-shade mb-2">
                        Sort By
                    </label>
                    <select
                        id="sort"
                        v-model="sortBy"
                        class="px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base"
                    >
                        <option value="name">Name</option>
                        <option value="address">Address</option>
                        <option value="createdAt">Created Date</option>
                    </select>
                </div>
                
                <!-- Search Button -->
                <div class="flex-shrink-0">
                    <BaseButton
                        @click="refreshData"
                        class="text-white bg-primary-base px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base"
                    >
                        Search
                    </BaseButton>
                </div>
            </div>
        </div>

        <!-- Sublocation Table -->
        <div v-if="selectedLocationId" class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-4 md:p-6">
                <h2 class="text-lg font-semibold text-secondary-base mb-4">
                    Sublocations
                </h2>
                
                <!-- Selected Location Info -->
                <div v-if="selectedLocation" class="mb-4 p-3 bg-gray-50 rounded-md">
                    <p class="text-sm text-gray-600">
                        Showing sublocations for: <span class="font-medium text-gray-900">{{ selectedLocation.name }}</span>
                    </p>
                </div>

                <ReusableTable
                    :items="filteredSublocations"
                    :columns="enhancedColumns"
                    :is-loading="isLoading"
                    :actions="[]"
                    class="sublocation-table"
                >
                    <template #cell-actions="{ item }">
                        <div class="flex flex-wrap gap-2">
                            <button
                                @click.stop="handleEditSublocation(item)"
                                class="font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4"
                            >
                                Edit
                            </button>
                            <button
                                @click.stop="handleDeleteSublocation(item)"
                                class="font-medium text-red-600 hover:text-red-800 hover:underline"
                            >
                                Delete
                            </button>
                        </div>
                    </template>
                </ReusableTable>

                <div v-if="!isLoading && filteredSublocations.length === 0" class="text-center py-8">
                    <p class="text-gray-500">
                        {{ searchQuery ? 'No sublocations found matching your search.' : 'No sublocations found for this location.' }}
                    </p>
                </div>
            </div>
        </div>

        <!-- No Location Selected -->
        <div v-else class="bg-white rounded-lg shadow-sm border border-gray-200">
            <div class="p-4 md:p-6 text-center py-12">
                <p class="text-lg mb-2 text-gray-500">Select a location to view its sublocations</p>
                <p class="text-sm text-gray-400">Choose a location from the dropdown above to get started</p>
            </div>
        </div>

        <!-- Add Sublocation Modal -->
        <AddSublocationModal
            :is-visible="showAddSublocationModal"
            :location-id="selectedLocationId"
            @close="handleCloseAddSublocationModal"
            @sublocation-added="handleSublocationAdded"
        />

        <!-- Edit Sublocation Modal -->
        <EditSublocationModal
            v-if="showEditSublocationModal"
            :is-visible="showEditSublocationModal"
            :sublocation="sublocationToEdit"
            @close="handleCloseEditSublocationModal"
            @updated="handleSublocationUpdated"
        />

        <!-- Confirmation Dialog -->
        <ConfirmDialog
            v-model="showDeleteDialog"
            :title="deleteDialogTitle"
            :message="deleteDialogMessage"
            :highlight-text="itemToDelete?.name || ''"
            :confirm-text="'Delete'"
            :cancel-text="'Cancel'"
            @confirm="confirmDelete"
            @cancel="cancelDelete"
        />
    </div>
</template>

<script setup>
import { computed } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import BaseInput from '@/components/base/BaseInput.vue';
import BaseButton from '@/components/base/BaseButton.vue';
import ConfirmDialog from '@/components/base/ConfirmDialog.vue';
import AddSublocationModal from './AddSublocationModal.vue';
import EditSublocationModal from './EditSublocationModal.vue';
import { useSubLocationTable } from './composables/useSubLocationTable';
import { useSubLocationFilters } from './composables/useSubLocationFilters';
import { useSubLocationActions } from './composables/useSubLocationActions';
import { useNotification } from '@/composables/notification/useNotification';

const props = defineProps({
    locations: {
        type: Array,
        default: () => []
    }
});

const { showNotification } = useNotification();

// Use composables
const {
    sublocations,
    isLoading,
    selectedLocationId,
    selectedLocation,
    refreshData,
    handleLocationChange,
    handleSublocationAdded
} = useSubLocationTable(props.locations);

const {
    searchQuery,
    sortBy,
    filteredSublocations
} = useSubLocationFilters(sublocations);

const {
    showAddSublocationModal,
    showEditSublocationModal,
    sublocationToEdit,
    showDeleteDialog,
    deleteDialogTitle,
    deleteDialogMessage,
    itemToDelete,
    handleAddSublocation,
    handleCloseAddSublocationModal,
    handleEditSublocation,
    handleCloseEditSublocationModal,
    handleSublocationUpdated,
    handleDeleteSublocation,
    confirmDelete,
    cancelDelete
} = useSubLocationActions(refreshData, showNotification);

// Table columns
const sublocationColumns = [
    { key: 'name', label: 'Name' },
    { key: 'address', label: 'Address' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...sublocationColumns, { key: 'actions', label: 'Actions' }];
});
</script>

<style scoped>
.sublocation-table .table-row {
    transition: all 0.2s ease;
}
</style>