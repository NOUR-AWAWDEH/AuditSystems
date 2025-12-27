import { useAuthStatus } from '@/composables/auth/useAuthStatus';

/**
 * Composable to check if user has required roles
 * @param {string|string[]} requiredRoles - Role(s) required to access
 * @returns {Promise<boolean>} - True if user has required role(s)
 */
export async function useRouteGuard(requiredRoles) {
  const { checkAuthStatus, authStatus } = useAuthStatus();
  
  // Ensure we have the latest auth status
  await checkAuthStatus();
  
  if (!authStatus.value?.roles) {
    return false;
  }
  
  const roles = Array.isArray(requiredRoles) ? requiredRoles : [requiredRoles];
  return roles.some((role) => authStatus.value.roles.includes(role));
}

/**
 * Check if user has a specific role
 * @param {string} role - Role to check
 * @returns {boolean} - True if user has the role
 */
export function hasRole(role) {
  const { authStatus } = useAuthStatus();
  return authStatus.value?.roles?.includes(role) ?? false;
}

/**
 * Get all user roles
 * @returns {string[]} - Array of user roles
 */
export function getUserRoles() {
  const { authStatus } = useAuthStatus();
  return authStatus.value?.roles ?? [];
}
