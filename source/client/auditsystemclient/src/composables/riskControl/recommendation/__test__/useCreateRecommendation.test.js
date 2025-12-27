import { describe, it, expect, vi, beforeEach } from 'vitest';
import { useCreateRecommendation } from '../useCreateRecommendation';
import { createRecommendationRepository } from '@/services/riskControl/recommendation/createRecommendationRepository';

// 🔹 Mock the repository
vi.mock('@/services/riskControl/recommendation/createRecommendationRepository', () => ({
  createRecommendationRepository: {
    createRecommendation: vi.fn(),
  },
}));

describe('useCreateRecommendation', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('creates recommendation successfully', async () => {
    const mockResponse = {
      success: true,
      result: { id: 1, title: 'Test Recommendation' },
      successMessage: 'Created successfully',
    };

    createRecommendationRepository.createRecommendation.mockResolvedValue(mockResponse);

    const {
      createRecommendation,
      isCreating,
      error,
      createdRecommendation,
    } = useCreateRecommendation();

    const result = await createRecommendation({ title: 'Test Recommendation' });

    expect(isCreating.value).toBe(false);
    expect(error.value).toBe(null);
    expect(createdRecommendation.value).toEqual(mockResponse.result);

    expect(result).toEqual({
      success: true,
      data: mockResponse.result,
      message: 'Created successfully',
    });
  });

  it('handles API failure response', async () => {
    const mockResponse = {
      success: false,
      errors: [{ message: 'Validation failed' }],
    };

    createRecommendationRepository.createRecommendation.mockResolvedValue(mockResponse);

    const {
      createRecommendation,
      error,
      createdRecommendation,
    } = useCreateRecommendation();

    const result = await createRecommendation({});

    expect(createdRecommendation.value).toBe(null);
    expect(error.value).toBe('Validation failed');

    expect(result).toEqual({
      success: false,
      message: 'Validation failed',
      errors: [{ message: 'Validation failed' }],
    });
  });

  it('handles thrown exception', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {});

    const mockError = new Error('Network error');
    createRecommendationRepository.createRecommendation.mockRejectedValue(mockError);

    const {
      createRecommendation,
      error,
      isCreating,
    } = useCreateRecommendation();

    const result = await createRecommendation({});

    expect(isCreating.value).toBe(false);
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
      error,
      createdRecommendation,
      isCreating,
    } = useCreateRecommendation();

    error.value = 'Some error';
    createdRecommendation.value = { id: 1 };
    isCreating.value = true;

    resetState();

    expect(error.value).toBe(null);
    expect(createdRecommendation.value).toBe(null);
    expect(isCreating.value).toBe(false);
  });
});