import { vi, describe, it, expect, beforeEach } from 'vitest';

vi.mock('@/services/api/api', () => ({
  default: { post: vi.fn() },
}));

vi.mock('@/services/auth/authRepos/authService', () => ({
  default: { getLocalToken: vi.fn() },
}));

import { createObjectiveRepository } from '@/services/riskControl/objective/createObjectiveRepository';

import api from '@/services/api/api';
import authService from '@/services/auth/authRepos/authService';
import { ApiResponseT } from '@/utils/ApiResponse';

describe('createObjectiveRepository', () => {
  const token = 'test-token';

  beforeEach(() => {
    vi.clearAllMocks();
    authService.getLocalToken.mockReturnValue(token);
  });

  it('sends correct payload for primitive riskControlMatrixId', async () => {
    api.post.mockResolvedValue({
      status: 201,
      data: { success: true, result: { id: 1 }, successMessage: 'Created!' },
    });

    const result = await createObjectiveRepository.createObjective({
      description: 'Test',
      riskControlMatrixId: 5,
    });

    expect(api.post).toHaveBeenCalledWith(
      '/api/Objective/create-objective',
      { description: 'Test', riskControlMatrixId: 5 },
      expect.any(Object)
    );
    expect(result).toEqual(ApiResponseT.ok({ id: 1 }, 'Created!'));
  });

  it('extracts riskControlMatrixId from object.value', async () => {
    api.post.mockResolvedValue({
      status: 201,
      data: { success: true, result: {}, successMessage: 'ok' },
    });

    await createObjectiveRepository.createObjective({
      description: 'desc',
      riskControlMatrixId: { value: 77 },
    });

    const [, body] = api.post.mock.calls[0];
    expect(body.riskControlMatrixId).toBe(77);
  });

  it('handles 400 Bad Request', async () => {
    api.post.mockRejectedValue({
      response: {
        status: 400,
        data: { errors: [{ message: 'Invalid data' }], successMessage: 'Bad input' },
      },
    });

    const result = await createObjectiveRepository.createObjective({
      description: '',
      riskControlMatrixId: null,
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

    const result = await createObjectiveRepository.createObjective({
      description: 'desc',
      riskControlMatrixId: 1,
    });

    expect(result).toEqual(ApiResponseT.unauthorized(
      [{ message: 'Authentication required. Please log in again.' }],
      'Authentication required'
    ));
  });

  it('handles unexpected errors', async () => {
    api.post.mockRejectedValue(new Error('Network error'));

    const result = await createObjectiveRepository.createObjective({
      description: 'desc',
      riskControlMatrixId: 1,
    });

    expect(result).toEqual(ApiResponseT.serverError(
      [{ message: 'Network error' }],
      'An unexpected error occurred while creating Objective.'
    ));
  });
});
