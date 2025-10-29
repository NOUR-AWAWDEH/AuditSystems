import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const EXIT_CONTROL_BASE_ENDPOINT = '/api/ExitControl';
const SEARCH_EXIT_CONTROLS_BY_USER_ID_ENDPOINT = '/search-exit-controls-by-user-id';

export const searchExitControlsByUserIdRepository = {
    async searchExitControlsByUserId(searchTerm = '', pageNumber = 1, pageSize = 10, objectiveId = '') {
        try {
            const token = authService.getLocalToken();

            const params = {
                searchTerm: searchTerm,
                objectiveId: objectiveId,
                page: pageNumber,
                pageSize: pageSize
            };

            const response = await api.get(
                `${EXIT_CONTROL_BASE_ENDPOINT}${SEARCH_EXIT_CONTROLS_BY_USER_ID_ENDPOINT}`,
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
                        data.successMessage || 'Exit Controls fetched successfully.'
                    );
                }


                return ApiResponseT.badRequest(
                    [{ message: 'Missing or invalid result structure.' }],
                    'Invalid response format'
                );
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format.' }],
                'Failed to search Exit Controls.'
            );

        } catch (error) {
            console.error('Search Exit Controls error:', error);


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
                [{ message: error.message || 'An unexpected error occurred while searching Exit Controls.' }],
                'An unexpected error occurred while searching Exit Controls.'
            );
        }
    }
};
