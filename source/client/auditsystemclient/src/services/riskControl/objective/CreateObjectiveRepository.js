import api from '@/services/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const OBJECTIVE_API_ENDPOINT = '/api/objectives';
const CREATE_OBJECTIVE_API_ENDPOINT = '/create-objective';

export const createObjectiveRepository = {
    async createObjective(objectiveData) {
        try {
            const token = authService.getLocalToken();
            const payload = {
                description: objectiveData.description,
              riskControlMatrixId: typeof objectiveData.riskControlMatrixId === 'object'
                ? objectiveData.riskControlMatrixId.value || objectiveData.riskControlMatrixId.id
                    : objectiveData.riskControlMatrixId
            };

            const response = await api.post(
                `${OBJECTIVE_API_ENDPOINT}${CREATE_OBJECTIVE_API_ENDPOINT}`,
                payload,
                {
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    },
                }
            );

            console.log('Create Objective Response:', response);

            const { data, status } = response;
            if (status === 201 && data?.success) {
                return ApiResponseT.ok(data.result, data.successMessage || 'Objective created successfully');
            }
            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.successMessage || 'Failed to create Objective' }],
                data?.successMessage || 'Failed to create Objective.'
            );
        } catch (error) {
            console.error('Error creating Objective:', error);
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
                [{ message: error.message || 'An unexpected error occurred while creating Objective.' }],
                'An unexpected error occurred while creating Objective.'
            );
        }
    }
};
