<template>
    <div class="flex flex-col lg:flex-row lg:items-end gap-4">
        <!-- Search Input -->
        <div class="flex-1 min-w-0">
            <label for="search" class="block text-sm font-semibold text-secondary-base mb-2">
                <svg class="inline w-4 h-4 mr-1 text-secondary-base" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
                </svg>
                Search
            </label>
            <BaseInput
                id="search"
                :model-value="search"
                @update:model-value="$emit('update:search', $event)"
                :placeholder="searchPlaceholder"
                :input-class="'w-full px-4 py-2.5 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all duration-200 hover:border-gray-400'"
            />
        </div>
        
        <!-- Sort Dropdown -->
        <div class="flex-shrink-0">
            <label for="sort" class="block text-sm font-semibold text-secondary-base mb-2">
                <svg class="inline w-4 h-4 mr-1 text-secondary-base" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4h13M3 8h9m-9 4h6m4 0l4-4m0 0l4 4m-4-4v12"></path>
                </svg>
                Sort By
            </label>
            <select
                id="sort"
                :value="sort"
                @change="$emit('update:sort', $event.target.value)"
                class="px-4 py-2.5 border border-secondary-base rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-secondary-shade focus:border-secondary-shade transition-all duration-200 hover:border-secondary-shade cursor-pointer min-w-[140px]"
            >
                <option value="name">Name</option>
                <option value="address" >Address</option>
                <option value="createdAt" >Created Date</option>
            </select>
        </div>
        
        <!-- Search Button -->
        <div class="flex-shrink-0">
            <BaseButton
                @click="$emit('refresh')"
                class="inline-flex items-center px-6 py-2.5 bg-secondary-base text-white font-medium rounded-lg shadow-md hover:bg-secondary-shade focus:outline-none focus:ring-2 focus:ring-secondary-shade focus:ring-offset-2 transition-all duration-200 transform hover:scale-105 active:scale-95"
            >
                <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
                </svg>
                Search
            </BaseButton>
        </div>
    </div>
</template>

<script setup>
import BaseInput from '@/components/base/BaseInput.vue';
import BaseButton from '@/components/base/BaseButton.vue';

defineProps({
    search: {
        type: String,
        default: ''
    },
    sort: {
        type: String,
        default: 'name'
    },
    searchPlaceholder: {
        type: String,
        default: 'Search locations...'
    }
});
defineEmits(['update:search', 'update:sort', 'refresh']);

</script>