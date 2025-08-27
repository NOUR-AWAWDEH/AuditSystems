import { ref } from 'vue';
import { useSearchDepartmentByUserId } from '@/composables/organization/department/useSearchDepartmentByUserId';
import { useGetDepartmentsByUserId } from '@/composables/organization/department/useGetDepartmentsByUserId';

export function useDepartmentData() {
  const {
    searchResults = ref([]),
    isSearching = ref(false),
    totalCount = ref(0),
    searchDepartments,
    clearSearch
  } = useSearchDepartmentByUserId() || {};

  const {
    departments: allDepartments = ref([]),
    isLoading: isLoadingAll = ref(false),
    getDepartmentsByUserId
  } = useGetDepartmentsByUserId() || {};

  return {
    searchResults,
    allDepartments,
    isSearching,
    isLoadingAll,
    totalCount,
    searchDepartments,
    clearSearch,
    getDepartmentsByUserId
  };
}
