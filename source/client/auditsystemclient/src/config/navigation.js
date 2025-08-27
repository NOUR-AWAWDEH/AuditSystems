import DashboardIcon from '@/components/common/icons/DashboardIcon.vue'
import DepartmentIcon from '@/components/common/icons/DepartmentIcon.vue'
import RiskControlIcon from '@/components/common/icons/RiskControlIcon.vue'
import DocumentIcon from '@/components/common/icons/DocumentIcon.vue'

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
    route: '/organization',
    name: 'Organization',
    label: 'Organization',
    icon: DepartmentIcon,
    requiresAuth: true
  },
  {
    route: '/risk-control',
    name: 'RiskControl',
    label: 'Risk & Control',
    icon: RiskControlIcon,
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