import { ref } from 'vue';

const notification = ref({
  show: false,
  message: '',
  type: 'info', // success, error, warning, info
  timeout: null,
});

export function useNotification() {
  const showNotification = (message, type = 'info', duration = 4000) => {
    if (notification.value.timeout) {
      clearTimeout(notification.value.timeout);
    }
    notification.value.show = true;
    notification.value.message = message;
    notification.value.type = type;
    notification.value.timeout = setTimeout(() => {
      notification.value.show = false;
    }, duration);
  };

  const hideNotification = () => {
    if (notification.value.timeout) {
      clearTimeout(notification.value.timeout);
    }
    notification.value.show = false;
  };

  return {
    notification,
    showNotification,
    hideNotification,
  };
}