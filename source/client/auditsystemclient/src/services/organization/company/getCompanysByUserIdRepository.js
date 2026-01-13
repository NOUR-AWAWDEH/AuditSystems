import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const COMPANY_BASE_ENDPOINT = '/api/company';
const SEARCH_COMPANY_ENDPOINT = '/search-company';

export const getCompanyByUserIdRepository = {
    async getCompanyByUserId() {
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${COMPANY_BASE_ENDPOINT}${SEARCH_COMPANY_ENDPOINT}`, {

                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.data?.success && response.data?.result) {
                // If it's returning a pagination response, we extract items
                const result = response.data.result;
                const companies = result.items || result.Items || result.companys || result.companies || result;
                return ApiResponseT.ok(companies, response.data.successMessage || 'Company retrieved successfully.');
            }

            return ApiResponseT.badRequest(
                response.data?.errors || [{ message: 'Failed to retrieve company.' }],
                response.data?.successMessage || 'Failed to retrieve company.'
            );
            
        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response?.data?.errors ||
                    [{
                        message: error.response?.data?.successMessage ||
                            'Bad request'
                    }];
                return ApiResponseT.badRequest(
                    errors,
                    error.response?.data?.successMessage || 'Bad request'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving company.' }],
                'Server error'
            );
        }
    }
};