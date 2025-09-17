import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const SUB_PROCESS_BASE_ENDPOINT = '/api/SubProcess';
const SEARCH_SUB_PROCESS_BY_USER_ID_ENDPOINT = '/search-sub-process-by-user-id';

export const searchSubProcessesByUserIdRepository = {
    async searchSubProcessesByUserId(searchTerm = '', pageNumber = 1, pageSize = 10, auditProcessId = '') {
        try {
            const token = authService.getLocalToken();

            const params = {
                searchTerm: searchTerm,
                auditProcessId: auditProcessId,
                page: pageNumber,
                pageSize: pageSize
            };

            const response = await api.get(
                `${SUB_PROCESS_BASE_ENDPOINT}${SEARCH_SUB_PROCESS_BY_USER_ID_ENDPOINT}`,
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
                        data.successMessage || 'SubProcesses fetched successfully.'
                    );
                }

                return ApiResponseT.badRequest(
                    [{ message: 'Missing or invalid result structure.' }],
                    'Invalid response format'
                );
            }

            return ApiResponseT.badRequest(
                [{ message: 'Unexpected response format.' }],
                'Failed to search sub processes'
            );

        } catch (error) {
            console.error('Search sub processes error:', error);

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