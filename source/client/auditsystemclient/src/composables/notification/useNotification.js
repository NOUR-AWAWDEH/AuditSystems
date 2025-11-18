import { ref } from "vue";

const notifications = ref([]);

export function useNotification() {
  const showNotification = (message, type = "info", duration = 4000) => {
    const id = Date.now();

    notifications.value.push({
      id,
      message,
      type,
      show: true,
    });

    setTimeout(() => hideNotification(id), duration);
  };

  const hideNotification = (id) => {
    const index = notifications.value.findIndex((n) => n.id === id);
    if (index !== -1) notifications.value.splice(index, 1);
  };

  return {
    notifications,
    showNotification,
    hideNotification,
  };
}
