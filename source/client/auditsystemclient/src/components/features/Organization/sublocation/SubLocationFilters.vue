<template>
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 md:p-6 mb-6">
        <div class="flex flex-col lg:flex-row lg:items-end gap-4">
            <!-- Location Selection -->
            <div class="flex-1">
                <label for="location-select" class="block text-lg font-bold text-secondary-base mb-2">
                    Select Location
                </label>
                <select 
                    id="location-select"
                    :value="selectedLocationId"
                    @change="$emit('update:selectedLocationId', $event.target.value)"
                    class="w-full px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base"
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
            </div>
            
            <!-- Search Input -->
            <div v-if="selectedLocationId" class="flex-1">
                <label for="search" class="block text-sm font-medium text-secondary-shade mb-2">
                    Search Sublocations
                </label>
                <BaseInput
                    id="search"
                    :model-value="search"
                    @update:model-value="$emit('update:search', $event)"
                    placeholder="Search by name or address..."
                    :input-class="'w-full px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base'"
                />
            </div>
            
            <!-- Sort Dropdown -->
            <div v-if="selectedLocationId" class="flex-shrink-0">
                <label for="sort" class="block text-sm font-medium text-secondary-shade mb-2">
                    Sort By
                </label>
                <select
                    id="sort"
                    :value="sort"
                    @change="$emit('update:sort', $event.target.value)"
                    class="px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base"
                >
                    <option value="name">Name</option>
                    <option value="address">Address</option>
                    <option value="createdAt">Created Date</option>
                </select>
            </div>
            
            <!-- Refresh Button -->
            <div v-if="selectedLocationId" class="flex-shrink-0">
                <BaseButton
                    @click="$emit('refresh')"
                    class="text-white bg-primary-base px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base"
                >
                    Refresh
                </BaseButton>
            </div>
        </div>
    </div>
</template>

<script setup>
import BaseInput from '@/components/base/BaseInput.vue';
import BaseButton from '@/components/base/BaseButton.vue';

defineProps({
    locations: Array,
    selectedLocationId: String,
    search: String,
    sort: String
});

defineEmits(['update:selectedLocationId', 'update:search', 'update:sort', 'refresh']);
</script>