import { login } from '../api/auth'

export function useAuth() {
  async function loginUser(email, password) {
    const { token } = await login(email, password)
    sessionStorage.setItem('token', token)
  }

  function logout() {
    sessionStorage.removeItem('token')
  }

  function getToken() {
    return sessionStorage.getItem('token')
  }

  return { loginUser, logout, getToken }
}
