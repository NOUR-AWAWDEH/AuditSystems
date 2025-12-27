import { vi, describe, it, expect, beforeEach } from 'vitest';

vi.mock('@/services/api/api', () => ({
  default: { post: vi.fn() },
}));

vi.mock('@/services/auth/authRepos/authService', () => ({
  default: { getLocalToken: vi.fn() },
}));

import { createRecommendationRepository } from '@/services/riskControl/recommendation/createRecommendationRepository';

import api from '@/services/api/api';
import authService from '@/services/auth/authRepos/authService';
import { ApiResponseT } from '@/utils/ApiResponse';

describe('createRecommendationRepository', () => {
  const token = 'test-token';

  beforeEach(() => {
    vi.clearAllMocks();
    authService.getLocalToken.mockReturnValue(token);
  });

  it('sends correct payload for primitive observationId', async () => {
    api.post.mockResolvedValue({
      status: 201,
      data: { success: true, result: { id: 1 }, successMessage: 'Created!' },
    });

    const result = await createRecommendationRepository.createRecommendation({
      title: 'Test Title',
      description: 'Test Description',
      observationId: 5,
    });

    expect(api.post).toHaveBeenCalledWith(
      '/api/Recommendation/create-recommendation',
      { title: 'Test Title', description: 'Test Description', observationId: 5 },
      expect.any(Object)
    );
    expect(result).toEqual(ApiResponseT.ok({ id: 1 }, 'Created!'));
  });

  it('extracts observationId from object.value', async () => {
    api.post.mockResolvedValue({
      status: 201,
      data: { success: true, result: {}, successMessage: 'ok' },
    });

    await createRecommendationRepository.createRecommendation({
      title: 'Title',
      description: 'Description',
      observationId: { value: 77 },
    });

    const [, body] = api.post.mock.calls[0];
    expect(body.observationId).toBe(77);
  });

  it('extracts observationId from object.id when value is not available', async () => {
    api.post.mockResolvedValue({
      status: 201,
      data: { success: true, result: {}, successMessage: 'ok' },
    });

    await createRecommendationRepository.createRecommendation({
      title: 'Title',
      description: 'Description',
      observationId: { id: 88 },
    });

    const [, body] = api.post.mock.calls[0];
    expect(body.observationId).toBe(88);
  });

  it('uses observationId directly when it is not an object', async () => {
    api.post.mockResolvedValue({
      status: 201,
      data: { success: true, result: {}, successMessage: 'ok' },
    });

    await createRecommendationRepository.createRecommendation({
      title: 'Title',
      description: 'Description',
      observationId: 99,
    });

    const [, body] = api.post.mock.calls[0];
    expect(body.observationId).toBe(99);
  });

  it('handles 400 Bad Request', async () => {
    api.post.mockRejectedValue({
      response: {
        status: 400,
        data: { errors: [{ message: 'Invalid data' }], successMessage: 'Bad input' },
      },
    });

    const result = await createRecommendationRepository.createRecommendation({
      title: '',
      description: '',
      observationId: null,
    });

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Invalid data' }],
      'Bad input'
    ));
  });

  it('handles 401 Unauthorized', async () => {
    api.post.mockRejectedValue({
      response: { status: 401 },
    });

    const result = await createRecommendationRepository.createRecommendation({
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    expect(result).toEqual(ApiResponseT.unauthorized(
      [{ message: 'Authentication required. Please log in again.' }],
      'Authentication required'
    ));
  });

  it('handles unexpected errors', async () => {
    api.post.mockRejectedValue(new Error('Network error'));

    const result = await createRecommendationRepository.createRecommendation({
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    expect(result).toEqual(ApiResponseT.serverError(
      [{ message: 'Network error' }],
      'An unexpected error occurred while creating Recommendation.'
    ));
  });

  it('includes correct headers in API call', async () => {
    api.post.mockResolvedValue({
      status: 201,
      data: { success: true, result: {} },
    });

    await createRecommendationRepository.createRecommendation({
      title: 'Title',
      description: 'Description',
      observationId: 1,
    });

    const [, , config] = api.post.mock.calls[0];
    expect(config.headers).toEqual({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  });
});
