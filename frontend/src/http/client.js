import axios from 'axios'

const client = axios.create()

client.interceptors.request.use(config => {
  console.log(`[HTTP] ${config.method.toUpperCase()} ${config.url}`, config)
  if (config.authenticated) {
    const token = sessionStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
      console.log('[HTTP] Token attached')
    } else {
      console.warn('[HTTP] authenticated=true but no token in sessionStorage')
    }
  }
  return config
})

export default client
