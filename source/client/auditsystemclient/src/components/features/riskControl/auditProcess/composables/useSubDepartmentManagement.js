import { ref, computed, watch } from 'vue';
import { useGetSubDepartmentById } from '@/composables/organization/subdepartment/useGetSubDepartmentById';

export function useSubDepartmentManagement(subDepartments) {
    // Initialize from localStorage
    const selectedSubDepartmentId = ref(localStorage.getItem('selectedSubDepartmentId') || '');

    // Watch for changes and persist to localStorage
    watch(selectedSubDepartmentId, (newValue) => {
        if (newValue) {
            localStorage.setItem('selectedSubDepartmentId', newValue);
        } else {
            localStorage.removeItem('selectedSubDepartmentId');
        }
    });

    const {
        subDepartment: fetchSubDepartment,
        isLoading: isSubDepartmentLoading,
        fetchSubDepartmentById,
        clearSubDepartment
    } = useGetSubDepartmentById();

    const subDepartmentsRef = computed(() => {
        if (subDepartments && typeof subDepartments === 'object' && 'value' in subDepartments) {
            return subDepartments.value || [];
        }
        return subDepartments || [];
    });

    const selectedSubDepartment = computed(() => {
        const subDepartmentId = selectedSubDepartmentId.value;
        const subDepartmentsList = subDepartmentsRef.value;

        if (!subDepartmentId || !subDepartmentsList || subDepartmentsList.length === 0) {
            return null;
        }
        const subDepartment = subDepartmentsList.find(sub => sub.id === subDepartmentId);
        return subDepartment || null;
    });

    const currentSubDepartment = computed(() => {
        const subDepartment = fetchSubDepartment?.value || selectedSubDepartment?.value;
        return subDepartment?.subDepartmentModel || subDepartment;
    });

    return {
        selectedSubDepartmentId,
        selectedSubDepartment,
        currentSubDepartment,
        isSubDepartmentLoading,
        subDepartmentsRef,
        fetchSubDepartmentById,
        clearSubDepartment
    };
}