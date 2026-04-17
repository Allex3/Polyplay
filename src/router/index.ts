import Home from '../pages/Home.vue'

import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/register',
    name: 'Register',
    // generates a separate .ts file for this route, which is lazy-loaded when this route is visited
    // (helps initial loading perforamnce ig?)
    component: () => import('../pages/Register.vue'),
  },
  {
    path: '/login',
    name: 'Login',
    component: () => import('../pages/Login.vue'),
  },
  {
    path: '/user/table',
    name: 'UserGamesTable',
    component: () => import('../pages/TableView/UserGamesTable.vue'),
  },
  {
    path: '/games/:gameid/edit',
    component: () => import('../pages/EditGame.vue'),
  },
  {
    path:'/PermissionDenied',
    component: () => import('../pages/PermissionDenied.vue')
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router
