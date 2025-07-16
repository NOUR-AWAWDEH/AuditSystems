import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const DEPARTMENT_BASE_ENDPOINT = '/api/department';
const SEARCH_DEPARTMENT_BY_USER_ID_ENDPOINT = '/search-department-by-user-id';

export const searchDepartmentByUserIdRepository = {

    async searchDepartmentByUserId(searchParams = {}) {
        try {
            const token = authService.getLocalToken();

            if (!token) {
                throw new Error('Authentication token not found');
            }

            const queryParams = new URLSearchParams();

            if (searchParams.searchTerm) {
                queryParams.append('searchTerm', searchParams.searchTerm);
            }

            if (searchParams.subLocationId) {
                queryParams.append('subLocationId', searchParams.subLocationId);
            }

            if (searchParams.page) {
                queryParams.append('page', searchParams.page);
            }

            if (searchParams.pageSize) {
                queryParams.append('pageSize', searchParams.pageSize);
            }

            if (searchParams.sortBy) {
                queryParams.append('sortBy', searchParams.sortBy);
            }

            if (searchParams.sortDirection) {
                queryParams.append('sortDirection', searchParams.sortDirection);
            }

            const queryString = queryParams.toString();
            const url = `${DEPARTMENT_BASE_ENDPOINT}${SEARCH_DEPARTMENT_BY_USER_ID_ENDPOINT}${queryString ? `?${queryString}` : ''}`;
            const response = await api.get(url, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.data && response.status === 200) {
                return ApiResponseT.ok(response.data.result, response.data.message || 'Departments retrived successfully.');
            }

            return ApiResponseT.badRequest(
                response.data?.errors || [{ message: 'Failed to retrieve department.' }],
                response.data?.message || 'Failed to retrieve departments.'
            );

        } catch (error) {
            console.error("Error searching department:", error);
            
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }
            
            return ApiResponseT.internalServerError(
                error.response?.data?.message || error.message || "Failed to retrieve departments."
            );
        }
    }
};