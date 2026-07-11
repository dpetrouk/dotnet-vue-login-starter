import client from '../http/client'

export async function login(email, password) {
  const { data } = await client.post('/api/auth/login', { email, password })
  return data
}
