import { describe, it, expect } from 'vitest';
import { validateUpdateRecommendation } from '../validateRecommendationData';

describe('validateUpdateRecommendation', () => {
  it('returns null for valid recommendation data', () => {
    const validData = {
      id: 'rec-123',
      title: 'Valid Title',
      description: 'Valid Description',
      observationId: 5,
    };

    expect(validateUpdateRecommendation(validData)).toBe(null);
  });

  it('returns error when recommendation ID is missing', () => {
    const data = {
      title: 'Valid Title',
      description: 'Valid Description',
      observationId: 5,
    };

    expect(validateUpdateRecommendation(data)).toBe('Recommendation ID is required');
  });

  it('returns error when title is missing', () => {
    const data = {
      id: 'rec-123',
      title: '',
      description: 'Valid Description',
      observationId: 5,
    };

    expect(validateUpdateRecommendation(data)).toBe('Recommendation title is required');
  });

  it('returns error when title is only whitespace', () => {
    const data = {
      id: 'rec-123',
      title: '   ',
      description: 'Valid Description',
      observationId: 5,
    };

    expect(validateUpdateRecommendation(data)).toBe('Recommendation title is required');
  });

  it('returns error when description is missing', () => {
    const data = {
      id: 'rec-123',
      title: 'Valid Title',
      description: '',
      observationId: 5,
    };

    expect(validateUpdateRecommendation(data)).toBe('Recommendation description is required');
  });

  it('returns error when description is only whitespace', () => {
    const data = {
      id: 'rec-123',
      title: 'Valid Title',
      description: '   ',
      observationId: 5,
    };

    expect(validateUpdateRecommendation(data)).toBe('Recommendation description is required');
  });

  it('returns error when observationId is missing', () => {
    const data = {
      id: 'rec-123',
      title: 'Valid Title',
      description: 'Valid Description',
      observationId: null,
    };

    expect(validateUpdateRecommendation(data)).toBe('Observation ID is required');
  });

  it('returns error when observationId is undefined', () => {
    const data = {
      id: 'rec-123',
      title: 'Valid Title',
      description: 'Valid Description',
    };

    expect(validateUpdateRecommendation(data)).toBe('Observation ID is required');
  });
});
