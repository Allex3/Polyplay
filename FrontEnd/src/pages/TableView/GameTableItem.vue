<script setup lang="ts">
import { createGame, type Game } from '@/data/model'
import { useGamesStore } from '@/stores/gameRepo'
import { onBeforeMount, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const props = defineProps<{
  game: Game
}>()

const router = useRouter()

const emit = defineEmits(['deletedGame'])

const currentGame: Game | undefined = props.game

const gamesStore = useGamesStore()

function deleteGame(): void {
  gamesStore.deleteGame(currentGame)
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
    class="text-sm flex flex-row items-center bg-[#f4b5ea] p-1 md:p-2 md:text-2xl border-2"
  >
    <div
      @click="goToGamePage()"
      class="hover:cursor-pointer hover:scale-120 text-center w-18 md:w-56 overflow-hidden text-ellipsis whitespace-nowrap"
    >
      {{ currentGame?.name }}
    </div>
    <div class="text-center w-10 md:w-30 overflow-hidden text-ellipsis whitespace-nowrap">
      {{
        currentGame?.postedDate.getFullYear() +
        '/' +
        currentGame?.postedDate.getMonth() +
        '/' +
        currentGame?.postedDate.getDay()
      }}
    </div>
    <div class="text-center w-10 md:w-30 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.rating }}
    </div>
    <div class="text-center w-16 md:w-48 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.tags?.at(0) }}
    </div>
    <div class="text-center w-16 md:w-48 overflow-hidden text-ellipsis whitespace-nowrap">
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
