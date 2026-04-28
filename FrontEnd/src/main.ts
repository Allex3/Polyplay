import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import './global.css'

import { fetchCachedRequests } from './api/offlineApiSupport'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')

setInterval(fetchCachedRequests, 10000) // every 10 seconds
