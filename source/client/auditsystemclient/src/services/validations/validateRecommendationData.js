export const validateUpdateRecommendation = (recommendationData) => {
  if (!recommendationData.id) return "Recommendation ID is required";
  if (!recommendationData.description?.trim()) return "Recommendation description is required";
  if (!recommendationData.observationId) return "Observation ID is required";

  return null;
};
