import { ref } from 'vue';

export function useSidebar() {
  const isCollapsed = ref(true);

  const toggleSidebar = () => {
    isCollapsed.value =!isCollapsed.value
  }
  return {
    isCollapsed,
    toggleSidebar
  };
}