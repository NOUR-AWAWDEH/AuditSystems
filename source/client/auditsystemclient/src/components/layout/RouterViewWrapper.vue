<template>
  <router-view v-slot="{ Component }">
    <transition :name="transitionName">
      <component :is="Component" />
    </transition>
  </router-view>
</template>

<script setup>
import { useAppLayout } from '@/composables/layout/useAppLayout';

const { transitionName } = useAppLayout();
</script>