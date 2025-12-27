import api from "@/services/api/api";
import { ApiResponseT } from "@/utils/ApiResponse";
import authService from '@/services/auth/authRepos/authService';

const DELETE_RECOMMENDATION_API_ENDPOINT = '/api/Recommendation';
const DELETE_RECOMMENDATION_BY_ID_API_ENDPOINT = '/delete-recommendation-by-id';

export const deleteRecommendationByIdRepository = {
    async deleteRecommendationById(recommendationId) {
        try {
            const token = authService.getLocalToken();

            const response = await api.delete(
                `${DELETE_RECOMMENDATION_API_ENDPOINT}${DELETE_RECOMMENDATION_BY_ID_API_ENDPOINT}`, {
                    data: { id: recommendationId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
            });


            const { data, status } = response;

            if (status === 204 || (status === 200 && data?.success !== false)) {
                return ApiResponseT.ok(null, 'Recommendation deleted successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Delete Recommendation failed' }];
            return ApiResponseT.badRequest(errors, data?.successMessage);

        } catch (error) {
            console.error('Delete Recommendation error:', error);
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
                    [{ message: 'Recommendation not found' }], 'Recommendation not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while deleting Recommendation.' }],
                'An unexpected error occurred while deleting Recommendation.'
            );
        }
    }
};
