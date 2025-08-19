// src/components/__tests__/LoginForm.test.js
import { mount } from '@vue/test-utils';
import { createRouter, createWebHistory } from 'vue-router';
import LoginForm from '@/components/features/auth/LoginForm.vue';
import BaseInput from '@/components/base/BaseInput.vue';
import PasswordInput from '@/components/base/PasswordInput.vue';
import BaseButton from '@/components/base/BaseButton.vue';
import BaseLink from '@/components/base/BaseLink.vue';

// Stub routes used by LoginForm
const routes = [
  { path: '/forgot-password', component: { template: '<div>Forgot Password Page</div>' } },
  { path: '/dashboard', component: { template: '<div>Dashboard Page</div>' } },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

describe('LoginForm', () => {
  beforeEach(async () => {
    router.push('/');
    await router.isReady();
  });

  it('renders all main elements properly', () => {
    const wrapper = mount(LoginForm, {
      global: {
        plugins: [router],
      },
    });

    // Check container div exists
    expect(wrapper.find('div.bg-white').exists()).toBe(true);

    // Check logo exists
    expect(wrapper.findComponent({ name: 'AuditLogo' }).exists()).toBe(true);

    // Check username input
    expect(wrapper.findComponent(BaseInput).exists()).toBe(true);

    // Check password input
    expect(wrapper.findComponent(PasswordInput).exists()).toBe(true);

    // Check login button
    expect(wrapper.findComponent(BaseButton).exists()).toBe(true);

    // Check forgot password link
    expect(wrapper.findComponent(BaseLink).exists()).toBe(true);
  });

  it('validates empty form on login click', async () => {
    const wrapper = mount(LoginForm, {
      global: {
        plugins: [router],
      },
    });

    await wrapper.findComponent(BaseButton).trigger('click');

    // Check validation errors are shown
    const errors = wrapper.vm.validationErrors;
    expect(errors.length).toBeGreaterThan(0);
    expect(wrapper.vm.isValidationErrorVisible).toBe(true);
  });
});
