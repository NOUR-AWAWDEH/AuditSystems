import { ref, computed, watch } from 'vue';
import { useGetAuditProgramById } from '@/composables/riskControl/auditProgram/useGetAuditProgramById';

export function useAuditProgramManagement(auditPrograms) {
    // Initialize from localStorage - USE THE SAME KEY AS RiskControl.vue
    const selectedAuditProgramId = ref(localStorage.getItem('riskControlSelectedAuditProgramId') || '');

    // Watch for changes and persist to localStorage
    watch(selectedAuditProgramId, (newValue) => {
        if (newValue) {
            localStorage.setItem('riskControlSelectedAuditProgramId', newValue);
        } else {
            localStorage.removeItem('riskControlSelectedAuditProgramId');
        }
    });

    const {
        auditProgram: fetchAuditProgram,
        isLoading: isAuditProgramLoading,
        getAuditProgramById,
        clearAuditProgram
    } = useGetAuditProgramById();

    // Add watcher to fetch AuditProgram details when selectedAuditProgramId changes
    watch(selectedAuditProgramId, async (newAuditProgramId) => {
        if (newAuditProgramId) {
            try {
                await getAuditProgramById(newAuditProgramId);
            } catch (error) {
                console.error('Error fetching AuditProgram details:', error);
            }
        } else {
            clearAuditProgram();
        }
    }, { immediate: true });

    const auditProgramsRef = computed(() => {
        if (auditPrograms && typeof auditPrograms === 'object' && 'value' in auditPrograms) {
            return auditPrograms.value || [];
        }
        return auditPrograms || [];
    });

    const selectedAuditProgram = computed(() => {
        const auditProgramId = selectedAuditProgramId.value;
        const auditProgramsList = auditProgramsRef.value;

        if (!auditProgramId || !auditProgramsList || auditProgramsList.length === 0) {
            return null;
        }
        const auditProgram = auditProgramsList.find(auditProgram => auditProgram.id === auditProgramId);
        return auditProgram || null;
    });

    const currentAuditProgram = computed(() => {
        const auditProgram = fetchAuditProgram?.value || selectedAuditProgram?.value;
        return auditProgram?.auditProgramModel || auditProgram;
    });

    return {
        selectedAuditProgramId,
        selectedAuditProgram,
        currentAuditProgram,
        isAuditProgramLoading,
        auditProgramsRef,
        getAuditProgramById,
        clearAuditProgram
    };
}