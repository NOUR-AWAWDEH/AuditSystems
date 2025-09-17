import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const OBJECTIVE_API_ENDPOINT = '/api/Objective';
const OBJECTIVE_API_ENDPOINT_ID = '/update-objective';

export const updateObjectiveByIdRepository = {
    async updateObjectiveById(objectiveData) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const payload = {
                id: objectiveData.id,
                description: objectiveData.description,
                riskControlMatrixId: (objectiveData.riskControlMatrixId && typeof objectiveData.riskControlMatrixId === 'object')
                        ? objectiveData.riskControlMatrixId.value || objectiveData.riskControlMatrixId.id
                        : objectiveData.riskControlMatrixId
            };

            const response = await api.put(
                `${OBJECTIVE_API_ENDPOINT}${OBJECTIVE_API_ENDPOINT_ID}`,
                payload,
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            const { data, status } = response;
            responseData = data;

            if (status === 200 && data?.success !== false) {
                return ApiResponseT.ok(null, data?.successMessage || 'Objective updated successfully.');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Update failed' }];
            console.error('Update objective failed with errors:', errors);
            return ApiResponseT.badRequest(errors, data?.successMessage || 'Failed to update objective.');

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error updating objective:', error);

            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }

            if (error.response?.status === 400) {
                const errors = responseData?.errors || [{ message: responseData?.message || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.message || 'Bad request');
            }

            if (error.response?.status === 404) {
                return ApiResponseT.notFound(
                    [{ message: 'Objective not found' }],
                    'Objective not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while updating objective.' }],
                'An unexpected error occurred while updating objective.'
            );
        }
    }
};
