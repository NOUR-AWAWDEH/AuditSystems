import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const SUBLOCATION_API_BASE_URL = '/api/SubLocation';

export const searchSubLocationRepository = {
    async searchSubLocations( searchParams = {}) {
        try {
            const token = authService.getLocationToken();

            const queryParams = new URLSearchParams();

            if (searchParams.searchTerm) {
                queryParams.append('searchTerm', searchParams.searchTerm);
            }
            if (searchParams.locationId) {
                queryParams.append('locationId', searchParams.locationId);
            }

            if (searchParams.pageNumber) {
                queryParams.append('pageNumber', searchParams.pageNumber);
            }
            if (searchParams.pageSize) {
                queryParams.append('pageSize', searchParams.pageSize);
            }

            if (searchParams.sortBy) {
                queryParams.append('sortBy', searchParams.sortBy);
            }
            if (searchParams.sortBy) {
                queryParams.append('sortBy', searchParams.sortBy);
            }
            if (searchParams.sortDirection) {
                queryParams.append('sortDirection', searchParams.sortDirection);
            }

            const queryString = queryParams.toString();
            const url = `${SUBLOCATION_BASE_ENDPOINT}/search-sub-location/${searchParams.locationId}${queryString ? `?${queryString}` : ''}`;
            
            const response = await api.get(url, {
                headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
                }
            });
        }catch(error)
        {

        }
    }
}
