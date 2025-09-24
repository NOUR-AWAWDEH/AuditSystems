import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const AUDIT_PROGRAM_BASE_ENDPOINT = '/api/AuditProgram';
const SEARCH_AUDIT_PROGRAMS_BY_USER_ID_ENDPOINT = '/search-audit-programs-by-user-id';

export const searchAuditProgramsByUserIdRepository = {
    async searchAuditProgramsByUserId(searchTerm = '', pageNumber = 1, pageSize = 10, exitControlId = '') {
        try {
            const token = authService.getLocalToken();

            const params = {
                searchTerm: searchTerm || '',
                exitControlId: exitControlId,
                page: pageNumber,
                pageSize: pageSize
            };

            const response = await api.get(
                `${AUDIT_PROGRAM_BASE_ENDPOINT}${SEARCH_AUDIT_PROGRAMS_BY_USER_ID_ENDPOINT}`,
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
                        data.successMessage || 'Audit Programs fetched successfully.'
                    );
                }


                return ApiResponseT.badRequest(
                    [{ message: 'Missing or invalid result structure.' }],
                    'Invalid response format'
                );
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format.' }],
                'Failed to search Audit Programs.'
            );

        } catch (error) {
            console.error('Search Audit Programs error:', error);


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
                [{ message: error.message || 'An unexpected error occurred while searching Audit Programs.' }],
                'An unexpected error occurred while searching Audit Programs.'
            );
        }
    }
};