import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const RISK_CONTROL_MATRIX_BASE_ENDPOINT = '/api/RiskControlMatrix';
const SEARCH_RISK_CONTROL_MATRICES_BY_USER_ID_ENDPOINT = '/search-risk-control-matrices-by-user-id';

export const searchRiskControlMatricesByUserIdRepository = {
    async searchRiskControlMatricesByUserId(searchTerm = '', pageNumber = 1, pageSize = 10, subProcessId = '') {
        try {
            const token = authService.getLocalToken();

            const params = {
                searchTerm: searchTerm,
                subProcessId: subProcessId,
                page: pageNumber,
                pageSize: pageSize
            };

            const response = await api.get(
                `${RISK_CONTROL_MATRIX_BASE_ENDPOINT}${SEARCH_RISK_CONTROL_MATRICES_BY_USER_ID_ENDPOINT}`,
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

                if (result?.items && typeof result.totalCount === 'number') {
                    return ApiResponseT.ok(
                        result,
                        data.successMessage || 'Risk Control Matrices fetched successfully.'
                    );
                }

                return ApiResponseT.badRequest(
                    [{ message: 'Missing or invalid result structure.' }],
                    'Invalid response format'
                );
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format.' }],
                'Failed to search risk control matrices'
            );

        } catch (error) {
            console.error('Search risk control matrices error:', error);

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
                [{ message: error.message || 'An unexpected error occurred while searching risk control matrices.' }],
                'An unexpected error occurred while searching risk control matrices.'
            );
        }
    }
};
