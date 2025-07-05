<template>
  <div class="flex flex-wrap gap-2">
    <button
      v-for="action in actions"
      :key="action.label"
      @click.stop="$emit(action.event, item)"
      :class="action.class || 'font-medium text-blue-600 hover:text-blue-800 hover:underline mr-4'"
    >
      {{ action.label }}
    </button>
  </div>
</template>

<script setup>
const props = defineProps({
  actions: {
    type: Array,
    required: true
  },
  item: {
    type: Object,
    required: true
  }
});

defineEmits(['emit-action']);
</script>