<template>
  <router-link :to="to" :class="linkClasses">
    <slot />
  </router-link>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  to: {
    type: String,
    required: true,
  },
  variant: {
    type: String,
    default: 'default',
    validator: (value) => ['default', 'primary', 'logo'].includes(value),
  },
});

const linkClasses = computed(() => {
  let classes = 'transition duration-300';

  switch (props.variant) {
    case 'logo':
      classes += ' text-lg font-bold hover:text-secondary-tint';
      break;
    case 'primary':
      classes += ' bg-primary-base hover:bg-primary-shade text-white px-4 py-2 rounded';
      break;
    default:
      classes += ' hover:text-secondary-tint';
      break;
  }

  return classes;
});
</script>
