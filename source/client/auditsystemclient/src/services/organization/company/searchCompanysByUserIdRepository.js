import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const COMPANY_BASE_ENDPOINT = '/api/company';
const SEARCH_COMPANY_BY_USER_ID_ENDPOINT = '/search-company';

export const searchCompanysByUserIdRepository = {

    async searchCompanysByUserId(searchParams = {}) {
        try {
            const token = authService.getLocalToken();

            if (!token) {
                throw new Error('Authentication token not found');
            }

            const response = await api.get(`${COMPANY_BASE_ENDPOINT}${SEARCH_COMPANY_BY_USER_ID_ENDPOINT}`, {
                params: {
                    SearchTerm: searchParams.searchTerm,
                    PageNumber: searchParams.page,
                    PageSize: searchParams.pageSize
                },
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            const { data, status } = response;
            
            if (status === 200 && data?.success) {
                return ApiResponseT.ok(
                    data.result, 
                    data.successMessage || 'Companies retrieved successfully.'
                );
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: 'Failed to retrieve companies.' }],
                data?.message || 'Failed to retrieve companies.'
            );

        } catch (error) {
            console.error("Error searching company:", error);
            
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