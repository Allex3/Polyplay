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
    name: 'TableView',
    component: () => import('../pages/TableView/TableView.vue'),
  },
  {
    path: '/games/:gameid/edit',
    component: () => import('../pages/Games/EditGame.vue'),
  },
  {
    path: '/games/:gameid',
    component: () => import('../pages/Games/GamePage.vue'),
  },
  {
    path: '/PermissionDenied',
    component: () => import('../pages/PermissionDenied.vue'),
  },
  {
    path: '/games',
    component: () => import('../pages/ImageView/MainGamesCatalogue.vue'),
  },
  {
    path: '/stats',
    component: () => import('../pages/Statistics/Statistics.vue'),
  },
  {
    path: '/general',
    component: () => import('../pages/Chat/GeneralChat.vue'),
  },
  {
    path: '/userActivity',
    component: () => import('../pages/Logging/UserActivitiesLog.vue'),
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

export default router
