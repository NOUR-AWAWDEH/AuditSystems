import { createRouter, createWebHistory } from "vue-router";

const Dashboard = () => import("@/components/features/dashboard/DashboardPage.vue");
const ForgotPassword = () => import("@/components/features/auth/ForgotPassword.vue");
const ResetPassword = () => import("@/components/features/auth/ResetPassword.vue");
const Login = () => import("@/components/features/auth/LoginForm.vue");
const SupportingDocManager = () => import("@/components/features/supportingDoc/SupportingDocumentsView.vue");
const Organization = () => import("@/components/features/Organization/OrganizationView.vue");
const RiskControl = () => import("@/components/features/riskControl/RiskControl.vue");
const UserManagement = () => import("@/components/features/userManagement/UserManagementVue.vue");
const ActionOwner = () => import("@/components/features/actionOwner/ActionOwnerView.vue");
const AccessDenied = () => import("@/components/features/errors/AccessDeniedPage.vue");

const routes = [
  {
    path: "/",
    redirect: "/login",
  },
  {
    path: "/confirm-email",
    name: "ConfirmEmail",
    component: () =>
      import("@/components/features/userManagement/confirmEmail/ConfirmEmailPage.vue"),
    meta: { requiresAuth: false },
  },
  {
    path: "/login",
    name: "Login",
    component: Login,
    meta: { requiresAuth: false },
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    component: Dashboard,
    meta: { requiresAuth: true, denyRoles: ["ActionOwner"] },
  },
  {
    path: "/forgot-password",
    name: "ForgotPassword",
    component: ForgotPassword,
    meta: { requiresAuth: false },
  },
  {
    path: "/reset-password",
    name: "ResetPassword",
    component: ResetPassword,
    meta: { requiresAuth: false },
  },
  {
    path: "/resend-verification-email",
    name: "ResendVerificationEmail",
    component: () => import("@/components/features/auth/ResendVerificationEmailPage.vue"),
    meta: { requiresAuth: false },
  },
  {
    path: "/logout",
    name: "Logout",
    beforeEnter: (to, from, next) => {
      localStorage.removeItem("token");
      localStorage.removeItem("refreshToken");
      next("/login");
    },
  },
  {
    path: "/risk-control",
    name: "RiskControl",
    component: RiskControl,
    meta: { requiresAuth: true, denyRoles: ["ActionOwner"] },
  },
  {
    path: "/organization",
    name: "Organization",
    component: Organization,
    meta: { requiresAuth: true, denyRoles: ["ActionOwner"] },
  },
  {
    path: "/:pathMatch(.*)*",
    redirect: "/login",
  },
  {
    path: "/supporting-docs",
    name: "SupportingDocs",
    component: SupportingDocManager,
    meta: { requiresAuth: true, denyRoles: ["ActionOwner"] },
  },
  {
    path: "/user-management",
    name: "UserManagement",
    component: UserManagement,
    meta: { requiresAuth: true, denyRoles: ["ActionOwner"] },
  },
  {
    path: "/action-owner",
    name: "ActionOwner",
    component: ActionOwner,
    meta: { requiresAuth: true, roles: ["ActionOwner"] },
  },
  {
    path: "/access-denied",
    name: "AccessDenied",
    component: AccessDenied,
    meta: { requiresAuth: true },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    return savedPosition || { top: 0 };
  },
});

import { isTokenExpired } from "@/utils/jwtUtils";
import { useAuthStatus } from "@/composables/auth/useAuthStatus";

router.beforeEach(async (to, from, next) => {
  const token = localStorage.getItem("token");
  const isAuthenticated = !!token;
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth);
  const requiredRoles = to.matched.find((record) => record.meta.roles)?.meta.roles;
  const deniedRoles = to.matched.find((record) => record.meta.denyRoles)?.meta.denyRoles;

  // Check if token is expired
  if (isAuthenticated && isTokenExpired(token)) {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");

    if (requiresAuth) {
      next("/login");
      return;
    }
  }

  if (to.path === "/" && !isAuthenticated) {
    next("/login");
    return;
  }

  if (to.name === "Login" && isAuthenticated) {
    // Check user role and redirect appropriately
    const { checkAuthStatus, authStatus } = useAuthStatus();
    await checkAuthStatus(token);
    
    if (authStatus.value?.roles?.includes("ActionOwner")) {
      next("/action-owner");
    } else {
      next("/dashboard");
    }
    return;
  }

  // Check role-based access (required roles)
  if (requiredRoles && isAuthenticated) {
    const { checkAuthStatus, authStatus } = useAuthStatus();
    
    if (!authStatus.value?.roles) {
      await checkAuthStatus(token);
    }
    
    const hasRequiredRole = requiredRoles.some((role) => authStatus.value?.roles?.includes(role));
    
    if (!hasRequiredRole) {
      console.warn(`Access denied: User does not have required roles [${requiredRoles.join(', ')}]`);
      next("/access-denied");
      return;
    }
  }

  // Check denied roles (denyRoles)
  if (deniedRoles && isAuthenticated) {
    const { checkAuthStatus, authStatus } = useAuthStatus();
    
    if (!authStatus.value?.roles) {
      await checkAuthStatus(token);
    }
    
    const hasDisallowedRole = deniedRoles.some((role) => authStatus.value?.roles?.includes(role));
    
    if (hasDisallowedRole) {
      console.warn(`Access denied: User has disallowed roles [${deniedRoles.join(', ')}]`);
      next("/access-denied");
      return;
    }
  }

  if (requiresAuth && !isAuthenticated) {
    next({ name: "Login", state: { redirectTo: to.fullPath } });
    return;
  }

  next();
});

export default router;
