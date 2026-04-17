import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import './global.css'

const app = createApp(App)

app.use(createPinia())
app.use(router)

import { useGamesStore } from './stores/gameRepo'
import { createGame } from './data/model'

app.mount('#app')
