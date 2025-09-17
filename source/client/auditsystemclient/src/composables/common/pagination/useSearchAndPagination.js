import { ref } from 'vue';

export function useSearchAndPagination() {
    const searchQuery = ref('');
    const currentPage = ref(1);
    const pageSize = ref(10);

    const handleSearch = async (newSearchQuery) => {
        searchQuery.value = newSearchQuery;
        currentPage.value = 1;
    };

    const resetPagination = () => {
        currentPage.value = 1;
        searchQuery.value = '';
    };

    return {
        searchQuery,
        currentPage,
        pageSize,
        handleSearch,
        resetPagination
    };
}