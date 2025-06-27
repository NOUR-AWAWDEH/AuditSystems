import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const SUBLOCATION_API_BASE_URL = '/api/SubLocation';
export const searchSubLocationRepository = {
    async searchSubLocations(searchParams = {}) {
        try {
            const token = authService.getLocationToken();
            
            if (!token) {
                throw new Error('Authentication token not found');
            }

            const queryParams = new URLSearchParams();

            // Add search parameters
            if (searchParams.searchTerm) {
                queryParams.append('searchTerm', searchParams.searchTerm);
            }
            
            // Pagination parameters
            if (searchParams.page) {
                queryParams.append('page', searchParams.page);
            }
            if (searchParams.pageSize) {
                queryParams.append('pageSize', searchParams.pageSize);
            }
            
            // Sorting parameters
            if (searchParams.sortBy) {
                queryParams.append('sortBy', searchParams.sortBy);
            }
            if (searchParams.sortDirection) {
                queryParams.append('sortDirection', searchParams.sortDirection);
            }

            const queryString = queryParams.toString();
            const url = `${SUBLOCATION_API_BASE_URL}/search-sub-location${queryString ? `?${queryString}` : ''}`;
            
            const response = await api.get(url, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            return ApiResponseT.success(response.data);
            
        } catch (error) {
            console.error('Error searching sublocations:', error);
            
            if (error.response?.status === 401) {
                return ApiResponseT.error('Unauthorized: Please login again');
            }
            
            return ApiResponseT.error(
                error.response?.data?.message || 
                error.message || 
                'Failed to search sublocations'
            );
        }
    }
};
