import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const AUDIT_PROCESS_BASE_ENDPOINT = '/api/AuditProcess';
const SEARCH_AUDIT_PROCESS_BY_USER_ID_ENDPOINT = '/search-audit-processes-by-user-id';

export const searchAuditProcessesByUserIdRepository = {
    async searchAuditProcessesByUserId(searchParams = {}) {
        try {
            const token = authService.getLocalToken();

            if (!token) {
                throw new Error('Authentication token not found');
            }

            const queryParams = new URLSearchParams();

            if (searchParams.searchTerm) {
                queryParams.append('searchTerm', searchParams.searchTerm);
            }

            if (searchParams.subDepartment) {
                queryParams.append('subDepartment', searchParams.subDepartment);
            }

            if (searchParams.page) {
                queryParams.append('page', searchParams.page);
            }

            if (searchParams.pageSize) {
                queryParams.append('pageSize', searchParams.pageSize);
            }

            const queryString = queryParams.toString();
            const url = `${AUDIT_PROCESS_BASE_ENDPOINT}${SEARCH_AUDIT_PROCESS_BY_USER_ID_ENDPOINT}${queryString ? `?${queryString}` : ''}`;
            const response = await api.get(url, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response?.success && response.status === 200) {
                return ApiResponseT.ok(response?.result, response?.data?.successMessage || 'Processes retrieved successfully.');
            }

            return ApiResponseT.badRequest(
                response?.errors || [{ message: 'Failed to search processes.' }],
                response?.data?.successMessage || 'Failed to search processes.'
            );

        } catch (error) {
            console.error("Error searching processes:", error);
            
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }
            
            return ApiResponseT.internalServerError(
                error.response?.data?.message || error.message || "Failed to retrieve processes."
            );
        }
    }
};