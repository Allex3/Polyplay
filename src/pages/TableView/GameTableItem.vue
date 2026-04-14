<script setup lang="ts">
import { createGame, type Game } from '@/data/model'
import { useUserGamesStore } from '@/stores/gameRepo'

const props = defineProps<{
  game: Game
}>()

const emit = defineEmits(['deletedGame'])

const currentGame: Game | undefined = props.game

const userGamesStore = useUserGamesStore()

function deleteGame(): void {
  userGamesStore.deleteGame(currentGame)
  emit('deletedGame')
}

function editGame(): void {
  // pls implement this in a separate page
}
</script>

<template>
  <div class="flex flex-row items-center bg-[#f4b5ea] p-2 text-2xl border-2">
    <div class="text-center w-56 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.name }}
    </div>
    <div class="text-center w-30 overflow-hidden text-ellipsis whitespace-nowrap">
      {{
        currentGame?.postedDate.getFullYear() +
        '/' +
        currentGame?.postedDate.getMonth() +
        '/' +
        currentGame?.postedDate.getDay()
      }}
    </div>
    <div class="text-center w-30 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.rating }}
    </div>
    <div class="text-center w-48 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.tags?.at(0) }}
    </div>
    <div class="text-center w-48 overflow-hidden text-ellipsis whitespace-nowrap">
      {{ currentGame?.developer }}
    </div>
    <button
      @click="deleteGame"
      class="bg-[#905a88] hover:cursor-pointer hover:scale-110 hover:bg-[#6e4467] active:bg-[#563550] pl-1.5 pr-1.5 border-2"
    >
      X
    </button>
    <button
      @click="editGame"
      class="bg-[#905a88] hover:cursor-pointer hover:scale-110 hover:bg-[#6e4467] active:bg-[#563550] pl-1.5 pr-1.5 border-2 ml-5"
    >
      E
    </button>
  </div>
</template>
