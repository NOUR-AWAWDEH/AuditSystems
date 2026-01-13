import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';

const OBSERVATION_API_ENDPOINT = '/api/Observation';
const CREATE_OBSERVATION_API_ENDPOINT = '/create-observation';

export const createObservationRepository = { 
    async createObservation(observationData) {
        try{
            const token = authService.getLocalToken();

            const payload = {
                highlight: observationData.highlight,
                background: observationData.background,
                isActive: observationData.isActive,
                auditProgramId: observationData.auditProgramId,
                rateId: observationData.rateId,
                classification: observationData.classification
            };

            const response = await api.post(
                `${OBSERVATION_API_ENDPOINT}${CREATE_OBSERVATION_API_ENDPOINT}`,
                payload,
                {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
            });

            const { data, status } = response;

            if (status === 201 && data?.success) {
                return ApiResponseT.ok(data.result, data.successMessage || 'Observation created successfully');
            }

            return ApiResponseT.badRequest(
                data?.errors || [{ message: data?.successMessage || 'Failed to create Observation' }],
                data?.successMessage || 'Failed to create Observation.'
            );

        }catch(error){
            console.error('Error creating Observation:', error);
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
                [{ message: error.message || 'An unexpected error occurred while creating observation.' }], 'Server error'
            );
        }
    }
};