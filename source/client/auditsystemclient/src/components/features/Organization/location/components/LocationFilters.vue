<template>
    <div class="bg-white rounded-lg shadow-sm border border-gray-200 p-4 md:p-6 mb-6">
        <div class="flex flex-col lg:flex-row lg:items-end gap-4">
            <!-- Search Input -->
            <div class="flex-1">
                <label for="search" class="block text-lg font-bold text-secondary-base mb-2">
                    Search Locations
                </label>
                <BaseInput
                    id="search"
                    :model-value="search"
                    @update:model-value="$emit('update:search', $event)"
                    placeholder="Search by location name, address, or sublocation..."
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
                    :value="sort"
                    @change="$emit('update:sort', $event.target.value)"
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
                    @click="$emit('refresh')"
                    class="text-white bg-primary-base px-3 py-2 border border-gray-tint rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-primary-base focus:border-primary-base"
                >
                    Search
                </BaseButton>
            </div>
        </div>
    </div>
</template>

<script setup>
import BaseInput from '@/components/base/BaseInput.vue';
import BaseButton from '@/components/base/BaseButton.vue';

defineProps({
    search: String,
    sort: String
});

defineEmits(['update:search', 'update:sort', 'refresh']);
</script>