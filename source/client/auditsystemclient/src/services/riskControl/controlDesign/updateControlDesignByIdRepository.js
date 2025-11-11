import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const CONTROL_DESIGN_API_ENDPOINT = '/api/ControlDesign';
const CONTROL_DESIGN_API_ENDPOINT_ID = '/update-control-design-by-id';

export const updateControlDesignByIdRepository = {
    async updateControlDesignById(controlDesignData) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const payload = {
                id: controlDesignData.id,
                description: controlDesignData.description,
                exitControlId: controlDesignData.exitControlId
            };

            const response = await api.put(
                `${CONTROL_DESIGN_API_ENDPOINT}${CONTROL_DESIGN_API_ENDPOINT_ID}`,
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
                return ApiResponseT.ok(null, data?.successMessage || 'Control Design updated successfully.');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Update failed' }];
            console.error('Update control design failed with errors:', errors);
            return ApiResponseT.badRequest(errors, data?.successMessage || 'Failed to update control design.');

        } catch (error) {
            responseData = error.response?.data;
            console.error('Error updating control design:', error);

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
                    [{ message: 'Control Design not found' }],
                    'Control Design not found'
                );
            }

            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while updating control design.' }],
                'An unexpected error occurred while updating control design.'
            );
        }
    }
};