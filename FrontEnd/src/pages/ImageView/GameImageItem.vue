<script setup lang="ts">
import { type Game, createGame } from '@/data/Game'
import { useRouter } from 'vue-router'

const props = defineProps<{
  game: Game
}>()

const router = useRouter()

const currentGame: Game = props.game

function goToGamePage(): void {
  router.push('/games/' + currentGame?.id)
}
</script>
<template>
  <div class="flex flex-col border hover:cursor-pointer hover:scale-105" @click="goToGamePage()">
    <!-- Game Thumbnail -->
    <img :src="currentGame.thumbnailPath" :alt="currentGame.name" />
    <!-- Game name + rating -->
    <div class="pt-1 pb-1 pl-2 pr-2 text-[#6C4366] flex flex-col items-center border-t">
      <span>{{ currentGame.name }}</span>
      <!-- Rating -->
      <div class="self-end -mt-2 flex flex-row gap-1">
        <span class="self-center">{{ currentGame.rating }}</span>
        <img
          src="../../../assets/Games/RatingStar.png"
          class="w-4 h-4 self-center mb-0.5"
          alt="Rating Star"
        />
      </div>
    </div>
  </div>
</template>
