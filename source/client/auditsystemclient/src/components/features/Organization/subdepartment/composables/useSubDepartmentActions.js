export function useSubDepartmentActions({
    searchSubDepartments,
    getSubDepartmentsByUserId,
    clearSearch,
    clearDepartment,
    allSubDepartments,
    searchQuery,
    selectedDepartmentId,
    currentPage,
    pageSize
}) {
    const refreshData = async () => {
        const hasSearchOrFilter = searchQuery.value.trim() || selectedDepartmentId.value;
        if (hasSearchOrFilter) {
            if (searchSubDepartments) {
                await searchSubDepartments({
                    searchTerm: searchQuery.value || '',
                    departmentId: selectedDepartmentId.value,
                    page: currentPage.value,
                    pageSize: pageSize.value,
                });
            }
        } else {
            if (getSubDepartmentsByUserId) {
                await getSubDepartmentsByUserId();
            }
        }
    };

    const handleSubDepartmentChange = async (newDepartmentId) => {
        if (clearSearch) clearSearch();
        if (allSubDepartments?.value !== undefined) allSubDepartments.value = [];
        selectedDepartmentId.value = newDepartmentId;
    };

    const resetToDefaultView = () => {
        selectedDepartmentId.value = null;
        searchQuery.value = '';
        currentPage.value = 1;
        if (clearSearch) clearSearch();
        if (allSubDepartments?.value !== undefined) allSubDepartments.value = [];
        if (clearDepartment) clearDepartment();
    };

    return { refreshData, handleSubDepartmentChange, resetToDefaultView };
}