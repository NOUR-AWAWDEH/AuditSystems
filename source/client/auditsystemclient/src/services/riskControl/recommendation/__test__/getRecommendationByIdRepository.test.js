import { vi, describe, it, expect, beforeEach } from 'vitest';

vi.mock('@/services/api/api', () => ({
  default: { get: vi.fn() },
}));

vi.mock('@/services/auth/authRepos/authService', () => ({
  default: { getLocalToken: vi.fn() },
}));

import { getRecommendationByIdRepository } from '@/services/riskControl/recommendation/getRecommendationByIdRepository';
import api from '@/services/api/api';
import authService from '@/services/auth/authRepos/authService';
import { ApiResponseT } from '@/utils/ApiResponse';

describe('getRecommendationByIdRepository', () => {
  const token = 'test-token';

  beforeEach(() => {
    vi.clearAllMocks();
    authService.getLocalToken.mockReturnValue(token);
  });

  it('successfully retrieves recommendation with 200 status', async () => {
    const mockRecommendation = {
      id: 'rec-123',
      title: 'Test Recommendation',
      description: 'Test description',
      observationId: 'obs-456'
    };

    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: { recommendationModel: mockRecommendation },
        successMessage: 'Retrieved successfully'
      },
    });

    const result = await getRecommendationByIdRepository.getRecommendationById('rec-123');

    expect(api.get).toHaveBeenCalledWith(
      '/api/Recommendation/get-recommendation-by-id',
      expect.objectContaining({
        params: { id: 'rec-123' },
        headers: expect.any(Object)
      })
    );
    
    expect(result).toEqual(ApiResponseT.ok(mockRecommendation, 'Retrieved successfully'));
  });

  it('handles 200 status with success false as bad request', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: false,
        successMessage: 'Not found',
        errors: [{ message: 'Recommendation not accessible' }]
      },
    });

    const result = await getRecommendationByIdRepository.getRecommendationById('rec-123');

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Recommendation not accessible' }],
      'Not found'
    ));
  });

  it('handles 200 status with missing success field as success', async () => {
    const mockRecommendation = { id: 'rec-123', title: 'Test' };

    api.get.mockResolvedValue({
      status: 200,
      data: {
        result: { recommendationModel: mockRecommendation },
        successMessage: 'Retrieved'
      },
    });

    const result = await getRecommendationByIdRepository.getRecommendationById('rec-123');

    expect(result).toEqual(ApiResponseT.ok(mockRecommendation, 'Retrieved'));
  });

  it('handles 401 Unauthorized', async () => {
    api.get.mockRejectedValue({
      response: { status: 401 },
    });

    const result = await getRecommendationByIdRepository.getRecommendationById('rec-123');

    expect(result).toEqual(ApiResponseT.unauthorized(
      [{ message: 'Authentication required. Please log in again.' }],
      'Authentication required'
    ));
  });

  it('handles 400 Bad Request', async () => {
    api.get.mockRejectedValue({
      response: {
        status: 400,
        data: { errors: [{ message: 'Invalid ID format' }], successMessage: 'Bad input' },
      },
    });

    const result = await getRecommendationByIdRepository.getRecommendationById('invalid-id');

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Invalid ID format' }],
      'Bad input'
    ));
  });

  it('handles 404 Not Found', async () => {
    api.get.mockRejectedValue({
      response: { status: 404 },
    });

    const result = await getRecommendationByIdRepository.getRecommendationById('non-existent-id');

    expect(result).toEqual(ApiResponseT.notFound(
      [{ message: 'Recommendation not found.' }],
      'Recommendation not found.'
    ));
  });

  it('handles unexpected errors', async () => {
    api.get.mockRejectedValue(new Error('Network error'));

    const result = await getRecommendationByIdRepository.getRecommendationById('rec-123');

    expect(result).toEqual(ApiResponseT.serverError(
      [{ message: 'Network error' }],
      'An unexpected error occurred while retrieving Recommendation.'
    ));
  });

  it('includes correct headers in API call', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: { success: true, result: { recommendationModel: {} } },
    });

    await getRecommendationByIdRepository.getRecommendationById('rec-123');

    const [, config] = api.get.mock.calls[0];
    expect(config.headers).toEqual({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  });

  it('sends recommendation ID as query parameter', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: { success: true, result: { recommendationModel: {} } },
    });

    await getRecommendationByIdRepository.getRecommendationById('test-id-123');

    const [, config] = api.get.mock.calls[0];
    expect(config.params).toEqual({ id: 'test-id-123' });
  });

  it('returns recommendation model from nested result structure', async () => {
    const mockRecommendation = {
      id: 'rec-456',
      title: 'Nested Recommendation',
      observationId: 'obs-789'
    };

    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: {
          recommendationModel: mockRecommendation
        }
      },
    });

    const result = await getRecommendationByIdRepository.getRecommendationById('rec-456');

    expect(result.result).toEqual(mockRecommendation);
  });
});
