import { vi, describe, it, expect, beforeEach } from 'vitest';

vi.mock('@/services/api/api', () => ({
  default: { delete: vi.fn() },
}));

vi.mock('@/services/auth/authRepos/authService', () => ({
  default: { getLocalToken: vi.fn() },
}));

import { deleteRecommendationByIdRepository } from '@/services/riskControl/recommendation/deleteRecommendationByIdRepository';

import api from '@/services/api/api';
import authService from '@/services/auth/authRepos/authService';
import { ApiResponseT } from '@/utils/ApiResponse';

describe('deleteRecommendationByIdRepository', () => {
  const token = 'test-token';

  beforeEach(() => {
    vi.clearAllMocks();
    authService.getLocalToken.mockReturnValue(token);
  });

  it('successfully deletes recommendation with 204 status', async () => {
    api.delete.mockResolvedValue({
      status: 204,
    });

    const result = await deleteRecommendationByIdRepository.deleteRecommendationById('rec-123');

    expect(api.delete).toHaveBeenCalledWith(
      '/api/Recommendation/delete-recommendation-by-id',
      expect.objectContaining({
        data: { id: 'rec-123' },
        headers: expect.any(Object)
      })
    );
    expect(result).toEqual(ApiResponseT.ok(null, 'Recommendation deleted successfully'));
  });

  it('successfully deletes recommendation with 200 status and success true', async () => {
    api.delete.mockResolvedValue({
      status: 200,
      data: { success: true, successMessage: 'Deleted successfully' },
    });

    const result = await deleteRecommendationByIdRepository.deleteRecommendationById('rec-456');

    expect(result).toEqual(ApiResponseT.ok(null, 'Recommendation deleted successfully'));
  });

  it('handles 200 status with success false as bad request', async () => {
    api.delete.mockResolvedValue({
      status: 200,
      data: { success: false, successMessage: 'Delete failed', errors: [{ message: 'Permission denied' }] },
    });

    const result = await deleteRecommendationByIdRepository.deleteRecommendationById('rec-789');

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Permission denied' }],
      'Delete failed'
    ));
  });

  it('handles 400 Bad Request', async () => {
    api.delete.mockRejectedValue({
      response: {
        status: 400,
        data: { errors: [{ message: 'Invalid recommendation ID' }], successMessage: 'Bad input' },
      },
    });

    const result = await deleteRecommendationByIdRepository.deleteRecommendationById('invalid-id');

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Invalid recommendation ID' }],
      'Bad input'
    ));
  });

  it('handles 401 Unauthorized', async () => {
    api.delete.mockRejectedValue({
      response: { status: 401 },
    });

    const result = await deleteRecommendationByIdRepository.deleteRecommendationById('rec-123');

    expect(result).toEqual(ApiResponseT.unauthorized(
      [{ message: 'Authentication required. Please log in again.' }],
      'Authentication required'
    ));
  });

  it('handles 404 Not Found', async () => {
    api.delete.mockRejectedValue({
      response: { status: 404 },
    });

    const result = await deleteRecommendationByIdRepository.deleteRecommendationById('non-existent-id');

    expect(result).toEqual(ApiResponseT.notFound(
      [{ message: 'Recommendation not found' }],
      'Recommendation not found'
    ));
  });

  it('handles unexpected errors', async () => {
    api.delete.mockRejectedValue(new Error('Network error'));

    const result = await deleteRecommendationByIdRepository.deleteRecommendationById('rec-123');

    expect(result).toEqual(ApiResponseT.serverError(
      [{ message: 'Network error' }],
      'An unexpected error occurred while deleting Recommendation.'
    ));
  });

  it('includes correct headers in API call', async () => {
    api.delete.mockResolvedValue({
      status: 204,
    });

    await deleteRecommendationByIdRepository.deleteRecommendationById('rec-123');

    const [, config] = api.delete.mock.calls[0];
    expect(config.headers).toEqual({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  });

  it('sends recommendation ID in request body', async () => {
    api.delete.mockResolvedValue({
      status: 204,
    });

    await deleteRecommendationByIdRepository.deleteRecommendationById('test-id-123');

    const [, config] = api.delete.mock.calls[0];
    expect(config.data).toEqual({ id: 'test-id-123' });
  });
});
