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


            const queryString = queryParams.toString();
            const url = `${DEPARTMENT_BASE_ENDPOINT}${SEARCH_DEPARTMENT_BY_USER_ID_ENDPOINT}${queryString ? `?${queryString}` : ''}`;
            
            const response = await api.get(url, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            // Fix: Access response.data and response.status correctly
            const { data, status } = response;
            
            if (status === 200 && data?.success) {
                return ApiResponseT.ok(
                    data.result, 
                    data.successMessage || 'Departments retrieved successfully.'
                );
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: 'Failed to retrieve departments.' }],
                data?.message || 'Failed to retrieve departments.'
            );

        } catch (error) {
            console.error("Error searching department:", error);
            
            const responseData = error.response?.data;
            
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }
            
            if (error.response?.status === 400) {
                const errors = responseData?.errors || [{ message: responseData?.message || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
            }
            
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred.' }],
                'Server error'
            );
        }
    }
};