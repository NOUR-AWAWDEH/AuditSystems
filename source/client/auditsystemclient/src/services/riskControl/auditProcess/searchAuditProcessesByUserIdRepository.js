import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const AUDIT_PROCESS_BASE_ENDPOINT = '/api/AuditProcess';
const SEARCH_AUDIT_PROCESS_BY_USER_ID_ENDPOINT = '/search-audit-processes-by-user-id';

export const searchAuditProcessesByUserIdRepository = {
    async searchAuditProcessesByUserId(searchTerm = '', pageNumber = 1, pageSize = 10, subDepartmentId = '') {
        try {
            const token = authService.getLocalToken();

            const params = {
                searchTerm,
                subDepartmentId: subDepartmentId,
                page: pageNumber,
                pageSize
            };

            const response = await api.get(
                `${AUDIT_PROCESS_BASE_ENDPOINT}${SEARCH_AUDIT_PROCESS_BY_USER_ID_ENDPOINT}`,
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

                // Defensive check in case the result structure changes
                if (result?.items && typeof result.totalCount === 'number') {
                    return ApiResponseT.ok(
                        result,
                        data.successMessage || 'Audit processes fetched successfully.'
                    );
                }

                return ApiResponseT.badRequest(
                    [{ message: 'Missing or invalid result structure.' }],
                    'Invalid response format'
                );
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format.' }],
                'Failed to search audit processes'
            );

        } catch (error) {
            console.error('Search audit processes error:', error);

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
                [{ message: error.message || 'An unexpected error occurred.' }],
                'Server error'
            );
        }
    }
};