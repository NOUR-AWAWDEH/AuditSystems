import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const DELETE_EXIT_CONTROL_API_ENDPOINT = '/api/ExitControl';
const DELETE_EXIT_CONTROL_BY_ID_API_ENDPOINT = '/delete-exit-control-by-id';

export const deleteExitControlByIdRepository = {
    async deleteExitControlById(exitControlId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.delete(
                `${DELETE_EXIT_CONTROL_API_ENDPOINT}${DELETE_EXIT_CONTROL_BY_ID_API_ENDPOINT}`, {
                    data: { id: exitControlId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
            });

            
            const { data, status } = response;

            if (status === 204 || (status === 200 && data?.success !== false)) {
                return ApiResponseT.ok(null, 'Exit Control deleted successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Delete Exit Control failed' }];
            return ApiResponseT.badRequest(errors, data?.successMessage);

        } catch (error) {
            console.error('Delete Exit Control error:', error);
            const responseData = error.response?.data;

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors ||
                    [{ message: responseData?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.successMessage || 'Bad request');
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Exit Control not found' }], 'Exit Control not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting Exit Control.' }],
                'An unexpected error occurred while deleting Exit Control.'
            );
        }
    }
};
