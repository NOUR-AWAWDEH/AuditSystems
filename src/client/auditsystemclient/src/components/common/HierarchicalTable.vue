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
        <template #cell-hierarchy="{ item, value }">
          <div :class="['flex items-center', getIndentationClass(item)]">
            <ExpandButton
              v-if="!item[childIndicatorKey] && hasChildren(item)"
              :expanded="isExpanded(item[itemKey])"
              :children-count="item[childrenKey].length"
              @click.stop="toggleChildren(item[itemKey])"
            />

            <ChildIndicator v-if="item[childIndicatorKey]" class="hidden md:block" />

            <span :class="getTextClass(item)">
              {{ value }}
              <span
                v-if="!item[childIndicatorKey] && hasChildren(item)"
                class="text-sm text-gray-500 ml-2"
              >
                ({{ isExpanded(item[itemKey]) ? 'Hide' : 'Show' }}
                {{ item[childrenKey].length }} sublocation{{ item[childrenKey].length !== 1 ? 's' : '' }})
              </span>
            </span>
          </div>
        </template>

        <template #cell-actions="{ item }">
          <ActionButtons :item="item" :actions="getActionsForItem(item)" @emit-action="$emit" />
        </template>

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
import ExpandButton from './partials/ExpandButton.vue';
import ChildIndicator from './partials/ChildIndicator.vue';
import ActionButtons from './partials/ActionButtons.vue';

const props = defineProps({
  data: Array,
  columns: Array,
  itemKey: { type: String, default: 'id' },
  childrenKey: { type: String, default: 'children' },
  childIndicatorKey: { type: String, default: 'isChild' },
  parentKey: { type: String, default: 'parentId' },
  hierarchyColumn: { type: String, required: true },
  parentActions: { type: Array, default: () => [] },
  childActions: { type: Array, default: () => [] },
  title: String,
  isLoading: Boolean,
  showSrNo: Boolean,
  noDataMessage: String,
  indentationClass: { type: String, default: 'pl-8' },
  parentTextClass: { type: String, default: 'text-gray-900 font-medium' },
  childTextClass: { type: String, default: 'text-gray-700' },
  expandByDefault: Boolean
});

const emit = defineEmits([
  'row-click',
  'edit-parent',
  'delete-parent',
  'add-child',
  'edit-child',
  'delete-child'
]);

const expandedItems = ref(new Set());

const hasChildren = (item) => Array.isArray(item[props.childrenKey]) && item[props.childrenKey].length > 0;

onMounted(() => {
  if (props.expandByDefault) {
    props.data.forEach(item => {
      if (hasChildren(item)) expandedItems.value.add(item[props.itemKey]);
    });
  }
});

const toggleChildren = (id) => {
  expandedItems.value.has(id) ? expandedItems.value.delete(id) : expandedItems.value.add(id);
};

const isExpanded = (id) => expandedItems.value.has(id);
const getIndentationClass = (item) => item[props.childIndicatorKey] ? 'md:' + props.indentationClass : '';
const getTextClass = (item) => item[props.childIndicatorKey] ? props.childTextClass : props.parentTextClass;
const getActionsForItem = (item) => item[props.childIndicatorKey] ? props.childActions : props.parentActions;

const enhancedColumns = computed(() => {
  const base = props.columns.map(c => c.key === props.hierarchyColumn ? { ...c, key: 'hierarchy' } : c);
  return [...base, { key: 'actions', label: 'Actions' }];
});

const flattenedItems = computed(() => {
  const items = [];

  props.data.forEach(parent => {
    // Push the parent item
    items.push({
      ...parent,
      [props.childIndicatorKey]: false,
      hierarchy: parent[props.hierarchyColumn]
    });

    // Push the children if expanded
    if (isExpanded(parent[props.itemKey]) && hasChildren(parent)) {
      parent[props.childrenKey].forEach(child => {
        items.push({
          ...child,
          [props.childIndicatorKey]: true,
          [props.parentKey]: parent[props.itemKey],
          hierarchy: child[props.hierarchyColumn],
          locationName: parent.name
        });
      });
    }
  });

  return items;
});

const handleRowClick = (item) => {
  if (!item[props.childIndicatorKey] && hasChildren(item)) toggleChildren(item[props.itemKey]);
  emit('row-click', item);
};
</script>

<style scoped>
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
  from { opacity: 0; transform: translateY(-10px); }
  to   { opacity: 1; transform: translateY(0);   }
}
.expand-button { transition: all 0.2s ease; }
.expand-button:hover { transform: scale(1.1); }
</style>
