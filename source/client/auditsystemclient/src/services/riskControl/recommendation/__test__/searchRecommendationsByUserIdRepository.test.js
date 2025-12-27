import { vi, describe, it, expect, beforeEach } from 'vitest';

vi.mock('@/services/api/api', () => ({
  default: { get: vi.fn() },
}));

vi.mock('@/services/auth/authRepos/authService', () => ({
  default: { getLocalToken: vi.fn() },
}));

import { searchRecommendationsByUserIdRepository } from '@/services/riskControl/recommendation/searchRecommendationsByUserIdRepository';

import api from '@/services/api/api';
import authService from '@/services/auth/authRepos/authService';
import { ApiResponseT } from '@/utils/ApiResponse';

describe('searchRecommendationsByUserIdRepository', () => {
  const token = 'test-token';

  beforeEach(() => {
    vi.clearAllMocks();
    authService.getLocalToken.mockReturnValue(token);
  });

  it('successfully searches recommendations with valid response', async () => {
    const mockItems = [
      { id: 'rec-1', title: 'Recommendation 1', observationId: 'obs-1' },
      { id: 'rec-2', title: 'Recommendation 2', observationId: 'obs-2' }
    ];

    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: {
          items: mockItems,
          totalCount: 2
        },
        successMessage: 'Search completed'
      },
    });

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId(
      'test search',
      1,
      10,
      'obs-123'
    );

    expect(api.get).toHaveBeenCalledWith(
      '/api/Recommendation/search-recommendations-by-user-id',
      expect.objectContaining({
        params: {
          searchTerm: 'test search',
          observationId: 'obs-123',
          pageNumber: 1,
          pageSize: 10
        },
        headers: expect.any(Object)
      })
    );
    expect(result).toEqual(ApiResponseT.ok({
      items: mockItems,
      totalCount: 2
    }, 'Search completed'));
  });

  it('handles totalCount correction when totalCount is 0 but items exist', async () => {
    const mockItems = [
      { id: 'rec-1', title: 'Recommendation 1' },
      { id: 'rec-2', title: 'Recommendation 2' }
    ];

    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: {
          items: mockItems,
          totalCount: 0  // Incorrect totalCount
        }
      },
    });

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    expect(result.result.totalCount).toBe(2); // Should be corrected to items.length
  });

  it('preserves correct totalCount when it matches items length', async () => {
    const mockItems = [
      { id: 'rec-1', title: 'Recommendation 1' }
    ];

    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: {
          items: mockItems,
          totalCount: 1
        }
      },
    });

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    expect(result.result.totalCount).toBe(1);
  });

  it('handles empty search term', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: {
          items: [],
          totalCount: 0
        }
      },
    });

    await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId('', 1, 10, '');

    const [, config] = api.get.mock.calls[0];
    expect(config.params.searchTerm).toBe('');
  });

  it('handles whitespace-only search term', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: {
          items: [],
          totalCount: 0
        }
      },
    });

    await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId('   ', 1, 10, '');

    const [, config] = api.get.mock.calls[0];
    expect(config.params.searchTerm).toBe('');
  });

  it('returns bad request for missing items array', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: {
          totalCount: 5
          // missing items array
        }
      },
    });

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Missing or invalid result structure.' }],
      'Invalid response format'
    ));
  });

  it('returns bad request for non-numeric totalCount', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: {
          items: [],
          totalCount: 'invalid'
        }
      },
    });

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Missing or invalid result structure.' }],
      'Invalid response format'
    ));
  });

  it('returns bad request for success false', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: false,
        result: {
          items: [],
          totalCount: 0
        }
      },
    });

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Unexpected response format.' }],
      'Failed to search Recommendations.'
    ));
  });

  it('handles 401 Unauthorized', async () => {
    api.get.mockRejectedValue({
      response: { status: 401 },
    });

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.unauthorized(
      [{ message: 'Authentication required. Please log in again.' }],
      'Unauthorized'
    ));
  });

  it('handles 400 Bad Request', async () => {
    api.get.mockRejectedValue({
      response: {
        status: 400,
        data: { errors: [{ message: 'Invalid search parameters' }], message: 'Bad input' },
      },
    });

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.badRequest(
      [{ message: 'Invalid search parameters' }],
      'Bad input'
    ));
  });

  it('handles unexpected errors', async () => {
    api.get.mockRejectedValue(new Error('Network error'));

    const result = await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    expect(result).toEqual(ApiResponseT.serverError(
      [{ message: 'Network error' }],
      'An unexpected error occurred while searching Recommendations.'
    ));
  });

  it('includes correct headers in API call', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: { items: [], totalCount: 0 }
      },
    });

    await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    const [, config] = api.get.mock.calls[0];
    expect(config.headers).toEqual({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });
  });

  it('uses default parameter values when not provided', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: { items: [], totalCount: 0 }
      },
    });

    await searchRecommendationsByUserIdRepository.searchRecommendationsByUserId();

    const [, config] = api.get.mock.calls[0];
    expect(config.params).toEqual({
      searchTerm: '',
      observationId: '',
      pageNumber: 1,
      pageSize: 10
    });
  });
});
