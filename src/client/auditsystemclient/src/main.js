import { createApp } from 'vue';
import App from './App.vue';
import './styles/style.css';
import router from './router/router';
import authService from '@/services/auth/authService';

const app = createApp(App);
app.use(router);

router.isReady().then(() => {
  console.log("Router is ready");
  app.mount('#app');
  console.log("App mounted");
  authService.initializeTokenRefresh();
});
