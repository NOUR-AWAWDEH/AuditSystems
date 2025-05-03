// This would be your actual API service
export const authService = {
  async login(username, password) {
    // In a real app, this would make an API call
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve({
          user: {
            id: 1,
            username,
            name: 'Test User',
            role: 'admin'
          },
          token: 'fake-jwt-token'
        })
      }, 1000)
    })
  },
  
  async logout() {
    // In a real app, this would make an API call
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve({ success: true })
      }, 500)
    })
  }
}