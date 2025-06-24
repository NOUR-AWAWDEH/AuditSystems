import DashboardIcon from '@/components/common/sidebar/icons/DashboardIcon.vue'
import DocumentIcon from '@/components/common/sidebar/icons/DocumentIcon.vue'
import LocationIcon from '@/components/common/sidebar/icons/LocationIcon.vue'

// Navigation items that correspond to your router.js routes
export const navigationItems = [
  {
    route: '/dashboard',
    name: 'Dashboard', // matches router name
    label: 'Dashboard',
    icon: DashboardIcon,
    requiresAuth: true
  },
  {
    route: '/supporting-docs',
    name: 'SupportingDocs', // matches router name
    label: 'Supporting Docs',
    icon: DocumentIcon,
    requiresAuth: true
  },
  {
    route: '/locations',
    name: 'Locations', // matches router name
    label: 'Locations',
    icon: LocationIcon,
    requiresAuth: true
  }
]

// Filter navigation items based on authentication status
export const getVisibleNavigationItems = (isAuthenticated) => {
  return navigationItems.filter(item => {
    if (item.requiresAuth && !isAuthenticated) {
      return false
    }
    return true
  })
}