import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const DELETE_CONTROL_DESIGN_API_ENDPOINT = '/api/ControlDesign';
const DELETE_CONTROL_DESIGN_BY_ID_API_ENDPOINT = '/delete-control-design-by-id';

export const deleteControlDesignByIdRepository = {
    async deleteControlDesignById(controlDesignId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.delete(
                `${DELETE_CONTROL_DESIGN_API_ENDPOINT}${DELETE_CONTROL_DESIGN_BY_ID_API_ENDPOINT}`, {
                    data: { id: controlDesignId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
            });

            
            const { data, status } = response;

            if (status === 204 || (status === 200 && data?.success !== false)) {
                return ApiResponseT.ok(null, 'Control Design deleted successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Delete Control Design failed' }];
            return ApiResponseT.badRequest(errors, data?.successMessage);

        } catch (error) {
            console.error('Delete Control Design error:', error);
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
                    [{ message: 'Control Design not found' }], 'Control Design not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting Control Design.' }],
                'An unexpected error occurred while deleting Control Design.'
            );
        }
    }
};