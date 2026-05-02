<script setup lang="ts">
import apiService from '@/api/apiService'
import { createGame, type Game } from '@/data/Game'
import { onBeforeMount, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const props = defineProps<{
  game: Game
}>()

const router = useRouter()

const emit = defineEmits(['deletedGame'])

const currentGame: Game = props.game

function deleteGame(): void {
  apiService.games.deleteGame(currentGame.id)
  emit('deletedGame')
}

function editGame(): void {
  router.push('/games/' + currentGame?.id + '/edit')
}

onBeforeMount(() => {
  // for some reason it  was STILL IN ISO 8601 FORMAT, NOT A DATE
  currentGame.postedDate = new Date(currentGame.postedDate)
})

function goToGamePage(): void {
  router.push('/games/' + currentGame?.id)
}
</script>

<template>
  <div
    data-testid="gameItem"
    class="text-xs flex flex-row items-center bg-[#f4b5ea] p-1 md:p-2 md:text-2xl border-2"
  >
    <div
      @click="goToGamePage()"
      class="text-xl hover:cursor-pointer hover:scale-105 text-center w-14 md:w-56 overflow-hidden text-ellipsis whitespace-nowrap"
    >
      {{ currentGame?.name }}
    </div>
    <div class="text-xl text-center w-10 md:w-30 overflow-hidden text-ellipsis whitespace-nowrap">
      {{
        currentGame?.postedDate.getFullYear() +
        '/' +
        currentGame?.postedDate.getMonth() +
        '/' +
        currentGame?.postedDate.getDay()
      }}
    </div>
    <div class="text-xl text-center w-10 md:w-30 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.rating.toPrecision(3) }}
    </div>
    <div class="text-xl text-center w-16 md:w-48 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.mainTag }}
    </div>
    <div class="text-xl text-center w-16 md:w-48 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.developer }}
    </div>
    <button
      @click="deleteGame"
      data-testid="removeGameButton"
      class="bg-[#905a88] hover:cursor-pointer hover:scale-110 hover:bg-[#6e4467] active:bg-[#563550] pl-1.5 pr-1.5 border-2"
    >
      X
    </button>
    <button
      @click="editGame"
      data-testid="editGameButton"
      class="bg-[#905a88] hover:cursor-pointer hover:scale-110 hover:bg-[#6e4467] active:bg-[#563550] pl-1.5 pr-1.5 border-2 ml-5"
    >
      E
    </button>
  </div>
</template>
