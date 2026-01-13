<template>
    <!--LocationHeader.vue-->
    <div class="flex items-center">
        <div>
            <h2 class="text-xl sm:text-2xl font-bold text-secondary-base">
                Locations
                <span v-if="selectedCompanyName" class="text-primary-base ml-2">
                    (<span class="text-gray-400 font-normal text-lg">Company: </span>{{ selectedCompanyName }})
                </span>
            </h2>
            <p class="text-sm text-gray-500 mt-1">Manage your organization locations</p>
        </div>
    </div>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
    companies: {
        type: Array,
        default: () => []
    },
    selectedCompanyId: {
        type: [String, Number],
        default: null
    }
});

const selectedCompanyName = computed(() => {
    if (!props.selectedCompanyId || !props.companies) return null;
    const company = props.companies.find(c => c.id === props.selectedCompanyId);
    return company ? company.name : null;
});
</script>
