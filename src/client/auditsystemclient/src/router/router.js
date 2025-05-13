import { createRouter, createWebHistory } from 'vue-router';
import LoginForm from '@/components/features/auth/LoginForm.vue';

const Dashboard = () => import('@/components/features/dashboard/DashboardPage.vue');
const ForgotPassword = () => import('@/components/features/auth/ForgotPassword.vue');
const ResetPassword = () => import('@/components/features/auth/ResetPassword.vue');

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
      next('/login');
    }
  },
  {
    path: '/sidebar',
    name: 'Sidebar',
    component: () => import('@/components/common/SideBar.vue'),
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

  if (to.path === '/' && !isAuthenticated) {
    next('/login');
    return;
  }

  if (to.name === 'Login' && isAuthenticated) {
    next('/sidebar');
    return;
  }

  if (requiresAuth && !isAuthenticated) {
    next({ name: 'Login', state: { redirectTo: to.fullPath } });
    return;
  }

  next();
});

export default router;
