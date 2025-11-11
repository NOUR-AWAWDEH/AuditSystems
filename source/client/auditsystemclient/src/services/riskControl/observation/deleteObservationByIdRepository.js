import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const OBSERVATION_API_ENDPOINT = '/api/Observation';
const DELETE_OBSERVATION_API_ENDPOINT = '/delete-observation-by-id';

export const deleteObservationByIdRepository = {
    async deleteObservationById(observationId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.delete(
                `${OBSERVATION_API_ENDPOINT}${DELETE_OBSERVATION_API_ENDPOINT}`, {
                    data: { id: observationId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
            });

            const { data, status } = response;

            if (status === 204 || (status === 200 && data?.success !== false)) {
                return ApiResponseT.ok(null, 'Observation deleted successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Delete Observation failed' }];
            return ApiResponseT.badRequest(errors, data?.successMessage);

        } catch (error) {
            console.error('Delete Observation error:', error);
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
                    [{ message: 'Observation not found' }], 'Observation not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting Observation.' }],
                'Delete Observation failed'
            );
        }
    }
};
