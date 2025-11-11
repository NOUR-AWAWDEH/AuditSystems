import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { useLogin } from '@/composables/auth/useLogin';

export function useAppLayout() {
  const route = useRoute();
  const { isAuthenticated } = useLogin();
  
  // Routes where NavBar and SideBar should not be displayed
  const hideNavBarRoutes = ['Login', 'ForgotPassword', 'ResetPassword'];
  
  const shouldShowNavBar = computed(() => {
    return isAuthenticated.value && !hideNavBarRoutes.includes(route.name);
  });
  
  const shouldShowSideBar = computed(() => {
    return isAuthenticated.value && !hideNavBarRoutes.includes(route.name);
  });
  
  const transitionName = computed(() => {
    // Disable transitions in development to prevent HMR issues
    if (import.meta.env.DEV) {
      return '';
    }
    return route.meta.transition || 'fade';
  });
  
  return {
    shouldShowNavBar,
    shouldShowSideBar,
    transitionName,
    isAuthenticated
  };
}