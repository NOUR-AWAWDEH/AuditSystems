import { createRouter, createWebHistory } from 'vue-router';
import LoginForm from '@/components/features/auth/LoginForm.vue';
import SignOut from '@/components/features/auth/SignOut.vue';
// Import other components you want to route to, e.g., a Dashboard
// import Dashboard from '@/components/Dashboard.vue';

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: LoginForm,
  },
  {
    path: '/signout', // Use lowercase for consistency
    name: 'SignOut',
    component: SignOut,
  },
  // Add other routes here, e.g.:
  // {
  //   path: '/dashboard',
  //   name: 'Dashboard',
  //   component: Dashboard,
  //   meta: { requiresAuth: true } // Example for protected routes
  // },
  // Redirect root path to login or dashboard depending on auth state
  {
    path: '/',
    redirect: '/login', // Or '/dashboard' if authenticated
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Optional: Add navigation guards for authentication checks
// router.beforeEach((to, from, next) => {
//   const isAuthenticated = !!localStorage.getItem('token'); // Basic check
//   if (to.meta.requiresAuth && !isAuthenticated) {
//     next('/login');
//   } else {
//     next();
//   }
// });

export default router;
