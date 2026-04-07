<script setup lang="ts">
// import type { BoardGame } from '@/data/boardgame'
import { createGame } from '@/data/model'

const props = defineProps({
  game: createGame,
})

const currentGame = props.game
</script>

<template>
  <div class="flex flex-row items-center bg-mauve-200 p-4 text-2xl">
    <div class="w-56">{{ currentGame?.name }}</div>
    <div class="w-56">{{ currentGame?.description }}</div>
    <div class="w-36">{{ currentGame?.postedDate }} RON</div>
    <div class="w-56">{{ currentGame?.rating }} copies</div>
    <div class="w-48">{{ currentGame?.mainTag }}</div>
    <div class="w-48">{{ currentGame?.developer }}</div>
    <!--<Teleport-->
  </div>
</template>
