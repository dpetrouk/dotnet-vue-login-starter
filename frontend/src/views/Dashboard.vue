<template>
  <div class="dashboard">
    <div v-if="user" class="profile-card">
      <h1>Welcome, {{ user.fullName }}</h1>

      <div class="info">
        <p><strong>Email:</strong> {{ user.email }}</p>

        <div v-if="user.profile" class="profile">
          <h3>Profile</h3>
          <p>{{ user.profile.city }}, {{ user.profile.street }} {{ user.profile.building }}</p>
          <p>ZIP: {{ user.profile.zipCode }}</p>
        </div>
      </div>

      <button @click="logout" class="logout-btn">Logout</button>
    </div>

    <div v-else class="not-logged-in">
      <p>Please log in first.</p>
      <router-link to="/">Go to Login</router-link>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuth } from '../composables/useAuth'
import { getProfile } from '../api/profile'

const router = useRouter()
const { getToken, logout: clearToken } = useAuth()
const user = ref(null)

onMounted(async () => {
  if (!getToken()) return
  user.value = await getProfile()
})

function logout() {
  clearToken()
  router.push('/')
}
</script>

<style scoped>
.dashboard {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f5f5f5;
}

.profile-card {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 500px;
}

.profile-card h1 {
  margin: 0 0 1rem;
}

.info p {
  margin: 0.5rem 0;
}

.profile {
  margin-top: 1rem;
  padding-top: 1rem;
  border-top: 1px solid #eee;
}

.logout-btn {
  margin-top: 1.5rem;
  padding: 0.5rem 1rem;
  background: #e74c3c;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  width: 100%;
}

.not-logged-in {
  text-align: center;
}
</style>
