import { createRouter, createWebHistory } from 'vue-router';
import LoginForm from '@/components/features/auth/LoginForm.vue';
//import SignOut from '@/components/features/auth/SignOut.vue';

// Lazy-loaded components for better performance
const Dashboard = () => import('@/components/features/dashboard/DashboardPage.vue');
//const AuditReports = () => import('@/components/features/reports/AuditReports.vue');
//const Settings = () => import('@/components/features/settings/UserSettings.vue');

const routes = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/login',
    name: 'Login',
    component: LoginForm,
    meta: { requiresAuth: false }
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Dashboard,
    meta: { requiresAuth: true }
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/login'
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    return savedPosition || { top: 0 }
  }
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = localStorage.getItem('token');
  const requiresAuth = to.matched.some(record => record.meta.requiresAuth);

  // Handle root path redirect first
  if (to.path === '/' && !isAuthenticated) {
    next('/login');
    return;
  }

  // Redirect authenticated users from login to dashboard
  if (to.name === 'Login' && isAuthenticated) {
    next('/dashboard');
    return;
  }

  // Protect authenticated routes
  if (requiresAuth && !isAuthenticated) {
    next({ name: 'Login', state: { redirectTo: to.fullPath } });
    return;
  }

  next();
});

export default router;
