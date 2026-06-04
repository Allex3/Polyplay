import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import './global.css'

export const BASE_URL = 'https://172.30.248.197:7114'

import { fetchCachedRequests } from './api/offlineApiSupport'
import { useUserStore } from './stores/userStore.ts'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')
// setInterval(fetchCachedRequests, 10000) // every 10 seconds
