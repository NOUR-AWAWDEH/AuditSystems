<template>
    <div class="bg-white rounded-lg shadow-sm border border-gray-200">
        <div class="p-4 md:p-6">
            <h2 v-if="title" class="text-lg font-semibold text-secondary-base mb-4">
                {{ title }}
            </h2>
            <ReusableTable
                :items="flattenedItems"
                :columns="enhancedColumns"
                :is-loading="isLoading"
                :actions="[]"
                :show-sr-no="showSrNo"
                :no-data-message="noDataMessage"
            >
                <!-- Custom hierarchy cell with indentation -->
                <template #cell-hierarchy="{ item, value }">
                    <div :class="['flex items-center', getIndentationClass(item)]">
                        <!-- Expand/Collapse button for items with children -->
                        <button
                            v-if="!item[childIndicatorKey] && hasChildren(item)"
                            @click="toggleChildren(item[itemKey])"
                            class="mr-2 p-1 hover:bg-gray-100 rounded transition-colors"
                        >
                            <svg 
                                :class="['w-4 h-4 transition-transform', isExpanded(item[itemKey]) ? 'rotate-90' : '']"
                                fill="none" 
                                stroke="currentColor" 
                                viewBox="0 0 24 24"
                            >
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                            </svg>
                        </button>
                        
                        <!-- Child indicator -->
                        <div v-if="item[childIndicatorKey]" class="mr-2 text-gray-400">
                            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                            </svg>
                        </div>
                        
                        <span :class="getTextClass(item)">
                            {{ value }}
                        </span>
                    </div>
                </template>
                
                <!-- Custom actions cell -->
                <template #cell-actions="{ item }">
                    <div class="flex flex-wrap gap-2">
                        <template v-for="action in getActionsForItem(item)" :key="action.label">
                            <button
                                @click="$emit(action.event, item)"
                                :class="action.class || 'font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4'"
                            >
                                {{ action.label }}
                            </button>
                        </template>
                    </div>
                </template>
                
                <!-- Pass through other custom slots -->
                <template v-for="(_, slotName) in $slots" :key="slotName" #[slotName]="slotProps">
                    <slot :name="slotName" v-bind="slotProps" />
                </template>
            </ReusableTable>
        </div>
    </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import ReusableTable from './ReusableTable.vue';

const props = defineProps({
    data: {
        type: Array,
        required: true
    },
    columns: {
        type: Array,
        required: true
    },
    
    // Hierarchy configuration
    itemKey: {
        type: String,
        default: 'id'
    },
    childrenKey: {
        type: String,
        default: 'children'
    },
    childIndicatorKey: {
        type: String,
        default: 'isChild'
    },
    parentKey: {
        type: String,
        default: 'parentId'
    },
    hierarchyColumn: {
        type: String,
        required: true // The column that should show hierarchy (e.g., 'name')
    },
    
    // Actions configuration
    parentActions: {
        type: Array,
        default: () => []
    },
    childActions: {
        type: Array,
        default: () => []
    },
    
    // Display props
    title: {
        type: String,
        default: ''
    },
    isLoading: {
        type: Boolean,
        default: false
    },
    showSrNo: {
        type: Boolean,
        default: false
    },
    noDataMessage: {
        type: String,
        default: 'No data available.'
    },
    
    // Styling props
    indentationClass: {
        type: String,
        default: 'pl-8'
    },
    parentTextClass: {
        type: String,
        default: 'text-gray-900 font-medium'
    },
    childTextClass: {
        type: String,
        default: 'text-gray-700'
    }
});

defineEmits([
    'row-click',
    'edit-parent',
    'delete-parent',
    'add-child',
    'edit-child',
    'delete-child'
]);

// Track expanded items
const expandedItems = ref(new Set());

// Toggle children visibility
const toggleChildren = (itemId) => {
    if (expandedItems.value.has(itemId)) {
        expandedItems.value.delete(itemId);
    } else {
        expandedItems.value.add(itemId);
    }
};

// Check if item is expanded
const isExpanded = (itemId) => {
    return expandedItems.value.has(itemId);
};

// Check if item has children
const hasChildren = (item) => {
    return item[props.childrenKey] && item[props.childrenKey].length > 0;
};

// Get indentation class for item
const getIndentationClass = (item) => {
    return item[props.childIndicatorKey] ? props.indentationClass : '';
};

// Get text class for item
const getTextClass = (item) => {
    return item[props.childIndicatorKey] ? props.childTextClass : props.parentTextClass;
};

// Get actions for specific item
const getActionsForItem = (item) => {
    return item[props.childIndicatorKey] ? props.childActions : props.parentActions;
};

// Enhanced columns with hierarchy support
const enhancedColumns = computed(() => {
    return props.columns.map(column => {
        if (column.key === props.hierarchyColumn) {
            return {
                ...column,
                key: 'hierarchy'
            };
        }
        return column;
    }).concat([{ key: 'actions', label: 'Actions' }]);
});

// Flatten items for table display
const flattenedItems = computed(() => {
    const items = [];
    
    // Fix: Use props.data instead of props.items
    if (!props.data) return items;
    
    props.data.forEach(item => {
        // Add the parent item with hierarchy value
        items.push({
            ...item,
            [props.childIndicatorKey]: false,
            hierarchy: item[props.hierarchyColumn]
        });
        
        // Add children if expanded
        if (isExpanded(item[props.itemKey]) && hasChildren(item)) {
            item[props.childrenKey].forEach(child => {
                items.push({
                    ...child,
                    [props.childIndicatorKey]: true,
                    [props.parentKey]: item[props.itemKey],
                    hierarchy: child[props.hierarchyColumn]
                });
            });
        }
    });
    
    return items;
});
</script>