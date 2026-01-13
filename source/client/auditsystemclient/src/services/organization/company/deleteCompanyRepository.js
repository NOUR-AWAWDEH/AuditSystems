import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const COMPANY_BASE_ENDPOINT = '/api/company';
const DELETE_COMPANY_ENDPOINT = '/delete-company';

export const deleteCompanyByIdRepository = {
    async deleteCompanyById(companyId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.delete(
                `${COMPANY_BASE_ENDPOINT}${DELETE_COMPANY_ENDPOINT}`, {

                data: { id: companyId },
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 204 || response?.success !== false) {
                return ApiResponseT.ok(null, 'Company deleted successfully');
            }

            const errors = Array.isArray(response.errors)
                ? response.errors
                : [{ message: response.successMessage || 'Delete failed' }];
            return ApiResponseT.badRequest(errors, response.successMessage);

        } catch (error) {
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                // Extract server error message properly
                const responseData = error.response?.data;
                const serverMessage = responseData?.message || responseData?.title || responseData?.detail || 'Bad request';
                const errors = responseData?.errors || [{ message: serverMessage }];
                return ApiResponseT.badRequest(errors, serverMessage);
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Company not found' }], 'Company not found'
                );
            }

            // Handle other server errors
            const responseData = error.response?.data;
            const serverMessage = responseData?.message || responseData?.title || error.message || 'An unexpected error occurred while deleting company.';
            
            return ApiResponseT.serverError(
                [{ message: serverMessage }],
                'Server error'
            );
        }
    }
};