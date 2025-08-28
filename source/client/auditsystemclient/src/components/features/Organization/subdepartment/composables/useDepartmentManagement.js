import { ref, computed, watch } from 'vue';
import { useGetDepartmentById } from '@/composables/organization/department/useGetDepartmentById';

export function useDepartmentManagement(departments) {
    // Initialize from localStorage
    const selectedDepartmentId = ref(localStorage.getItem('organizationSelectedDepartmentId') || '');

    //Watch for changes and persist to localStorage
    watch(selectedDepartmentId, (newValue) => {
        if (newValue) {
            localStorage.setItem('organizationSelectedDepartmentId', newValue);
        } else {
            localStorage.removeItem('organizationSelectedDepartmentId');
        }
    });

    const {
        department: fetchDepartment,
        isLoading: isDepartmentLoading,
        getDepartmentById,
        clearDepartment,
    } = useGetDepartmentById();

    // Add watcher to fetch Department details when selectedDepartmentId changes
    watch(selectedDepartmentId, async (newDepartmentId) => {
        if (newDepartmentId) {
            try {
                await getDepartmentById(newDepartmentId);
            } catch (error) {
                console.error('Error fetching Department details:', error);
            }
        } else {
            clearDepartment();
        }
    }, { immediate: true });

    const departmentsRef = computed(() => {
        if (departments && typeof departments === 'object' && 'value' in departments) {
            return departments.value || [];
        }
        return departments || [];
    });

    const selectedDepartment = computed(() => {
        const departmentId = selectedDepartmentId.value;
        const departmentsList = departmentsRef.value;

        if (!departmentId || !departmentsList || departmentsList.length === 0) {
            return null;
        }
        const department = departmentsList.find(loc => loc.id === departmentId);
        return department || null;
    });

    const currentDepartment = computed(() => {
        const department = fetchDepartment?.value || selectedDepartment?.value;
        return department?.departmentModel || department;
    });

    return {
        selectedDepartmentId,
        selectedDepartment,
        currentDepartment,
        isDepartmentLoading,
        departmentsRef,
        getDepartmentById,
        clearDepartment,
    };
}