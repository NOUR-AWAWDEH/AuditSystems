import { ref } from 'vue';
import { useSearchDepartmentsByUserId } from '@/composables/organization/department/useSearchDepartmentsByUserId';
import { useGetDepartmentsByUserId } from '@/composables/organization/department/useGetDepartmentsByUserId';

export function useDepartmentData() {
  const {
    searchResults = ref([]),
    isSearching = ref(false),
    totalCount = ref(0),
    searchDepartments,
    clearSearch
  } = useSearchDepartmentsByUserId() || {};

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
