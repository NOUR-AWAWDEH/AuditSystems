import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const OBJECTIVE_API_ENDPOINT = '/api/Objective';
const OBJECTIVE_API_ENDPOINT_ID = '/get-objectives-by-user-id';

export const getObjectivesByUserIdRepository = {
    async getObjectivesByUserId() {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${OBJECTIVE_API_ENDPOINT}${OBJECTIVE_API_ENDPOINT_ID}`, {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                }
            );


            const { data, status } = response;
            responseData = data;

            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(data?.result?.objectiveModels, data?.successMessage || 'Objectives retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve Objectives.' }];
            console.error('Retrieve Objectives failed with errors:', errors);
            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve Objectives.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving Objectives:', error);

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors ||
                    [{
                        message: responseData?.successMessage ||
                            'Bad request'
                    }];
                return ApiResponseT.badRequest(
                    errors,
                    responseData?.successMessage || 'Bad request'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving Objectives.' }],
                'An unexpected error occurred while retrieving Objectives.'
            );
        }
    }
};
