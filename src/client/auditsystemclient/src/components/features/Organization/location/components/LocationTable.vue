<template>
    <div class="bg-white rounded-lg shadow-sm border border-gray-200">
        <div class="p-4 md:p-6">
            <h2 class="text-lg font-semibold text-secondary-base mb-4">
                Locations
            </h2>
            <ReusableTable
                :items="locations"
                :columns="locationColumns"
                :is-loading="isLoading"
                :actions="locationActions"
                @edit-item="$emit('edit-location', $event)"
                @delete-item="$emit('delete-location', $event)"
            />
        </div>
    </div>
</template>

<script setup>
import ReusableTable from '@/components/common/ReusableTable.vue';
import { locationTableConfig } from '../config/locationTableConfig';

defineProps({
    locations: {
        type: Array,
        required: true
    },
    isLoading: {
        type: Boolean,
        default: false
    }
});

defineEmits(['edit-location', 'delete-location']);

const { locationColumns, locationActions } = locationTableConfig;
</script>