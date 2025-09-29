import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const CONTROL_DESIGN_API_ENDPOINT = '/api/ControlDesign';
const CREATE_CONTROL_DESIGN_API_ENDPOINT = '/create-control-design';


export const createControlDesignRepository = {
    async createControlDesign(controlDesignData) {
        try {
            const token = authService.getLocalToken();
            const payload = {
                description: controlDesignData.description,
                exitControlId: controlDesignData.exitControlId,
                inadequate: controlDesignData.inadequate,
            }

            const response = await api.post(
                `${CONTROL_DESIGN_API_ENDPOINT}${CREATE_CONTROL_DESIGN_API_ENDPOINT}`,
                payload,
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    },
                }
            );

            const { data, status } = response;
            if (status === 201 && data?.success) {
                return ApiResponseT.ok(data.result, data.successMessage || 'Control Design created successfully');
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.successMessage || 'Failed to create Control Design' }],
                data?.successMessage || 'Failed to create Control Design.'
            );

        } catch (error) {
            console.error('Error creating Control Design:', error);
            const responseData = error.response?.data;
            if (error.response?.status === 401) {
                return ApiResponseT.unauthorized(
                    [{ message: 'Authentication required. Please log in again.' }],
                    'Authentication required'
                );
            }
            if (error.response?.status === 400) {
                const errors = responseData?.errors || [{ message: responseData?.successMessage || 'Bad request' }];
                return ApiResponseT.badRequest(errors, responseData?.successMessage || 'Bad request');
            }
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while creating Control Design.' }],
                'An unexpected error occurred while creating Control Design.'
            );
        }
    }
};