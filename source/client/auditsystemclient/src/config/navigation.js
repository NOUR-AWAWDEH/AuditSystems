import DashboardIcon from '@/components/common/icons/DashboardIcon.vue'
import DocumentIcon from '@/components/common/icons/DocumentIcon.vue'
import LocationIcon from '@/components/common/icons/LocationIcon.vue'

// Navigation items that correspond to your router.js routes
export const navigationItems = [
  {
    route: '/dashboard',
    name: 'Dashboard',
    label: 'Dashboard',
    icon: DashboardIcon,
    requiresAuth: true
  },
  {
    route: '/supporting-docs',
    name: 'SupportingDocs',
    label: 'Supporting Docs',
    icon: DocumentIcon,
    requiresAuth: true
  },
  {
    route: '/locations',
    name: 'Locations',
    label: 'Locations',
    icon: LocationIcon,
    requiresAuth: true
  }
  // Removed User Menu from sidebar navigation
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