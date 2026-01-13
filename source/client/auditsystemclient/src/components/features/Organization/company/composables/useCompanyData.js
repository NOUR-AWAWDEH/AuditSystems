import { ref } from 'vue';
import { useSearchCompanysByUserId } from '@/composables/organization/company/useSearchCompanysByUserId';
import { useGetCompanysByUserId } from '@/composables/organization/company/useGetCompanysByUserId';

export function useCompanyData() {
  const {
    searchResults = ref([]),
    isSearching = ref(false),
    totalCount = ref(0),
    searchCompanys,
    clearSearch
  } = useSearchCompanysByUserId() || {};

  const {
    companies: allCompanies = ref([]),
    isLoading: isLoadingAll = ref(false),
    getCompanysByUserId
  } = useGetCompanysByUserId() || {};

  return {
    searchResults,
    allCompanies,
    isSearching,
    isLoadingAll,
    totalCount,
    searchCompanys,
    clearSearch,
    getCompanysByUserId
  };
}
