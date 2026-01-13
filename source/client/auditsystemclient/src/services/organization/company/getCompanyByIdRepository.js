import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const COMPANY_BASE_ENDPOINT = '/api/company';
const GET_COMPANY_BY_ID_ENDPOINT = '/get-company';

export const getCompanyByIdRepository = {
    async getCompanyById(companyId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${COMPANY_BASE_ENDPOINT}${GET_COMPANY_BY_ID_ENDPOINT}`, {
                params: { id: companyId },
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            // Fix: Access response.data and response.status correctly
            const { data, status } = response;

            if (status === 200 && data?.success && data?.result) {
                return ApiResponseT.ok(
                    data.result, 
                    data.successMessage || 'Company retrieved successfully.'
                );
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.message || 'Failed to retrieve company.' }],
                data?.message || 'Failed to retrieve company.'
            );

        } catch (error) {
            console.error('Error fetching company:', error);
            
            const responseData = error.response?.data;

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors || 
                    [{ message: responseData?.message || 'Bad request' }];
                return ApiResponseT.badRequest(
                    errors,
                    responseData?.message || 'Bad request'
                );
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Company not found.' }],
                    'Company not found.'
                );
            }
            
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving company.' }],
                'Server error'
            );
        }
    }
};