<script setup lang="ts">
import { useRouter } from 'vue-router'
import GameImageItem from './GameImageItem.vue'
import { onBeforeMount, ref } from 'vue'
import apiService from '@/api/apiService'

const games = (await apiService.games.getGames()).data

const visibleGamesOnPage = 6

const visibleGames = ref(games.slice(0, visibleGamesOnPage))

const page = ref(1)
const totalPages = ref(Math.ceil(games.length / visibleGamesOnPage))

function getStartingPage() {
  totalPages.value = Math.ceil(games.length / visibleGamesOnPage)
  page.value = 1
  visibleGames.value = games.slice(0, visibleGamesOnPage)
}

function getEndingPage() {
  totalPages.value = Math.ceil(games.length / visibleGamesOnPage)
  page.value = totalPages.value
  visibleGames.value = games.slice((totalPages.value - 1) * visibleGamesOnPage)
}

function getPreviousPage() {
  totalPages.value = Math.ceil(games.length / visibleGamesOnPage)
  if (page.value == 1) page.value = totalPages.value
  else page.value--

  updateGamesView()
}

function getNextPage() {
  totalPages.value = Math.ceil(games.length / visibleGamesOnPage)
  if (page.value == totalPages.value) page.value = 1
  else page.value++

  updateGamesView()
}

function updateGamesView(): void {
  totalPages.value = Math.ceil(games.length / visibleGamesOnPage)
  const startingGameIndex = (page.value - 1) * visibleGamesOnPage
  visibleGames.value = games.slice(startingGameIndex, startingGameIndex + visibleGamesOnPage)

  if (totalPages.value == 0) // NO GAMES
  {
    updateNoGames()
    return
  }
}

function updateNoGames(): void {
  totalPages.value = 1
  page.value = 1
}

onBeforeMount(() => {
  visibleGames.value = games.slice(0, visibleGamesOnPage)
  totalPages.value = Math.ceil(games.length / visibleGamesOnPage)
})
</script>
<template>
  <div
    class="flex flex-col gap-5 items-center retro-window w-4/5 m-auto mt-15 pr-10 pl-10 pt-5 pb-5 mb-3"
  >
    <div class="gap-5 grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4">
      <div v-for="game in visibleGames" :key="game.id">
        <GameImageItem :game="game" />
      </div>
    </div>

    <div class="flex flex-row gap-x-3 text-2xl">
      <button
        class="bg-[#bfedef] border-black border-2 px-2 hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
        @click="getStartingPage"
      >
        &lt;&lt;
      </button>
      <button
        class="bg-[#bfedef] border-black border-2 px-2 hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
        @click="getPreviousPage"
      >
        &lt;
      </button>
      <div class="text-sm text-center pt-2">{{ page }} / {{ totalPages }}</div>
      <button
        class="bg-[#bfedef] border-black border-2 px-2 hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
        @click="getNextPage"
      >
        &gt;
      </button>
      <button
        class="bg-[#bfedef] border-black border-2 px-2 hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
        @click="getEndingPage"
      >
        &gt;&gt;
      </button>
    </div>
  </div>
</template>
