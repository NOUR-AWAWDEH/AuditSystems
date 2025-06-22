// Navigation configuration for the sidebar
import DashboardIcon from '@/components/common/sidebar/icons/DashboardIcon.vue'
import DocumentIcon from '@/components/common/sidebar/icons/DocumentIcon.vue'

// Define all navigation items
const navigationItems = [
  {
    route: '/dashboard',
    label: 'Dashboard',
    icon: DashboardIcon,
    requiresAuth: true
  },
  {
    route: '/supporting-docs',
    label: 'Supporting Documents',
    icon: DocumentIcon,
    requiresAuth: true
  }
]

// Function to get visible navigation items based on authentication status
export function getVisibleNavigationItems(isAuthenticated) {
  if (!isAuthenticated) {
    return []
  }
  
  return navigationItems.filter(item => {
    // Only show items that require auth when user is authenticated
    return !item.requiresAuth || isAuthenticated
  })
}

// Export navigation items for other uses
export { navigationItems }