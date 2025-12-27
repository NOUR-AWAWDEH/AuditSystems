import { describe, it, expect, vi, beforeEach } from 'vitest';
import { useGetRecommendationById } from '@/composables/riskControl/recommendation/useGetRecommendationById';
import { getRecommendationByIdRepository } from '@/services/riskControl/recommendation/getRecommendationByIdRepository';

vi.mock('@/services/riskControl/recommendation/getRecommendationByIdRepository', () => ({
  getRecommendationByIdRepository: {
    getRecommendationById: vi.fn(),
  },
}));

describe('useGetRecommendationById', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('fails validation when recommendationId is missing', async () => {
    const {
      getRecommendationById,
      error,
      recommendation,
      isLoading,
    } = useGetRecommendationById();

    const result = await getRecommendationById(null);

    expect(isLoading.value).toBe(false);
    expect(recommendation.value).toBe(null);
    expect(error.value).toBe('Recommendation ID is required.');

    expect(result).toEqual({
      success: false,
      message: 'Recommendation ID is required.',
      errors: [{ message: 'Recommendation ID is required.' }],
    });
  });

  it('fetches recommendation successfully', async () => {
    const mockResponse = {
      success: true,
      result: { id: 1, name: 'Recommendation 1' },
    };

    getRecommendationByIdRepository.getRecommendationById.mockResolvedValue(mockResponse);

    const {
      getRecommendationById,
      recommendation,
      error,
      isLoading,
    } = useGetRecommendationById();

    const result = await getRecommendationById(1);

    expect(isLoading.value).toBe(false);
    expect(error.value).toBe(null);
    expect(recommendation.value).toEqual(mockResponse.result);

    expect(result).toEqual({
      success: true,
      data: mockResponse.result,
    });
  });

  it('handles API failure response', async () => {
    const mockResponse = {
      success: false,
      errors: [{ message: 'Not found' }],
    };

    getRecommendationByIdRepository.getRecommendationById.mockResolvedValue(mockResponse);

    const {
      getRecommendationById,
      error,
      recommendation,
    } = useGetRecommendationById();

    const result = await getRecommendationById(5);

    expect(recommendation.value).toBe(null);
    expect(error.value).toBe('Not found');

    expect(result).toEqual({
      success: false,
      message: 'Not found',
      errors: [{ message: 'Not found' }],
    });
  });

  it('handles thrown exception', async () => {
    const mockError = new Error('Network error');

    getRecommendationByIdRepository.getRecommendationById.mockRejectedValue(mockError);

    const {
      getRecommendationById,
      error,
      isLoading,
    } = useGetRecommendationById();

    const result = await getRecommendationById(3);

    expect(isLoading.value).toBe(false);
    expect(error.value).toBe('Network error');

    expect(result).toEqual({
      success: false,
      message: 'Network error',
      errors: [{ message: 'Network error' }],
    });
  });

  it('clears recommendation state', () => {
    const {
      clearRecommendation,
      recommendation,
      error,
      isLoading,
    } = useGetRecommendationById();

    recommendation.value = { id: 1 };
    error.value = 'Some error';
    isLoading.value = true;

    clearRecommendation();

    expect(recommendation.value).toBe(null);
    expect(error.value).toBe(null);
    expect(isLoading.value).toBe(false);
  });
});
