<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import BaseNotification from '@/components/base/BaseNotification.vue'; // Import the notification component

const route = useRoute();
const transitionName = computed(() => route.meta.transition || 'fade');
</script>

<template>
  <div class="flex flex-col min-h-screen font-poppins">
    <router-view v-slot="{ Component }">
      <transition :name="transitionName">
        <component :is="Component" />
      </transition>
    </router-view>
    <BaseNotification /> <!-- Add the notification component here -->
  </div>
</template>
