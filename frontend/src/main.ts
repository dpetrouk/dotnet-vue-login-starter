import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import LoginForm from './components/LoginForm.vue'
import Dashboard from './views/Dashboard.vue'
import './style.css'

const routes = [
  { path: '/', component: LoginForm },
  { path: '/dashboard', component: Dashboard }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

createApp(App).use(router).mount('#app')
