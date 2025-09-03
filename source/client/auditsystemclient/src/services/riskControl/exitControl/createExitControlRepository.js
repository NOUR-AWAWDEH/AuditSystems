import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const EXIT_CONTROL_API_ENDPOINT = '/api/ExitControl';
const CREATE_EXIT_CONTROL_API_ENDPOINT = '/create-exit-control';


export const createExitControlRepository = {
    async createExitControl(exitControlData) {
        try {
            const token = authService.getLocalToken();
            const payload = {
                description: exitControlData.description,
                riskId: exitControlData.riskId,
                inadequate: exitControlData.inadequate,
            }

            const response = await api.post(
                `${EXIT_CONTROL_API_ENDPOINT}${CREATE_EXIT_CONTROL_API_ENDPOINT}`,
                payload,
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    },
                }
            );

            console.log('Response of Create Exit Control Repositroy: ', response.data);
            const { data, status } = response;
            if (status === 201 && data?.success) {
                return ApiResponseT.ok(data.result, data.successMessage || 'Exit Control created successfully');
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.successMessage || 'Failed to create Exit Control' }],
                data?.successMessage || 'Failed to create Exit Control.'
            );

        } catch (error) {
            console.error('Error creating Exit Control:', error);
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
                [{ message: error.message || 'An unexpected error occurred while creating Exit Control.' }],
                'An unexpected error occurred while creating Exit Control.'
            );
        }
    }
};
