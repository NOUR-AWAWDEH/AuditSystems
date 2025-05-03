import { ref } from 'vue'
import { authService } from '@/services/auth/authService'

export function useAuth() {
  const isAuthenticated = ref(false)
  const user = ref(null)
  const error = ref(null)

  const login = async (username, password) => {
    try {
      // For now, just simulate a successful login
      // In a real app, you would call your API
      console.log('Login attempt:', username, password)
      
      // Simulate API call
      // const response = await authService.login(username, password)
      // user.value = response.user
      
      isAuthenticated.value = true
      return { success: true }
    } catch (err) {
      error.value = err.message || 'Authentication failed'
      throw err
    }
  }

  const logout = async () => {
    try {
      // await authService.logout()
      isAuthenticated.value = false
      user.value = null
    } catch (err) {
      error.value = err.message || 'Logout failed'
      throw err
    }
  }

  return {
    isAuthenticated,
    user,
    error,
    login,
    logout
  }
}