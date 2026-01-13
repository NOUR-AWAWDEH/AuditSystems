import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const COMPANY_BASE_ENDPOINT = '/api/company';
const CREATE_COMPANY_ENDPOINT = '/create-company';

export const createCompanyRepository = {
    async createCompany(companyData) {
        try {
            const token = authService.getLocalToken();

            const payload = {
                name: companyData.name,
            };

            const response = await api.post(
                `${COMPANY_BASE_ENDPOINT}${CREATE_COMPANY_ENDPOINT}`,
                payload, {

                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 201 || response?.success !== false) {
                return ApiResponseT.ok(response.data.result, response.successMessage || 'Company created successfully');
            }

            const errors = Array.isArray(response?.errors)
                ? response.errors
                : [{ message: response?.data?.successMessage || 'Creation failed' }];
            return ApiResponseT.badRequest(errors, response?.data?.successMessage);
       
        } catch (error) {
            
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = error.response?.errors ||
                    [{ message: error.response?.data?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while creating company.' }],
                'Server error'
            );
        }
    }
};