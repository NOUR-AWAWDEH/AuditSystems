<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();
const transitionName = computed(() => route.meta.transition || 'fade');
</script>

<template>
  <div class="flex flex-col items-center justify-center min-h-screen font-poppins">
    <router-view v-slot="{ Component }">
      <transition :name="transitionName">
        <component :is="Component" />
      </transition>
    </router-view>
  </div>
</template>
