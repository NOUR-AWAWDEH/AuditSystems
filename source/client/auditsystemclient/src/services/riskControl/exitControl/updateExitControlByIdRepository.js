import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const EXIT_CONTROL_API_ENDPOINT = '/api/ExitControl';
const EXIT_CONTROL_API_ENDPOINT_ID = '/update-exit-control-by-id';

export const updateExitControlByIdRepository = {
    async updateExitControlById(exitControlData) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const payload = {
                id: exitControlData.id,
                description: exitControlData.description,
                objectiveId: exitControlData.objectiveId,
                inadequate: exitControlData.inadequate,
            };

            const response = await api.put(
                `${EXIT_CONTROL_API_ENDPOINT}${EXIT_CONTROL_API_ENDPOINT_ID}`,
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
                return ApiResponseT.ok(null, data?.successMessage || 'Exit Control updated successfully.');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Update failed' }];
            console.error('Update exit control failed with errors:', errors);
            return ApiResponseT.badRequest(errors, data?.successMessage || 'Failed to update exit control.');

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error updating exit control:', error);

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
                    [{ message: 'Exit Control not found' }],
                    'Exit Control not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while updating exit control.' }],
                'An unexpected error occurred while updating exit control.'
            );
        }
    }
};