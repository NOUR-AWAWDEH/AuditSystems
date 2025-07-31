import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import LoginForm from '../features/auth/LoginForm.vue'

describe('LoginForm', () => {
  it('renders properly', () => {
    const wrapper = mount(LoginForm)
    expect(wrapper.exists()).toBe(true)
  })
})