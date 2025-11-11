<template>
    <!--SearchIcon.vue-->
    <svg 
        ref="svgRef"
        :class="iconClass" 
        fill="none" 
        stroke="currentColor" 
        viewBox="0 0 24 24"
        xmlns="http://www.w3.org/2000/svg"
        role="img"
        aria-label="Search icon"
    >
        <path 
            stroke-linecap="round" 
            stroke-linejoin="round" 
            :stroke-width="strokeWidth" 
            d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
        />
    </svg>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
    size: {
        type: String,
        default: 'md',
        validator: (value) => ['xs', 'sm', 'md', 'lg', 'xl', 'xxl', 'xxxl'].includes(value)
    },
    strokeWidth: {
        type: [String, Number],
        default: 2
    },
    customClass: {
        type: String,
        default: ''
    }
});

const iconClass = computed(() => {
    const sizeClasses = {
        xs: 'w-3 h-3',
        sm: 'w-4 h-4',
        md: 'w-5 h-5',
        lg: 'w-6 h-6',
        xl: 'w-8 h-8',
        xxl: 'w-10 h-10',
        xxxl: 'w-12 h-12'
    };
    
    return `${sizeClasses[props.size]} ${props.customClass}`.trim();
});
</script>

<style scoped>
/* Icon styles */
svg {
    transition: all 0.2s ease;
}
</style>