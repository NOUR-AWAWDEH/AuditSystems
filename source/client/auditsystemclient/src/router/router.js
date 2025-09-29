import { createRouter, createWebHistory } from 'vue-router';

const Dashboard = () => import('@/components/features/dashboard/DashboardPage.vue');
const ForgotPassword = () => import('@/components/features/auth/ForgotPassword.vue');
const ResetPassword = () => import('@/components/features/auth/ResetPassword.vue');
const SideBar = () => import('@/components/common/SideBar.vue');
const Login = () => import('@/components/features/auth/LoginForm.vue');
const SupportingDocManager = () => import('@/components/features/supportingDoc/SupportingDocumentsView.vue');
const Organization = () => import('@/components/features/Organization/OrganizationView.vue');
const RiskControl = () => import('@/components/features/riskControl/RiskControl.vue');
const routes = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/login',
    name: 'Login',
    component: Login,
    meta: { requiresAuth: false }
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Dashboard,
    meta: { requiresAuth: true }
  },
  {
    path: '/forgot-password',
    name: 'ForgotPassword',
    component: ForgotPassword,
    meta: { requiresAuth: false }
  },
  {
    path: '/reset-password',
    name: 'ResetPassword',
    component: ResetPassword,
    meta: { requiresAuth: false }
  },
  {
    path: '/logout',
    name: 'Logout',
    beforeEnter: (to, from, next) => {
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
      next('/login');
    }
  },
  {
    path: '/sidebar',
    name: 'Sidebar',
    component: SideBar,
    meta: { requiresAuth: true }
  },
  {
    path: '/risk-control',
    name: 'RiskControl',
    component: RiskControl,
    meta: { requiresAuth: true }
  },
  {
    path: '/organization',
    name: 'Organization',
    component: Organization,
    meta: { requiresAuth: true }
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/login'
  },
  {
    path: '/supporting-docs',
    name: 'SupportingDocs',
    component: SupportingDocManager,
    meta: { requiresAuth: true },
  },
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

  if (to.path === '/' && !isAuthenticated) {
    next('/login');
    return;
  }

  if (to.name === 'Login' && isAuthenticated) {
    next('/dashboard');
    return;
  }

  if (requiresAuth && !isAuthenticated) {
    next({ name: 'Login', state: { redirectTo: to.fullPath } });
    return;
  }

  next();
});

export default router;