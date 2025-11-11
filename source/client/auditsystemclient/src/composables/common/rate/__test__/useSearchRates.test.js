import { describe, it, expect, vi, beforeEach } from 'vitest';
import { useSearchRates } from '../useSearchRates';
import { searchRatesRepository } from '@/services/common/rate/searchRatesRepository';

vi.mock('@/services/common/rate/searchRatesRepository', () => ({
  searchRatesRepository: { searchRates: vi.fn() },
}));

vi.mock('@/composables/notification/useNotification', () => ({
  useNotification: () => ({ showNotification: vi.fn() }),
}));

describe('useSearchRates', () => {
  let composable;

  beforeEach(() => {
    composable = useSearchRates();
    vi.clearAllMocks();
  });

  it('has correct initial state', () => {
    expect(composable.searchResults.value).toEqual([]);
    expect(composable.isSearching.value).toBe(false);
    expect(composable.error.value).toBeNull();
    expect(composable.totalCount.value).toBe(0);
    expect(composable.currentPage.value).toBe(1);
    expect(composable.pageSize.value).toBe(10);
    expect(composable.searchTerm.value).toBe('');
  });

  it('handles successful search', async () => {
    searchRatesRepository.searchRates.mockResolvedValue({
      success: true,
      result: { items: [{ id: 1, name: 'Rate1' }], totalCount: 5 },
    });

    const result = await composable.searchRates({ searchTerm: 'test' });

    expect(result.success).toBe(true);
    expect(composable.searchResults.value).toEqual([{ id: 1, name: 'Rate1' }]);
    expect(composable.totalCount.value).toBe(5);
    expect(composable.error.value).toBeNull();
  });

  it('handles failed search', async () => {
    searchRatesRepository.searchRates.mockResolvedValue({
      success: false,
      errors: [{ message: 'Something went wrong' }],
    });

    const result = await composable.searchRates({});
    expect(result.success).toBe(false);
    expect(composable.error.value).toBe('Something went wrong');
  });

  it('handles thrown error', async () => {
    searchRatesRepository.searchRates.mockRejectedValue(new Error('Network error'));

    const result = await composable.searchRates({});
    expect(result.success).toBe(false);
    expect(composable.error.value).toBe('Network error');
  });

  it('clearSearch resets state', () => {
    composable.searchResults.value = [{ id: 1 }];
    composable.error.value = 'Some error';

    composable.clearSearch();

    expect(composable.searchResults.value).toEqual([]);
    expect(composable.error.value).toBeNull();
    expect(composable.currentPage.value).toBe(1);
    expect(composable.searchTerm.value).toBe('');
  });

  it('resetState resets everything', () => {
    composable.searchResults.value = [{ id: 1 }];
    composable.error.value = 'Some error';
    composable.isSearching.value = true;

    composable.resetState();

    expect(composable.searchResults.value).toEqual([]);
    expect(composable.error.value).toBeNull();
    expect(composable.isSearching.value).toBe(false);
    expect(composable.currentPage.value).toBe(1);
    expect(composable.searchTerm.value).toBe('');
  });
});
