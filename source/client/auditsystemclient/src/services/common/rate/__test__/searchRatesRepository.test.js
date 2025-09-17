// src/services/common/rate/__test__/searchRatesRepository.test.js
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { searchRatesRepository } from '../searchRatesRepository';
import authService from '@/services/auth/authRepos/authService';
import api from '@/services/api/api';
import { ApiResponseT } from '@/utils/ApiResponse';

vi.mock('@/services/auth/authRepos/authService', () => ({
  default: { getLocalToken: vi.fn() },
}));

vi.mock('@/services/api/api', () => ({
  default: { get: vi.fn() },
}));

describe('searchRatesRepository', () => {
  const token = 'test-token';

  beforeEach(() => {
    vi.clearAllMocks();
    authService.getLocalToken.mockReturnValue(token);
  });

  it('builds query params and returns successful response', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: {
        success: true,
        result: [{ id: 1, name: 'Rate1' }],
        successMessage: 'Rates retrieved successfully',
      },
    });

    const result = await searchRatesRepository.searchRates({
      searchTerm: 'test',
      pageNumber: 2,
      pageSize: 10,
    });

    expect(api.get).toHaveBeenCalledWith(
      '/api/Rate/search-rates?searchTerm=test&pageNumber=2&pageSize=10',
      expect.objectContaining({
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      })
    );

    expect(result).toEqual(
      ApiResponseT.ok([{ id: 1, name: 'Rate1' }], 'Rates retrieved successfully')
    );
  });

  it('returns badRequest when API responds with success=false', async () => {
    api.get.mockResolvedValue({
      status: 200,
      data: { success: false, message: 'Failed to retrieve rates.' },
    });

    const result = await searchRatesRepository.searchRates({});

    expect(result).toEqual(
      ApiResponseT.badRequest(
        [{ message: 'Failed to retrieve rates.' }],
        'Failed to retrieve rates.'
      )
    );
  });

  it('handles 401 Unauthorized', async () => {
    api.get.mockRejectedValue({
      response: { status: 401, data: {} },
    });

    const result = await searchRatesRepository.searchRates({});

    expect(result).toEqual(
      ApiResponseT.unauthorized(
        [{ message: 'Authentication required. Please log in again.' }],
        'Authentication required'
      )
    );
  });

  it('handles 400 Bad Request', async () => {
    api.get.mockRejectedValue({
      response: {
        status: 400,
        data: { message: 'Invalid request' },
      },
    });

    const result = await searchRatesRepository.searchRates({});

    expect(result).toEqual(
      ApiResponseT.badRequest(
        [{ message: 'Invalid request' }],
        'Invalid request'
      )
    );
  });

  it('handles unexpected errors', async () => {
    api.get.mockRejectedValue(new Error('Network error'));

    const result = await searchRatesRepository.searchRates({});

    expect(result).toEqual(
      ApiResponseT.serverError(
        [{ message: 'Network error' }],
        'Server error'
      )
    );
  });

  it('returns serverError when no token is found', async () => {
    authService.getLocalToken.mockReturnValue(null);

    const result = await searchRatesRepository.searchRates({});

    expect(result).toEqual(
      ApiResponseT.serverError(
        [{ message: 'Authentication token not found' }],
        'Server error'
      )
    );
  });
});