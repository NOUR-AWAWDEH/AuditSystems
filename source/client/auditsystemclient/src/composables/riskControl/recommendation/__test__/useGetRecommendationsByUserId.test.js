import { describe, it, expect, vi, beforeEach } from 'vitest';
import { useGetRecommendationsByUserId } from '../useGetRecommendationsByUserId';
import { getRecommendationsByUserIdRepository } from '@/services/riskControl/recommendation/getRecommendationsByUserIdRepository';

// 🔹 Mock the repository
vi.mock('@/services/riskControl/recommendation/getRecommendationsByUserIdRepository', () => ({
  getRecommendationsByUserIdRepository: {
    getRecommendationsByUserId: vi.fn(),
  },
}));

describe('useGetRecommendationsByUserId', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('fetches recommendations successfully', async () => {
    const mockResponse = {
      success: true,
      result: {
        recommendationModel: [
          { id: 1, title: 'Recommendation 1' },
          { id: 2, title: 'Recommendation 2' },
        ],
      },
    };

    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      getRecommendationsByUserId,
      recommendations,
      error,
      isLoading,
    } = useGetRecommendationsByUserId();

    const result = await getRecommendationsByUserId();

    expect(isLoading.value).toBe(false);
    expect(error.value).toBe(null);
    expect(recommendations.value).toEqual(mockResponse.result.recommendationModel);

    expect(result).toEqual({
      success: true,
      data: mockResponse.result.recommendationModel,
    });
  });

  it('handles empty recommendations list', async () => {
    const mockResponse = {
      success: true,
      result: {
        recommendationModel: [],
      },
    };

    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      getRecommendationsByUserId,
      recommendations,
      error,
      isLoading,
    } = useGetRecommendationsByUserId();

    const result = await getRecommendationsByUserId();

    expect(isLoading.value).toBe(false);
    expect(error.value).toBe(null);
    expect(recommendations.value).toEqual([]);

    expect(result).toEqual({
      success: true,
      data: [],
    });
  });

  it('handles API failure response with errors array', async () => {
    const mockResponse = {
      success: false,
      errors: [{ message: 'Failed to fetch data' }],
      successMessage: 'Operation failed',
    };

    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      getRecommendationsByUserId,
      recommendations,
      error,
      isLoading,
    } = useGetRecommendationsByUserId();

    const result = await getRecommendationsByUserId();

    expect(isLoading.value).toBe(false);
    expect(recommendations.value).toEqual([]);
    expect(error.value).toBe('Failed to fetch data');

    expect(result).toEqual({
      success: false,
      message: 'Failed to fetch data',
      errors: [{ message: 'Failed to fetch data' }],
    });
  });

  it('handles API failure response with successMessage fallback', async () => {
    const mockResponse = {
      success: false,
      successMessage: 'Custom error message',
    };

    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      getRecommendationsByUserId,
      recommendations,
      error,
      isLoading,
    } = useGetRecommendationsByUserId();

    const result = await getRecommendationsByUserId();

    expect(isLoading.value).toBe(false);
    expect(recommendations.value).toEqual([]);
    expect(error.value).toBe('Custom error message');

    expect(result).toEqual({
      success: false,
      message: 'Custom error message',
      errors: [{ message: 'Custom error message' }],
    });
  });

  it('handles API failure response with default error message', async () => {
    const mockResponse = {
      success: false,
    };

    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      getRecommendationsByUserId,
      recommendations,
      error,
      isLoading,
    } = useGetRecommendationsByUserId();

    const result = await getRecommendationsByUserId();

    expect(isLoading.value).toBe(false);
    expect(recommendations.value).toEqual([]);
    expect(error.value).toBe('Failed to fetch Recommendations');

    expect(result).toEqual({
      success: false,
      message: 'Failed to fetch Recommendations',
      errors: [{ message: 'Failed to fetch Recommendations' }],
    });
  });

  it('handles thrown exception', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {});

    const mockError = new Error('Network error');
    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockRejectedValue(mockError);

    const {
      getRecommendationsByUserId,
      recommendations,
      error,
      isLoading,
    } = useGetRecommendationsByUserId();

    const result = await getRecommendationsByUserId();

    expect(isLoading.value).toBe(false);
    expect(recommendations.value).toEqual([]);
    expect(error.value).toBe('Network error');

    expect(result).toEqual({
      success: false,
      message: 'Network error',
      errors: [{ message: 'Network error' }],
    });
  });

  it('handles thrown exception with default error message', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {});

    const mockError = { message: '' };
    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockRejectedValue(mockError);

    const {
      getRecommendationsByUserId,
      recommendations,
      error,
      isLoading,
    } = useGetRecommendationsByUserId();

    const result = await getRecommendationsByUserId();

    expect(isLoading.value).toBe(false);
    expect(recommendations.value).toEqual([]);
    expect(error.value).toBe('Failed to fetch recommendations');

    expect(result).toEqual({
      success: false,
      message: 'Failed to fetch recommendations',
      errors: [{ message: 'Failed to fetch recommendations' }],
    });
  });

  it('sets loading state correctly during fetch', async () => {
    const mockResponse = {
      success: true,
      result: {
        recommendationModel: [{ id: 1, title: 'Test' }],
      },
    };

    // Create a promise that we can control
    let resolvePromise;
    const promise = new Promise((resolve) => {
      resolvePromise = resolve;
    });

    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockReturnValue(promise);

    const {
      getRecommendationsByUserId,
      isLoading,
    } = useGetRecommendationsByUserId();

    // Start the fetch
    const fetchPromise = getRecommendationsByUserId();

    // Check loading is true during fetch
    expect(isLoading.value).toBe(true);

    // Resolve the promise
    resolvePromise(mockResponse);

    // Wait for completion
    await fetchPromise;

    // Check loading is false after completion
    expect(isLoading.value).toBe(false);
  });

  it('clears previous state before fetch', async () => {
    const mockResponse = {
      success: true,
      result: {
        recommendationModel: [{ id: 1, title: 'New Recommendation' }],
      },
    };

    getRecommendationsByUserIdRepository.getRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      getRecommendationsByUserId,
      recommendations,
      error,
      isLoading,
    } = useGetRecommendationsByUserId();

    // Set initial state
    recommendations.value = [{ id: 999, title: 'Old Recommendation' }];
    error.value = 'Previous error';
    isLoading.value = false;

    const result = await getRecommendationsByUserId();

    // Should clear previous state and set new data
    expect(isLoading.value).toBe(false);
    expect(error.value).toBe(null);
    expect(recommendations.value).toEqual([{ id: 1, title: 'New Recommendation' }]);

    expect(result).toEqual({
      success: true,
      data: [{ id: 1, title: 'New Recommendation' }],
    });
  });
});
