import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const RECOMMENDATION_BASE_ENDPOINT = '/api/Recommendation';
const SEARCH_RECOMMENDATIONS_BY_USER_ID_ENDPOINT = '/search-recommendations-by-user-id';

export const searchRecommendationsByUserIdRepository = {
    async searchRecommendationsByUserId(searchTerm = '', pageNumber = 1, pageSize = 10, observationId = '') {
        try {
            const token = authService.getLocalToken();
            const searchTermParam = searchTerm?.trim() || '';
            const params = {
                searchTerm: searchTermParam,
                observationId: observationId,
                pageNumber: pageNumber,
                pageSize: pageSize
            };

            const response = await api.get(
                `${RECOMMENDATION_BASE_ENDPOINT}${SEARCH_RECOMMENDATIONS_BY_USER_ID_ENDPOINT}`,
                {
                    params,
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            const { data, status } = response;

            if (status === 200 && data?.success) {
                const result = data.result;

                if (Array.isArray(result?.items) && typeof result.totalCount === 'number') {
                    const safeTotalCount = result.totalCount === 0 && result.items.length > 0
                        ? result.items.length
                        : result.totalCount;

                    return ApiResponseT.ok(
                        { ...result, totalCount: safeTotalCount },
                        data.successMessage || 'Recommendations fetched successfully.'
                    );
                }


                return ApiResponseT.badRequest(
                    [{ message: 'Missing or invalid result structure.' }],
                    'Invalid response format'
                );
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format.' }],
                'Failed to search Recommendations.'
            );

        } catch (error) {
            console.error('Search Recommendations error:', error);


            const responseData = error.response?.data;

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Unauthorized'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors || [{ message: responseData?.message || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while searching Recommendations.' }],
                'An unexpected error occurred while searching Recommendations.'
            );
        }
    }
};