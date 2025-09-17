import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const OBJECTIVE_BASE_ENDPOINT = '/api/Objective';
const SEARCH_OBJECTIVES_BY_USER_ID_ENDPOINT = '/search-objectives-by-user-id';

export const searchObjectivesByUserIdRepository = {
    async searchObjectivesByUserId(searchTerm = '', pageNumber = 1, pageSize = 10, riskControlMatrixId = '') {
        try {
            const token = authService.getLocalToken();

            const params = {
                searchTerm: searchTerm,
                riskControlMatrixId: riskControlMatrixId,
                page: pageNumber,
                pageSize: pageSize
            };

            const response = await api.get(
                `${OBJECTIVE_BASE_ENDPOINT}${SEARCH_OBJECTIVES_BY_USER_ID_ENDPOINT}`,
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
                        data.successMessage || 'Objectives fetched successfully.'
                    );
                }


                return ApiResponseT.badRequest(
                    [{ message: 'Missing or invalid result structure.' }],
                    'Invalid response format'
                );
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format.' }],
                'Failed to search Objectives.'
            );

        } catch (error) {
            console.error('Search Objectives error:', error);


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
                [{ message: error.message || 'An unexpected error occurred while searching Objectives.' }],
                'An unexpected error occurred while searching Objectives.'
            );
        }
    }
};
