/**
 * Formats a date string to a localized date string
 * @param {string|Date} dateStr - The date string or Date object to format
 * @param {Object} options - Optional formatting options for toLocaleDateString
 * @returns {string} Formatted date string or fallback value
 */
export function formatDate(dateStr, options = {}) {
  if (!dateStr) return '—';

  try {
    const date = new Date(dateStr);

    if (isNaN(date.getTime())) {
      return 'Invalid Date';
    }

    // Default options if none provided
    const defaultOptions = {
      year: 'numeric',
      month: 'short',
      day: '2-digit',
      ...options
    };

    return date.toLocaleDateString('en-GB', defaultOptions);
  } catch (error) {
    console.error('Error formatting date:', error);
    return 'Invalid Date';
  }
}

/**
 * Simple date formatter using default locale
 * @param {string|Date} dateStr - The date string or Date object to format
 * @returns {string} Formatted date string
 */
export function formatDateSimple(dateStr) {
  if (!dateStr) return '—';

  try {
    const date = new Date(dateStr);
    if (isNaN(date.getTime())) return 'Invalid Date';
    return date.toLocaleDateString();
  } catch {
    return 'Invalid Date';
  }
}
