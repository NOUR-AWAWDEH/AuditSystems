import { ref } from 'vue';
import { useSearchSubDepartmentsByUserId } from '@/composables/organization/subdepartment/useSearchSubDepartmentsByUserId';
import { useGetSubDepartmentsByUserId } from '@/composables/organization/subdepartment/useGetSubDepartmentsByUserId';

export function useSubDepartmentData() {
  const {
    searchResults = ref([]),
    isSearching = ref(false),
    totalCount = ref(0),
    searchSubDepartments,
    clearSearch
  } = useSearchSubDepartmentsByUserId() || {};

  const {
    subDepartments: allSubDepartments = ref([]),
    isLoading: isLoadingAll = ref(false),
    getSubDepartmentsByUserId
  } = useGetSubDepartmentsByUserId() || {};

  return {
    searchResults,
    allSubDepartments,
    isSearching,
    isLoadingAll,
    totalCount,
    searchSubDepartments,
    clearSearch,
    getSubDepartmentsByUserId
  };
}
