import axios from 'axios'

export interface LoginResponse {
  userId: number
  fullName: string
  email: string
  profile?: {
    city: string
    street: string
    building: string
    zipCode: string
  }
}

export async function login(email: string, password: string): Promise<LoginResponse> {
  const { data } = await axios.post<LoginResponse>('/api/auth/login', { email, password })
  return data
}
