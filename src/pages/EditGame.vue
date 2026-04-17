<script setup lang="ts">
import { ref } from 'vue'
import { onBeforeRouteUpdate, useRoute, useRouter } from 'vue-router'
import { useUserService } from '@/stores/userService'
import { useGamesStore } from '@/stores/gameRepo'
import { type Game, type User, createGame } from '@/data/model'

const userService = useUserService()
const gameStore = useGamesStore()

const router = useRouter()
const route = useRoute()

const currentGame = ref<Game>(createGame())

if (userService.user.username == '2') {
  router.push('/PermissionDenied')
} else {
  // correct
  currentGame.value = gameStore.getGame(Number(route.params.gameid))
}
</script>
<template>
  <div>{{ currentGame?.id }}</div>
</template>
