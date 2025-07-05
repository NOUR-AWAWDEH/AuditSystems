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
                @row-click="handleRowClick"
                class="hierarchical-table"
            >
                <!-- Custom hierarchy cell with indentation -->
                <!-- In the hierarchy cell template -->
                <template #cell-hierarchy="{ item, value }">
                    <div :class="['flex items-center', getIndentationClass(item)]">
                        <!-- Enhanced expand/collapse button -->
                        <button
                            v-if="!item[childIndicatorKey] && hasChildren(item)"
                            @click.stop="toggleChildren(item[itemKey])"
                            class="mr-2 p-1 hover:bg-gray-100 rounded transition-colors flex items-center"
                            :class="{
                                'bg-blue-50 text-blue-600': isExpanded(item[itemKey]),
                                'text-gray-500': !isExpanded(item[itemKey])
                            }"
                        >
                            <svg 
                                :class="['w-4 h-4 transition-transform', isExpanded(item[itemKey]) ? 'rotate-90' : '']"
                                fill="none" 
                                stroke="currentColor" 
                                viewBox="0 0 24 24"
                            >
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                            </svg>
                            <span class="ml-1 text-xs" v-if="hasChildren(item)">
                                ({{ item[childrenKey].length }})
                            </span>
                        </button>
                        
                        <!-- Sublocation indicator -->
                        <div v-if="item[childIndicatorKey]" class="mr-2 text-blue-500">
                            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4" />
                            </svg>
                        </div>
                        
                        <span :class="getTextClass(item)">
                            {{ value }}
                            <span v-if="!item[childIndicatorKey] && hasChildren(item)" class="text-sm text-gray-500 ml-2">
                                ({{ isExpanded(item[itemKey]) ? 'Hide' : 'Show' }} {{ item[childrenKey].length }} sublocation{{ item[childrenKey].length !== 1 ? 's' : '' }})
                            </span>
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
import { ref, computed, onMounted } from 'vue';
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
    },
    
    // New prop for default expansion
    expandByDefault: {
        type: Boolean,
        default: false
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

// Check if item has children
const hasChildren = (item) => {
    return item[props.childrenKey] && item[props.childrenKey].length > 0;
};

// Auto-expand based on prop
onMounted(() => {
    if (props.expandByDefault && props.data) {
        props.data.forEach(item => {
            if (hasChildren(item)) {
                expandedItems.value.add(item[props.itemKey]);
            }
        });
    }
});

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

// Add this method to the script section
const handleRowClick = (item) => {
    // Only handle clicks on parent items (locations)
    if (!item[props.childIndicatorKey] && hasChildren(item)) {
        toggleChildren(item[props.itemKey]);
    }
    // Emit the row-click event for other handling
    emit('row-click', item);
};
</script>

<style scoped>
/* Enhanced styling for dropdown-like appearance */
.hierarchical-table .table-row {
    transition: all 0.2s ease;
}

.hierarchical-table .parent-row {
    background-color: #f8fafc;
    border-left: 3px solid #3b82f6;
}

.hierarchical-table .parent-row:hover {
    background-color: #e2e8f0;
    cursor: pointer;
}

.hierarchical-table .child-row {
    background-color: #ffffff;
    border-left: 3px solid #e5e7eb;
    animation: slideDown 0.2s ease-out;
}

@keyframes slideDown {
    from {
        opacity: 0;
        transform: translateY(-10px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}

.expand-button {
    transition: all 0.2s ease;
}

.expand-button:hover {
    transform: scale(1.1);
}
</style>