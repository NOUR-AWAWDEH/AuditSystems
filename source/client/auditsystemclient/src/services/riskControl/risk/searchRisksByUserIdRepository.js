import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const RISK_BASE_ENDPOINT = '/api/Risk';
const SEARCH_RISKS_BY_USER_ID_ENDPOINT = '/search-risks-by-user-id';

export const searchRisksByUserIdRepository = {
    async searchRisksByUserId(searchTerm = '', pageNumber = 1, pageSize = 10, objectiveId = '') {
        try {
            const token = authService.getLocalToken();

            const params = {
                searchTerm: searchTerm,
                objectiveId: objectiveId,
                page: pageNumber,
                pageSize: pageSize
            };

            const response = await api.get(
                `${RISK_BASE_ENDPOINT}${SEARCH_RISKS_BY_USER_ID_ENDPOINT}`,
                {
                    params,
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            console.log("SearchRisksByUserIdRepository: ", response);
            const { data, status } = response;

            if (status === 200 && data?.success) {
                const result = data.result;

                if (Array.isArray(result?.items) && typeof result.totalCount === 'number') {
                    const safeTotalCount = result.totalCount === 0 && result.items.length > 0
                        ? result.items.length
                        : result.totalCount;

                    return ApiResponseT.ok(
                        { ...result, totalCount: safeTotalCount },
                        data.successMessage || 'Risks fetched successfully.'
                    );
                }


                return ApiResponseT.badRequest(
                    [{ message: 'Missing or invalid result structure.' }],
                    'Invalid response format'
                );
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format.' }],
                'Failed to search Risks.'
            );

        } catch (error) {
            console.error('Search Risks error:', error);


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
                [{ message: error.message || 'An unexpected error occurred while searching Risks.' }],
                'An unexpected error occurred while searching Risks.'
            );
        }
    }
};