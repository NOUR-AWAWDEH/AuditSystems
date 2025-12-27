import { vi, describe, it, expect, beforeEach } from 'vitest';

vi.mock('@/services/api/api', () => ({
  default: { put: vi.fn() },
}));

vi.mock('@/services/auth/authRepos/authService', () => ({
  default: { getLocalToken: vi.fn() },
}));

import { updateRecommendationByIdRepository } from '@/services/riskControl/recommendation/updateRecommendationByIdRepository';

import api from '@/services/api/api';
import authService from '@/services/auth/authRepos/authService';
import { ApiResponseT } from '@/utils/ApiResponse';

describe('updateRecommendationByIdRepository', () => {
  const token = 'test-token';

  beforeEach(() => {
    vi.clearAllMocks();
    authService.getLocalToken.mockReturnValue(token);
  });

  it('successfully updates recommendation with primitive observationId', async () => {
    const mockResult = { id: 'rec-123', title: 'Updated Title' };

    api.put.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: mockResult,
        successMessage: 'Updated successfully'
      },
    });

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Updated Title',
      description: 'Updated Description',
      observationId: 5,
    });

    expect(api.put).toHaveBeenCalledWith(
      '/api/Recommendation/update-Recommendation-by-id',
      {
        id: 'rec-123',
        title: 'Updated Title',
        description: 'Updated Description',
        observationId: 5,
      },
      expect.any(Object)
    );
    expect(result).toEqual(ApiResponseT.ok(mockResult, 'Updated successfully'));
  });

  it('extracts observationId from object.value', async () => {
    api.put.mockResolvedValue({
      status: 200,
      data: { success: true, result: {} },
    });

    await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Title',
      description: 'Description',
      observationId: { value: 77 },
    });

    const [, body] = api.put.mock.calls[0];
    expect(body.observationId).toBe(77);
  });

  it('extracts observationId from object.id when value is not available', async () => {
    api.put.mockResolvedValue({
      status: 200,
      data: { success: true, result: {} },
    });

    await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Title',
      description: 'Description',
      observationId: { id: 88 },
    });

    const [, body] = api.put.mock.calls[0];
    expect(body.observationId).toBe(88);
  });

  it('uses observationId directly when it is not an object', async () => {
    api.put.mockResolvedValue({
      status: 200,
      data: { success: true, result: {} },
    });

    await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Title',
      description: 'Description',
      observationId: 99,
    });

    const [, body] = api.put.mock.calls[0];
    expect(body.observationId).toBe(99);
  });

  it('handles 200 status with success false as bad request', async () => {
    api.put.mockResolvedValue({
      status: 200,
      data: {
        success: false,
        successMessage: 'Update denied',
        errors: [{ message: 'Permission denied' }]
      },
    });

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Permission denied' }],
      'Update denied'
    ));
  });

  it('handles 200 status with missing success field as success', async () => {
    const mockResult = { id: 'rec-456', title: 'Updated' };

    api.put.mockResolvedValue({
      status: 200,
      data: {
        result: mockResult,
        successMessage: 'Updated'
      },
    });

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-456',
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    expect(result).toEqual(ApiResponseT.ok(mockResult, 'Updated'));
  });

  it('handles 401 Unauthorized', async () => {
    api.put.mockRejectedValue({
      response: { status: 401 },
    });

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    expect(result).toEqual(ApiResponseT.unauthorized(
      [{ message: 'Authentication required. Please log in again.' }],
      'Authentication required'
    ));
  });

  it('handles 400 Bad Request', async () => {
    api.put.mockRejectedValue({
      response: {
        status: 400,
        data: { errors: [{ message: 'Invalid data' }], message: 'Bad input' },
      },
    });

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: '',
      description: '',
      observationId: null,
    });

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Invalid data' }],
      'Bad input'
    ));
  });

  it('handles 404 Not Found', async () => {
    api.put.mockRejectedValue({
      response: { status: 404 },
    });

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'non-existent-id',
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    expect(result).toEqual(ApiResponseT.notFound(
      [{ message: 'Recommendation not found' }],
      'Recommendation not found'
    ));
  });

  it('handles unexpected errors', async () => {
    api.put.mockRejectedValue(new Error('Network error'));

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    expect(result).toEqual(ApiResponseT.serverError(
      [{ message: 'Network error' }],
      'An unexpected error occurred while updating Recommendation.'
    ));
  });

  it('includes correct headers in API call', async () => {
    api.put.mockResolvedValue({
      status: 200,
      data: { success: true, result: {} },
    });

    await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    const [, , config] = api.put.mock.calls[0];
    expect(config.headers).toEqual({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  });

  it('returns result data directly from response', async () => {
    const mockData = { id: 'rec-789', title: 'Updated Title', updatedAt: '2024-01-01' };

    api.put.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: mockData
      },
    });

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-789',
      title: 'New Title',
      description: 'New Description',
      observationId: 1,
    });

    expect(result.result).toEqual(mockData);
  });

  it('handles array error format in bad request path', async () => {
    api.put.mockResolvedValue({
      status: 200,
      data: {
        success: false,
        errors: [{ message: 'Validation error 1' }, { message: 'Validation error 2' }]
      },
    });

    const result = await updateRecommendationByIdRepository.updateRecommendationById({
      id: 'rec-123',
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    expect(result.errors).toEqual([
      { message: 'Validation error 1' },
      { message: 'Validation error 2' }
    ]);
  });
});
