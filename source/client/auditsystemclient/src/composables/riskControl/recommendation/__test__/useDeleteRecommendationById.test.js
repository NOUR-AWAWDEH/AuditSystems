import { describe, it, expect, vi, beforeEach } from 'vitest';
import { useDeleteRecommendationById } from '../useDeleteRecommendationById';
import { deleteRecommendationByIdRepository } from '@/services/riskControl/recommendation/deleteRecommendationByIdRepository';

// 🔹 Mock repository
vi.mock('@/services/riskControl/recommendation/deleteRecommendationByIdRepository', () => ({
  deleteRecommendationByIdRepository: {
    deleteRecommendationById: vi.fn(),
  },
}));

describe('useDeleteRecommendationById', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('fails validation when recommendationId is missing', async () => {
    const {
      deleteRecommendation,
      error,
      deletedRecommendation,
      isDeleting,
    } = useDeleteRecommendationById();

    const result = await deleteRecommendation(null);

    expect(isDeleting.value).toBe(false);
    expect(deletedRecommendation.value).toBe(null);
    expect(error.value).toBe('Recommendation ID is required');

    expect(result).toEqual({
      success: false,
      message: 'Recommendation ID is required',
      errors: [{ message: 'Recommendation ID is required' }],
    });
  });

  it('deletes recommendation successfully', async () => {
    const mockResponse = {
      success: true,
      successMessage: 'Deleted successfully',
    };

    deleteRecommendationByIdRepository.deleteRecommendationById.mockResolvedValue(mockResponse);

    const {
      deleteRecommendation,
      error,
      deletedRecommendation,
      isDeleting,
    } = useDeleteRecommendationById();

    const result = await deleteRecommendation(10);

    expect(isDeleting.value).toBe(false);
    expect(error.value).toBe(null);
    expect(deletedRecommendation.value).toBe(true);

    expect(result).toEqual({
      success: true,
      message: 'Deleted successfully',
    });
  });

  it('handles API failure response', async () => {
    const mockResponse = {
      success: false,
      errors: [{ message: 'Delete failed' }],
    };

    deleteRecommendationByIdRepository.deleteRecommendationById.mockResolvedValue(mockResponse);

    const {
      deleteRecommendation,
      error,
      deletedRecommendation,
    } = useDeleteRecommendationById();

    const result = await deleteRecommendation(5);

    expect(deletedRecommendation.value).toBe(null);
    expect(error.value).toBe('Delete failed');

    expect(result).toEqual({
      success: false,
      message: 'Delete failed',
      errors: [{ message: 'Delete failed' }],
    });
  });

  it('handles thrown exception', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {}); // optional

    const mockError = new Error('Network error');
    deleteRecommendationByIdRepository.deleteRecommendationById.mockRejectedValue(mockError);

    const {
      deleteRecommendation,
      error,
      isDeleting,
    } = useDeleteRecommendationById();

    const result = await deleteRecommendation(3);

    expect(isDeleting.value).toBe(false);
    expect(error.value).toBe('Network error');

    expect(result).toEqual({
      success: false,
      message: 'Network error',
      errors: [{ message: 'Network error' }],
    });
  });

  it('resets state correctly', () => {
    const {
      resetState,
      isDeleting,
      error,
      deletedRecommendation,
    } = useDeleteRecommendationById();

    isDeleting.value = true;
    error.value = 'Some error';
    deletedRecommendation.value = true;

    resetState();

    expect(isDeleting.value).toBe(false);
    expect(error.value).toBe(null);
    expect(deletedRecommendation.value).toBe(null);
  });
});
