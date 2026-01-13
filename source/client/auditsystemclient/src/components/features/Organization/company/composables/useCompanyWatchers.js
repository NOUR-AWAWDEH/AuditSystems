import { watch } from 'vue';

export function useCompanyWatchers({
  searchQuery,
  currentPage,
  pageSize,
  searchCompanys,
  clearSearch,
  getCompanysByUserId
}) {
  // Watch search query
  watch(searchQuery, async (search) => {
    if (search.trim()) {
      if (searchCompanys) {
        await searchCompanys({
          searchTerm: search,
          page: currentPage.value,
          pageSize: pageSize.value,
        });
      }
    } else {
      if (clearSearch) clearSearch();
      if (getCompanysByUserId) {
        await getCompanysByUserId();
      }
    }
  });

  // Watch pagination
  watch([currentPage, pageSize], async () => {
    if (searchQuery.value.trim()) {
      if (searchCompanys) {
        await searchCompanys({
          searchTerm: searchQuery.value,
          page: currentPage.value,
          pageSize: pageSize.value,
        });
      }
    } else {
      if (getCompanysByUserId) {
        await getCompanysByUserId();
      }
    }
  });
}