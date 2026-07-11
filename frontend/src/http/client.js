import axios from 'axios'

const client = axios.create()

client.interceptors.request.use(config => {
  if (config.authenticated) {
    const token = sessionStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
  }
  return config
})

export default client
