import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const COMPANY_BASE_ENDPOINT = '/api/company';
const UPDATE_COMPANY_BY_ID_ENDPOINT = '/update-company';

export const updateCompanyByIdRepository = {
    async updateCompanyById(companyData) {
        try {
            const token = authService.getLocalToken();

            if (!token) {
                throw new Error('Authentication token not found');
            }

            const payload = {
                id: companyData.id,
                name: companyData.name
            };

            const response = await api.put(
                `${COMPANY_BASE_ENDPOINT}${UPDATE_COMPANY_BY_ID_ENDPOINT}`,
                payload, 
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            if (response.status === 200 || response?.success !== false) {
              return ApiResponseT.ok(null, response?.data?.successMessage || 'Company updated successfully');
            }

          const errors = Array.isArray(response?.errors)
            ? response?.errors
            : [{ message: response?.data?.successMessage || 'Update failed' }];
        
          return ApiResponseT.badRequest(errors, response?.data?.successMessage);

    } catch (error) {
      
      if (error.response?.status === 401) {
        console.error('Unauthorized - token may be invalid');
        return ApiResponseT.unauthorized(
          [{ message: 'Authentication required. Please log in again.' }],
          'Authentication required'
        );
      }
      
      if (error.response?.status === 400) {
        const errors = error.response?.errors || [{ message: error.response?.data?.successMessage || 'Bad request' }];
        return ApiResponseT.badRequest(errors, error.response?.data?.successMessage || 'Bad request');
      }
      
      if (error.response?.status === 404) {
        return ApiResponseT.notFound(
          [{ message: 'Company not found' }],
          'Company not found'
        );
      }
      
      return ApiResponseT.serverError(
        [{ message: error.message || 'An unexpected error occurred while updating company.' }],
        'Server error'
      );
    }
  }
};