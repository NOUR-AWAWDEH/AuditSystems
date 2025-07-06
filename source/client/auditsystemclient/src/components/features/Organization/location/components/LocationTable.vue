<template>
    <div class="bg-white rounded-lg shadow-sm border border-gray-200">
        <div class="p-4 md:p-6">
            <h2 class="text-lg font-semibold text-secondary-base mb-4">
                Locations & Sublocations
            </h2>
            <ReusableTable
                :items="flattenedItems"
                :columns="enhancedColumns"
                :is-loading="isLoading"
                :actions="[]"
                :expand-by-default="true"
                class="hierarchical-table"
            >
                <template #cell-hierarchy="{ item, value }">
                    <div :class="['flex items-center', getIndentationClass(item)]">
                        <ExpandButton
                            v-if="!item.isSubLocation && hasChildren(item)"
                            :expanded="isExpanded(item.id)"
                            :children-count="item.sublocations.length"
                            @click.stop="toggleChildren(item.id)"
                        />

                        <ChildIndicator v-if="item.isSubLocation" class="hidden md:block" />

                        <span :class="getTextClass(item)">
                            {{ value }}
                            <span
                                v-if="!item.isSubLocation && hasChildren(item)"
                                class="text-sm text-gray-500 ml-2"
                            >
                                ({{ isExpanded(item.id) ? 'Hide' : 'Show' }}
                                {{ item.sublocations.length }} sublocation{{ item.sublocations.length !== 1 ? 's' : '' }})
                            </span>
                        </span>
                    </div>
                </template>

                <template #cell-actions="{ item }">
                    <div class="flex flex-wrap gap-2">
                        <template v-if="!item.isSubLocation">
                            <button
                                @click.stop="$emit('edit-location', item)"
                                class="font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4"
                            >
                                Edit
                            </button>
                            <button
                                @click.stop="$emit('delete-location', item)"
                                class="font-medium text-red-600 hover:text-red-800 hover:underline mr-4"
                            >
                                Delete
                            </button>
                            <button
                                @click.stop="$emit('add-sublocation', item.id)"
                                class="font-medium text-green-600 hover:text-green-800 hover:underline"
                            >
                                Add Sublocation
                            </button>
                        </template>
                        <template v-else>
                            <button
                                @click.stop="$emit('edit-sublocation', item)"
                                class="font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4"
                            >
                                Edit
                            </button>
                            <button
                                @click.stop="$emit('delete-sublocation', item)"
                                class="font-medium text-red-600 hover:text-red-800 hover:underline"
                            >
                                Delete
                            </button>
                        </template>
                    </div>
                </template>
            </ReusableTable>
        </div>
    </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import ReusableTable from '@/components/common/ReusableTable.vue';
import ExpandButton from '@/components/common/partials/ExpandButton.vue';
import ChildIndicator from '@/components/common/partials/ChildIndicator.vue';

const props = defineProps({
    locations: Array,
    sublocations: Array,
    isLoading: Boolean
});

defineEmits([
    'edit-location',
    'delete-location',
    'add-sublocation',
    'edit-sublocation',
    'delete-sublocation'
]);

const expandedItems = ref(new Set());

const locationColumns = [
    { key: 'hierarchy', label: 'Name' },
    { key: 'address', label: 'Address' },
    { key: 'createdAt', label: 'Created Date', type: 'date' }
];

const enhancedColumns = computed(() => {
    return [...locationColumns, { key: 'actions', label: 'Actions' }];
});

const hasChildren = (item) => Array.isArray(item.sublocations) && item.sublocations.length > 0;

const toggleChildren = (id) => {
    expandedItems.value.has(id) ? expandedItems.value.delete(id) : expandedItems.value.add(id);
};

const isExpanded = (id) => expandedItems.value.has(id);

const getIndentationClass = (item) => item.isSubLocation ? 'md:pl-8' : '';

const getTextClass = (item) => item.isSubLocation ? 'text-gray-700' : 'text-gray-900 font-medium';

const flattenedItems = computed(() => {
    const items = [];

    props.locations.forEach(location => {
        // Push the parent location
        items.push({
            ...location,
            isSubLocation: false,
            hierarchy: location.name
        });

        // Push the children if expanded
        if (isExpanded(location.id) && hasChildren(location)) {
            location.sublocations.forEach(sublocation => {
                items.push({
                    ...sublocation,
                    isSubLocation: true,
                    locationId: location.id,
                    hierarchy: sublocation.name,
                    locationName: location.name
                });
            });
        }
    });

    return items;
});

onMounted(() => {
    // Expand all items by default
    props.locations.forEach(location => {
        if (hasChildren(location)) {
            expandedItems.value.add(location.id);
        }
    });
});
</script>

<style scoped>
.hierarchical-table .table-row {
    transition: all 0.2s ease;
}
</style>