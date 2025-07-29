import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_BASE_ENDPOINT = 'api/SubLocation';
const SUBLOCATION_GET_BY_ID_ENDPOINT = '/get-sub-location-by-id';

export const getSubLocationByIdRepository = {
    async getSubLocationById(subLocationId) {
        try {
            const token = authService.getLocalToken();
            
            const response = await api.get(
                `${SUBLOCATION_BASE_ENDPOINT}${SUBLOCATION_GET_BY_ID_ENDPOINT}`,
                {
                    params: { id: subLocationId },
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
                    data.successMessage || 'SubLocation retrieved successfully'
                );
            }
            
            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.message || 'Failed to retrieve sublocation' }],
                data?.message || 'Failed to retrieve sublocation'
            );
            
        } catch (error) {
            console.error('Error fetching sublocation:', error);
            
            const responseData = error.response?.data;
            
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }
            
            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'SubLocation not found' }],
                    'SubLocation not found'
                );
            }
            
            if (error.response?.status === 400) {
                const errors = responseData?.errors || 
                    [{ message: responseData?.message || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
            }
            
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving sublocation.' }],
                'Server error'
            );
        }
    }
};