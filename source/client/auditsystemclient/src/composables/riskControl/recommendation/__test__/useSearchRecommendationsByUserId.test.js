import { describe, it, expect, vi, beforeEach } from 'vitest';
import { useSearchRecommendationsByUserId } from '../useSearchRecommendationsByUserId';
import { searchRecommendationsByUserIdRepository } from '@/services/riskControl/recommendation/searchRecommendationsByUserIdRepository';

// 🔹 Mock the repository
vi.mock('@/services/riskControl/recommendation/searchRecommendationsByUserIdRepository', () => ({
  searchRecommendationsByUserIdRepository: {
    searchRecommendationsByUserId: vi.fn(),
  },
}));

describe('useSearchRecommendationsByUserId', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('searches recommendations successfully with basic parameters', async () => {
    const mockResponse = {
      success: true,
      result: {
        items: [
          { id: 1, title: 'Recommendation 1' },
          { id: 2, title: 'Recommendation 2' },
        ],
        totalCount: 2,
      },
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      searchRecommendations,
      searchResults,
      totalCount,
      error,
      isSearching,
    } = useSearchRecommendationsByUserId();

    const result = await searchRecommendations({
      searchTerm: 'test',
      pageNumber: 1,
      pageSize: 10,
    });

    expect(isSearching.value).toBe(false);
    expect(error.value).toBe(null);
    expect(searchResults.value).toEqual(mockResponse.result.items);
    expect(totalCount.value).toBe(2);

    expect(result).toEqual({
      success: true,
      data: mockResponse.result,
    });

    expect(searchRecommendationsByUserIdRepository.searchRecommendationsByUserId)
      .toHaveBeenCalledWith('test', 1, 10, null);
  });

  it('searches with observationId parameter', async () => {
    const mockResponse = {
      success: true,
      result: {
        items: [{ id: 1, title: 'Recommendation 1' }],
        totalCount: 1,
      },
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const { searchRecommendations } = useSearchRecommendationsByUserId();

    await searchRecommendations({
      searchTerm: 'test',
      observationId: 'obs-123',
      pageNumber: 1,
      pageSize: 5,
    });

    expect(searchRecommendationsByUserIdRepository.searchRecommendationsByUserId)
      .toHaveBeenCalledWith('test', 1, 5, 'obs-123');
  });

  it('uses default values when no parameters provided', async () => {
    const mockResponse = {
      success: true,
      result: {
        items: [],
        totalCount: 0,
      },
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const { searchRecommendations } = useSearchRecommendationsByUserId();

    await searchRecommendations();

    expect(searchRecommendationsByUserIdRepository.searchRecommendationsByUserId)
      .toHaveBeenCalledWith('', 1, 10, null);
  });

  it('updates reactive state values when parameters provided', async () => {
    const mockResponse = {
      success: true,
      result: {
        items: [{ id: 1 }],
        totalCount: 1,
      },
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      searchRecommendations,
      searchTerm,
      observationId,
      currentPage,
      pageSize,
    } = useSearchRecommendationsByUserId();

    await searchRecommendations({
      searchTerm: 'updated search',
      observationId: 'obs-updated',
      pageNumber: 2,
      pageSize: 20,
    });

    expect(searchTerm.value).toBe('updated search');
    expect(observationId.value).toBe('obs-updated');
    expect(currentPage.value).toBe(2);
    expect(pageSize.value).toBe(20);
  });

  it('trims whitespace from search term', async () => {
    const mockResponse = {
      success: true,
      result: {
        items: [],
        totalCount: 0,
      },
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const { searchRecommendations } = useSearchRecommendationsByUserId();

    await searchRecommendations({
      searchTerm: '  test search  ',
    });

    expect(searchRecommendationsByUserIdRepository.searchRecommendationsByUserId)
      .toHaveBeenCalledWith('test search', 1, 10, null);
  });

  it('handles API failure response with errors array', async () => {
    const mockResponse = {
      success: false,
      errors: [{ message: 'Search failed' }],
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      searchRecommendations,
      searchResults,
      error,
      isSearching,
      totalCount,
    } = useSearchRecommendationsByUserId();

    const result = await searchRecommendations();

    expect(isSearching.value).toBe(false);
    expect(searchResults.value).toEqual([]);
    expect(totalCount.value).toBe(0);
    expect(error.value).toBe('Search failed');

    expect(result).toEqual({
      success: false,
      message: 'Search failed',
      errors: [{ message: 'Search failed' }],
    });
  });

  it('handles API failure response with message fallback', async () => {
    const mockResponse = {
      success: false,
      message: 'Custom error message',
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const { searchRecommendations, error } = useSearchRecommendationsByUserId();

    const result = await searchRecommendations();

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

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const { searchRecommendations, error } = useSearchRecommendationsByUserId();

    const result = await searchRecommendations();

    expect(error.value).toBe('Failed to search Recommendations');

    expect(result).toEqual({
      success: false,
      message: 'Failed to search Recommendations',
      errors: [{ message: 'Failed to search Recommendations' }],
    });
  });

  it('handles thrown exception', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {});

    const mockError = new Error('Network error');
    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockRejectedValue(mockError);

    const {
      searchRecommendations,
      searchResults,
      error,
      isSearching,
      totalCount,
    } = useSearchRecommendationsByUserId();

    const result = await searchRecommendations();

    expect(isSearching.value).toBe(false);
    expect(searchResults.value).toEqual([]);
    expect(totalCount.value).toBe(0);
    expect(error.value).toBe('Network error');

    expect(result).toEqual({
      success: false,
      message: 'Network error',
      errors: [{ message: 'Network error' }],
    });
  });

  it('handles thrown exception with default message', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {});

    const mockError = { message: '' };
    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockRejectedValue(mockError);

    const { searchRecommendations, error } = useSearchRecommendationsByUserId();

    const result = await searchRecommendations();

    expect(error.value).toBe('Failed to search Recommendations');

    expect(result).toEqual({
      success: false,
      message: 'Failed to search Recommendations',
      errors: [{ message: 'Failed to search Recommendations' }],
    });
  });

  it('sets loading state correctly during search', async () => {
    const mockResponse = {
      success: true,
      result: {
        items: [{ id: 1 }],
        totalCount: 1,
      },
    };

    let resolvePromise;
    const promise = new Promise((resolve) => {
      resolvePromise = resolve;
    });

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockReturnValue(promise);

    const { searchRecommendations, isSearching } = useSearchRecommendationsByUserId();

    const searchPromise = searchRecommendations();

    expect(isSearching.value).toBe(true);

    resolvePromise(mockResponse);

    await searchPromise;

    expect(isSearching.value).toBe(false);
  });

  it('clears error state before new search', async () => {
    const mockResponse = {
      success: true,
      result: {
        items: [{ id: 1 }],
        totalCount: 1,
      },
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const { searchRecommendations, error } = useSearchRecommendationsByUserId();

    // Set initial error state
    error.value = 'Previous error';

    await searchRecommendations();

    expect(error.value).toBe(null);
  });

  it('clears search state with clearSearch function', () => {
    const {
      clearSearch,
      searchResults,
      searchTerm,
      observationId,
      currentPage,
      error,
      totalCount,
      pageSize,
    } = useSearchRecommendationsByUserId();

    // Set some state
    searchResults.value = [{ id: 1 }];
    searchTerm.value = 'test';
    observationId.value = 'obs-123';
    currentPage.value = 5;
    error.value = 'some error';
    totalCount.value = 10;
    pageSize.value = 20;

    clearSearch();

    expect(searchResults.value).toEqual([]);
    expect(searchTerm.value).toBe('');
    expect(observationId.value).toBe(null);
    expect(currentPage.value).toBe(1);
    expect(error.value).toBe(null);
    expect(totalCount.value).toBe(0);
    expect(pageSize.value).toBe(10);
  });

  it('resets all state with resetState function', () => {
    const {
      resetState,
      searchResults,
      isSearching,
      error,
      totalCount,
      currentPage,
      searchTerm,
      observationId,
    } = useSearchRecommendationsByUserId();

    // Set some state
    searchResults.value = [{ id: 1 }];
    isSearching.value = true;
    error.value = 'some error';
    totalCount.value = 10;
    currentPage.value = 5;
    searchTerm.value = 'test';
    observationId.value = 'obs-123';

    resetState();

    expect(searchResults.value).toEqual([]);
    expect(isSearching.value).toBe(false);
    expect(error.value).toBe(null);
    expect(totalCount.value).toBe(0);
    expect(currentPage.value).toBe(1);
    expect(searchTerm.value).toBe('');
    expect(observationId.value).toBe(null);
  });

  it('handles empty result items gracefully', async () => {
    const mockResponse = {
      success: true,
      result: {
        items: null, // Invalid items
        totalCount: 0,
      },
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const { searchRecommendations, searchResults } = useSearchRecommendationsByUserId();

    await searchRecommendations();

    expect(searchResults.value).toEqual([]);
  });

  it('handles missing result object gracefully', async () => {
    const mockResponse = {
      success: true,
      result: null,
    };

    searchRecommendationsByUserIdRepository.searchRecommendationsByUserId.mockResolvedValue(mockResponse);

    const {
      searchRecommendations,
      searchResults,
      totalCount,
    } = useSearchRecommendationsByUserId();

    const result = await searchRecommendations();

    expect(searchResults.value).toEqual([]);
    expect(totalCount.value).toBe(0);
    expect(result).toEqual({
      success: true,
      data: null,
    });
  });

  it('returns all expected reactive refs and functions', () => {
    const composable = useSearchRecommendationsByUserId();

    expect(composable).toHaveProperty('searchResults');
    expect(composable).toHaveProperty('isSearching');
    expect(composable).toHaveProperty('error');
    expect(composable).toHaveProperty('totalCount');
    expect(composable).toHaveProperty('currentPage');
    expect(composable).toHaveProperty('pageSize');
    expect(composable).toHaveProperty('searchTerm');
    expect(composable).toHaveProperty('observationId');
    expect(composable).toHaveProperty('searchRecommendations');
    expect(composable).toHaveProperty('clearSearch');
    expect(composable).toHaveProperty('resetState');

    // Check that they are Vue refs
    expect(typeof composable.searchResults.value).not.toBe('undefined');
    expect(typeof composable.isSearching.value).toBe('boolean');
    expect(typeof composable.error.value).toBe('object'); // can be null
    expect(typeof composable.totalCount.value).toBe('number');
    expect(typeof composable.currentPage.value).toBe('number');
    expect(typeof composable.pageSize.value).toBe('number');
    expect(typeof composable.searchTerm.value).toBe('string');
    expect(typeof composable.observationId.value).toBe('object'); // can be null

    // Check that functions are functions
    expect(typeof composable.searchRecommendations).toBe('function');
    expect(typeof composable.clearSearch).toBe('function');
    expect(typeof composable.resetState).toBe('function');
  });
});
