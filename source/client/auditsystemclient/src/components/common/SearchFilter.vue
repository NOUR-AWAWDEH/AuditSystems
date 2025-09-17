<template>
    <div class="flex flex-col sm:flex-row sm:items-end gap-3">
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
                :input-class="'w-full px-3 py-2 sm:px-4 sm:py-2.5 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-all duration-200 hover:border-gray-400'"
            />
        </div>
        
        <!-- Search Button -->
        <div class="w-full sm:w-auto">
            <BaseButton
                @click="$emit('refresh')"
                class="w-full sm:w-auto inline-flex items-center justify-center px-2 py-1 sm:px-2 sm:py-1 bg-secondary-base text-white font-medium rounded-lg shadow-md hover:bg-secondary-shade focus:outline-none focus:ring-2 focus:ring-secondary-shade focus:ring-offset-2 transition-all duration-200"
            >
                <SearchIcon size="sm" />
                Search
            </BaseButton>
        </div>
    </div>
</template>

<script setup>
import BaseInput from '@/components/base/BaseInput.vue';
import BaseButton from '@/components/base/BaseButton.vue';
import SearchIcon from '@/components/common/icons/SearchIcon.vue';

defineProps({
    search: {
        type: String,
        default: ''
    },
    searchPlaceholder: {
        type: String,
        default: 'Search...'
    }
});
defineEmits(['update:search','refresh']);

</script>