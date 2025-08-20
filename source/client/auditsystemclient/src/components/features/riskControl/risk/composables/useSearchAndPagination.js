import { ref } from 'vue';

/**
 * Composable for handling search input and pagination state in a reusable way.
 *
 * @returns {Object} Exposes reactive state and helper functions.
 * @property {Ref<string>} searchQuery - Current search term used for filtering.
 * @property {Ref<number>} currentPage - Current active page for pagination.
 * @property {Ref<number>} pageSize - Number of items per page (default: 10).
 *
 * @property {Function} handleSearch - Updates search term and resets page to 1.
 * @param {string} newSearchQuery - New search string entered by user.
 *
 * @property {Function} resetPagination - Clears search and resets page to 1.
 */

export function useSearchAndPagination() {
    const searchQuery = ref('');
    const currentPage = ref(1);
    const pageSize = ref(10);

    /**
     * Update the search query and reset pagination to the first page.
     * @param {string} newSearchQuery - New search input string.
     */
    const handleSearch = async (newSearchQuery) => {
        searchQuery.value = newSearchQuery;
        currentPage.value = 1;
    };

    /**
     * Reset search and pagination back to default state.
     */
    const resetPagination = () => {
        searchQuery.value = '';
        currentPage.value = 1;
    };

    return {
        searchQuery,
        currentPage,
        pageSize,
        handleSearch,
        resetPagination
    };
}