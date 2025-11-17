<template>
    <!-- NoAuditProgramSelected.vue -->
    <div class="text-center py-20">
        <!-- When audit programs are available but none selected -->
        <div v-if="auditPrograms && auditPrograms.length > 0" class="max-w-md mx-auto">
            <div
                class="bg-linear-to-br from-blue-50 to-indigo-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg">
                <AuditProgramIcon size="xxxl" custom-class="text-blue-600" />
            </div>
            <h3 class="text-2xl font-bold text-gray-800 mb-4">
                Select an Audit Program
            </h3>
            <p class="text-gray-600 mb-6 leading-relaxed">
                Choose an audit program from the dropdown above to view and manage
                observations
            </p>
            <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
                <p class="text-sm text-blue-700">
                    <span class="font-medium">💡 Tip:</span>
                    Use the dropdown menu above to select an audit program and start
                    managing observations
                </p>
            </div>
        </div>

        <!-- When no audit programs are available -->
        <div v-else class="max-w-lg mx-auto">
            <div
                class="bg-linear-to-br from-gray-50 to-gray-100 rounded-full w-32 h-32 mx-auto mb-8 flex items-center justify-center shadow-lg border-2 border-dashed border-gray-300">
                <AuditProgramIcon size="xxxl" custom-class="text-gray-400" />
            </div>
            <h3 class="text-2xl font-bold text-gray-800 mb-4">
                No Observations Available
            </h3>
            <p class="text-gray-600 mb-6 leading-relaxed">
                You need to create observations first before you can manage them
            </p>
        </div>
    </div>
</template>

<script setup>
import AuditProgramIcon from '@/components/common/icons/AuditProgramIcon.vue'

defineProps({
    auditPrograms: {
        type: Array,
        default: () => []
    }
})
</script>
