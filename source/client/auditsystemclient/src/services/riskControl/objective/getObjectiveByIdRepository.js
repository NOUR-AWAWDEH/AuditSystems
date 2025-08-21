import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const OBJECTIVE_API_ENDPOINT = '/api/Objective';
const OBJECTIVE_API_ENDPOINT_ID = '/get-objective-by-id';

export const getObjectiveByIdRepository = {
    async getObjectiveById(objectiveId) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const response = await api.get(
                `${OBJECTIVE_API_ENDPOINT}${OBJECTIVE_API_ENDPOINT_ID}`, {
                    params: { id: objectiveId },
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                });

            console.log("getObjectiveByIdRepository: " ,response);

            const { data, status } = response;
            responseData = data;

            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(data?.result?.ObjectiveModel, data?.successMessage || 'Objective retrieved successfully');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: 'Failed to retrieve Objective.' }];
            console.error('Retrieve Objective failed with errors:', errors);

            return ApiResponseT.badRequest(
                errors,
                data?.successMessage || 'Failed to retrieve Objective.'
            );

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error retrieving an Objective:', error);

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors ||
                    [{ message: responseData?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(
                    errors,
                    responseData?.successMessage || 'Bad request'
                );
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Objective not found.' }],
                    'Objective not found.'
                );
            }
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while retrieving Objective.' }],
                'An unexpected error occurred while retrieving Objective.'
            );
        }
    }
};
