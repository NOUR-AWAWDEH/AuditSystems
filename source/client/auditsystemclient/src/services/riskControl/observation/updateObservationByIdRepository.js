import { ApiResponseT } from '@/utils/ApiResponse';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';

const OBSERVATION_API_ENDPOINT = '/api/Observation';
const OBSERVATION_API_ENDPOINT_ID = '/update-observation-by-id';

export const updateObservationByIdRepository = {
    async updateObservationById(observationData) {
        let responseData = null;
        try {
            const token = authService.getLocalToken();

            const payload = {
                id: observationData.id,
                highlight: observationData.highlight,
                background: observationData.background,
                isActive: observationData.isActive,
                auditProgramId: observationData.auditProgramId,
                rateId: observationData.rateId,
                classification: observationData.classification
            };

            const response = await api.put(
                `${OBSERVATION_API_ENDPOINT}${OBSERVATION_API_ENDPOINT_ID}`,
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
                return ApiResponseT.ok(null, data?.successMessage || 'Observation updated successfully.');
            }

            const errors = Array.isArray(data?.errors)
                ? data.errors
                : [{ message: data?.successMessage || 'Update failed' }];
            console.error('Update observation failed with errors:', errors);
            return ApiResponseT.badRequest(errors, data?.successMessage || 'Failed to update observation.');
            
        } catch (error) {
            responseData = error.response?.data;
            console.error('Error updating observation:', error);

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
                    [{ message: 'Observation not found' }],
                    'Observation not found'
                );
            }
        
            return ApiResponseT.serverError(
                [{ message: error.message || 'An unexpected error occurred while updating observation.' }],
                'Server error'
            );  
        }
    }
};