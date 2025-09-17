import { createApp } from 'vue';
import App from './App.vue';
import './styles/style.css';
import router from './router/router';
import authService from '@/services/auth/authRepos/authService';

const app = createApp(App);
app.use(router);

router.isReady().then(() => {
  app.mount('#app');
  authService.initializeTokenRefresh();
});
