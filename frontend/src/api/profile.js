import client from '../http/client'

export async function getProfile() {
  const { data } = await client.get('/api/profile', { authenticated: true })
  return data
}
