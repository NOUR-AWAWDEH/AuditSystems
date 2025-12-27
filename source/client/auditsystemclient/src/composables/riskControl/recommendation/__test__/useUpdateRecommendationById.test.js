import { describe, it, expect, vi, beforeEach } from 'vitest';
import { useUpdateRecommendationById } from '../useUpdateRecommendationById';
import { updateRecommendationByIdRepository } from '@/services/riskControl/recommendation/updateRecommendationByIdRepository';
import { validateUpdateRecommendation } from '@/services/validations/validateRecommendationData';

// 🔹 Mock the repository
vi.mock('@/services/riskControl/recommendation/updateRecommendationByIdRepository', () => ({
  updateRecommendationByIdRepository: {
    updateRecommendationById: vi.fn(),
  },
}));

// 🔹 Mock the validation function
vi.mock('@/services/validations/validateRecommendationData', () => ({
  validateUpdateRecommendation: vi.fn(),
}));

describe('useUpdateRecommendationById', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    validateUpdateRecommendation.mockReturnValue(null); // Default to no validation errors
  });

  describe('successful update', () => {
    it('updates recommendation successfully', async () => {
      const mockResponse = {
        success: true,
        result: {
          id: 'rec-123',
          title: 'Updated Recommendation',
          description: 'Updated description',
          observationId: 5,
        },
        successMessage: 'Updated successfully',
      };

      updateRecommendationByIdRepository.updateRecommendationById.mockResolvedValue(mockResponse);

      const {
        updateRecommendation,
        isUpdating,
        error,
        updatedRecommendation,
      } = useUpdateRecommendationById();

      const recommendationData = {
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      };

      const result = await updateRecommendation(recommendationData);

      expect(isUpdating.value).toBe(false);
      expect(error.value).toBe(null);
      expect(updatedRecommendation.value).toEqual(mockResponse.result);

      expect(result).toEqual({
        success: true,
        data: mockResponse.result,
      });

      expect(updateRecommendationByIdRepository.updateRecommendationById).toHaveBeenCalledWith(recommendationData);
    });
  });

  describe('validation', () => {
    it('calls validation function and fails when validation returns error', async () => {
      const validationError = 'Validation failed';
      validateUpdateRecommendation.mockReturnValue(validationError);

      const {
        updateRecommendation,
        isUpdating,
        error,
        updatedRecommendation,
      } = useUpdateRecommendationById();

      const recommendationData = {
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      };

      const result = await updateRecommendation(recommendationData);

      expect(validateUpdateRecommendation).toHaveBeenCalledWith(recommendationData);
      expect(isUpdating.value).toBe(false);
      expect(updatedRecommendation.value).toBe(null);
      expect(error.value).toBe(validationError);

      expect(result).toEqual({
        success: false,
        message: validationError,
        errors: [{ message: validationError }],
      });

      expect(updateRecommendationByIdRepository.updateRecommendationById).not.toHaveBeenCalled();
    });

    it('calls validation function with correct data when validation passes', async () => {
      validateUpdateRecommendation.mockReturnValue(null);

      const mockResponse = {
        success: true,
        result: { id: 'rec-123', title: 'Updated' },
      };
      updateRecommendationByIdRepository.updateRecommendationById.mockResolvedValue(mockResponse);

      const { updateRecommendation } = useUpdateRecommendationById();

      const recommendationData = {
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      };

      await updateRecommendation(recommendationData);

      expect(validateUpdateRecommendation).toHaveBeenCalledWith(recommendationData);
      expect(updateRecommendationByIdRepository.updateRecommendationById).toHaveBeenCalledWith(recommendationData);
    });
  });

  describe('API failure handling', () => {
    it('handles API failure response with errors array', async () => {
      const mockResponse = {
        success: false,
        errors: [{ message: 'Validation failed' }, { message: 'Another error' }],
      };

      updateRecommendationByIdRepository.updateRecommendationById.mockResolvedValue(mockResponse);

      const {
        updateRecommendation,
        error,
        updatedRecommendation,
        isUpdating,
      } = useUpdateRecommendationById();

      const recommendationData = {
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      };

      const result = await updateRecommendation(recommendationData);

      expect(isUpdating.value).toBe(false);
      expect(updatedRecommendation.value).toBe(null);
      expect(error.value).toBe('Validation failed');

      expect(result).toEqual({
        success: false,
        message: 'Validation failed',
        errors: mockResponse.errors,
      });
    });

    it('handles API failure response with successMessage fallback', async () => {
      const mockResponse = {
        success: false,
        successMessage: 'Update failed due to server error',
        errors: [],
      };

      updateRecommendationByIdRepository.updateRecommendationById.mockResolvedValue(mockResponse);

      const {
        updateRecommendation,
        error,
      } = useUpdateRecommendationById();

      const recommendationData = {
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      };

      const result = await updateRecommendation(recommendationData);

      expect(error.value).toBe('Update failed due to server error');

      expect(result).toEqual({
        success: false,
        message: 'Update failed due to server error',
        errors: [{ message: 'Update failed due to server error' }],
      });
    });

    it('handles API failure response with default message', async () => {
      const mockResponse = {
        success: false,
      };

      updateRecommendationByIdRepository.updateRecommendationById.mockResolvedValue(mockResponse);

      const {
        updateRecommendation,
        error,
      } = useUpdateRecommendationById();

      const result = await updateRecommendation({
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      });

      expect(error.value).toBe('Failed to update Recommendation.');

      expect(result).toEqual({
        success: false,
        message: 'Failed to update Recommendation.',
        errors: [{ message: 'Failed to update Recommendation.' }],
      });
    });
  });

  describe('exception handling', () => {
    it('handles thrown exception with message', async () => {
      const mockError = new Error('Network error');
      updateRecommendationByIdRepository.updateRecommendationById.mockRejectedValue(mockError);

      const {
        updateRecommendation,
        error,
        isUpdating,
        updatedRecommendation,
      } = useUpdateRecommendationById();

      const result = await updateRecommendation({
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      });

      expect(isUpdating.value).toBe(false);
      expect(updatedRecommendation.value).toBe(null);
      expect(error.value).toBe('Network error');

      expect(result).toEqual({
        success: false,
        message: 'Network error',
        errors: [{ message: 'Network error' }],
      });
    });

    it('handles thrown exception without message', async () => {
      const mockError = new Error();
      updateRecommendationByIdRepository.updateRecommendationById.mockRejectedValue(mockError);

      const {
        updateRecommendation,
        error,
      } = useUpdateRecommendationById();

      const result = await updateRecommendation({
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      });

      expect(error.value).toBe('Unexpected error while updating Recommendation');

      expect(result).toEqual({
        success: false,
        message: 'Unexpected error while updating Recommendation',
        errors: [{ message: 'Unexpected error while updating Recommendation' }],
      });
    });

    it('handles non-Error thrown exception', async () => {
      updateRecommendationByIdRepository.updateRecommendationById.mockRejectedValue('String error');

      const {
        updateRecommendation,
        error,
      } = useUpdateRecommendationById();

      const result = await updateRecommendation({
        id: 'rec-123',
        title: 'Updated Recommendation',
        description: 'Updated description',
        observationId: 5,
      });

      expect(error.value).toBe('Unexpected error while updating Recommendation');

      expect(result).toEqual({
        success: false,
        message: 'Unexpected error while updating Recommendation',
        errors: [{ message: 'Unexpected error while updating Recommendation' }],
      });
    });
  });

  describe('state management', () => {
    it('sets isUpdating to true during update', async () => {
      const mockResponse = {
        success: true,
        result: { id: 'rec-123', title: 'Updated' },
      };

      updateRecommendationByIdRepository.updateRecommendationById.mockResolvedValue(mockResponse);

      const { updateRecommendation, isUpdating } = useUpdateRecommendationById();

      const updatePromise = updateRecommendation({
        id: 'rec-123',
        title: 'Updated',
        description: 'Description',
        observationId: 5,
      });

      expect(isUpdating.value).toBe(true);

      await updatePromise;

      expect(isUpdating.value).toBe(false);
    });

    it('resets state correctly', () => {
      const {
        resetState,
        error,
        updatedRecommendation,
        isUpdating,
      } = useUpdateRecommendationById();

      // Set some values
      error.value = 'Some error';
      updatedRecommendation.value = { id: 'rec-123' };
      isUpdating.value = true;

      resetState();

      expect(error.value).toBe(null);
      expect(updatedRecommendation.value).toBe(null);
      expect(isUpdating.value).toBe(false);
    });

    it('clears previous error and result before new update', async () => {
      const mockResponse = {
        success: true,
        result: { id: 'rec-123', title: 'Updated' },
      };

      updateRecommendationByIdRepository.updateRecommendationById.mockResolvedValue(mockResponse);

      const { updateRecommendation, error, updatedRecommendation } = useUpdateRecommendationById();

      // Set previous state
      error.value = 'Previous error';
      updatedRecommendation.value = { id: 'old' };

      await updateRecommendation({
        id: 'rec-123',
        title: 'Updated',
        description: 'Description',
        observationId: 5,
      });

      expect(error.value).toBe(null);
      expect(updatedRecommendation.value).toEqual(mockResponse.result);
    });
  });
});