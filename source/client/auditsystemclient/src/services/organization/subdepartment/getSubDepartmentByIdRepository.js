import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBDEPARTMENT_BASE_ENDPOINT = 'api/SubDepartment';
const SUBDEPARTMENT_GET_BY_ID_ENDPOINT = '/get-sub-department-by-id';

export const getSubDepartmentByIdRepository = {
    async getSubDepartmentById(subDepartmentId) {
        try {
            const token = authService.getLocalToken();
            
            const response = await api.get(
                `${SUBDEPARTMENT_BASE_ENDPOINT}${SUBDEPARTMENT_GET_BY_ID_ENDPOINT}`,
                {
                    params: { 
                        id: subDepartmentId
                    },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );
    
            // Fix: Access response.data and response.status correctly
            const { data, status } = response;
    
            if (status === 200 && data?.success && data?.result) {
                return ApiResponseT.ok(
                    data.result, 
                    data.successMessage || 'SubDepartment retrieved successfully'
                );
            }
    
            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.message || 'Failed to retrieve sub-department.' }],
                data?.message || 'Failed to retrieve sub-department.'
            );
    
        } catch (error) {
            console.error('Error fetching subdepartment:', error);
            
            const responseData = error.response?.data;
    
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }
            
            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'SubDepartment not found' }],
                    'SubDepartment not found'
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
            
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving subdepartment.' }],
                'Server error'
            );
        }
    }
};
