import { vi, describe, it, expect, beforeEach } from 'vitest';

vi.mock('@/services/api/api', () => ({
  default: { get: vi.fn() },
}));

vi.mock('@/services/auth/authRepos/authService', () => ({
  default: { getLocalToken: vi.fn() },
}));

import {
  getRecommendationByUserIdRepository,
  getRecommendationsByUserIdRepository
} from '@/services/riskControl/recommendation/getRecommendationsByUserIdRepository';

import api from '@/services/api/api';
import authService from '@/services/auth/authRepos/authService';
import { ApiResponseT } from '@/utils/ApiResponse';

describe('getRecommendationByUserIdRepository', () => {
  const token = 'test-token';

  beforeEach(() => {
    vi.clearAllMocks();
    authService.getLocalToken.mockReturnValue(token);
  });

  it('successfully retrieves recommendations with 200 status', async () => {
    const mockRecommendations = [
      { id: 'rec-1', title: 'Recommendation 1', observationId: 'obs-1' },
      { id: 'rec-2', title: 'Recommendation 2', observationId: 'obs-2' }
    ];

    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: mockRecommendations,
        successMessage: 'Retrieved successfully'
      },
    });

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(api.get).toHaveBeenCalledWith(
      '/api/Recommendation/get-recommendation-by-user-id',
      expect.objectContaining({
        headers: expect.any(Object)
      })
    );
    expect(result).toEqual(ApiResponseT.ok(mockRecommendations, 'Retrieved successfully'));
  });

  it('handles 200 status with success false as bad request', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: false,
        successMessage: 'Access denied',
        errors: [{ message: 'Permission denied' }]
      },
    });

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Permission denied' }],
      'Access denied'
    ));
  });

  it('handles 200 status with missing success field as success', async () => {
    const mockRecommendations = [{ id: 'rec-1', title: 'Test' }];

    api.get.mockResolvedValue({
      status: 200,
      data: {
        result: mockRecommendations,
        successMessage: 'Retrieved'
      },
    });

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.ok(mockRecommendations, 'Retrieved'));
  });

  it('returns empty array when result is undefined', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        successMessage: 'No recommendations found'
      },
    });

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.ok(undefined, 'No recommendations found'));
  });

  it('handles 401 Unauthorized', async () => {
    api.get.mockRejectedValue({
      response: { status: 401 },
    });

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.unauthorized(
      [{ message: 'Authentication required. Please log in again.' }],
      'Authentication required'
    ));
  });

  it('handles 400 Bad Request', async () => {
    api.get.mockRejectedValue({
      response: {
        status: 400,
        data: { errors: [{ message: 'Invalid request' }], successMessage: 'Bad input' },
      },
    });

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Invalid request' }],
      'Bad input'
    ));
  });

  it('handles unexpected errors', async () => {
    api.get.mockRejectedValue(new Error('Network error'));

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.serverError(
      [{ message: 'Network error' }],
      'An unexpected error occurred while retrieving Recommendations.'
    ));
  });

  it('includes correct headers in API call', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: { success: true, result: [] },
    });

    await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    const [, config] = api.get.mock.calls[0];
    expect(config.headers).toEqual({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  });

  it('returns result data directly from response', async () => {
    const mockData = { recommendations: [], totalCount: 0 };

    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: mockData
      },
    });

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(result.result).toEqual(mockData);
  });

  it('handles array error format in bad request path', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: false,
        errors: [{ message: 'Validation error 1' }, { message: 'Validation error 2' }]
      },
    });

    const result = await getRecommendationsByUserIdRepository.getRecommendationsByUserId();

    expect(result.errors).toEqual([
      { message: 'Validation error 1' },
      { message: 'Validation error 2' }
    ]);
  });
});
