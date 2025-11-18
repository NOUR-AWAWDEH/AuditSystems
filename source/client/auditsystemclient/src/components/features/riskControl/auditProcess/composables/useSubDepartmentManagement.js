import { ref, computed, watch } from "vue";
import { useGetSubDepartmentById } from "@/composables/organization/subdepartment/useGetSubDepartmentById";

export function useSubDepartmentManagement(subDepartments) {
  const selectedSubDepartmentId = ref(
    localStorage.getItem("organizationSelectedSubDepartmentId") || ""
  );

  watch(selectedSubDepartmentId, (newVal) => {
    if (newVal) localStorage.setItem("organizationSelectedSubDepartmentId", newVal);
    else localStorage.removeItem("organizationSelectedSubDepartmentId");
  });

  const {
    subDepartment: fetchSubDepartment,
    isLoading: isSubDepartmentLoading,
    getSubDepartmentById,
    clearSubDepartment,
  } = useGetSubDepartmentById();

  watch(
    selectedSubDepartmentId,
    async (id) => {
      if (id) await getSubDepartmentById(id);
      else clearSubDepartment();
    },
    { immediate: true }
  );

  const subDepartmentsRef = ref(subDepartments || []);

  const selectedSubDepartment = computed(() => {
    if (!selectedSubDepartmentId.value) return null;
    return subDepartmentsRef.value.find((s) => s.id === selectedSubDepartmentId.value) || null;
  });

  const currentSubDepartment = computed(
    () => fetchSubDepartment.value || selectedSubDepartment.value
  );

  return {
    selectedSubDepartmentId,
    selectedSubDepartment,
    currentSubDepartment,
    isSubDepartmentLoading,
    subDepartmentsRef,
    getSubDepartmentById,
    clearSubDepartment,
  };
}
