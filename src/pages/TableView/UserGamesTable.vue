<script setup lang="ts">
import GameTableItem from './GameTableItem.vue'
import { onBeforeMount, onMounted, ref } from 'vue'

import AddGameModal from './AddGameModal.vue'
import { useUserGamesStore } from '@/stores/repo'
import { useAddGameModal } from '@/composables/useAddGameModal'

const userGamesStore = useUserGamesStore()
const { openAddGameModal } = useAddGameModal()

const visibleGamesOnPage = 4

const visibleGames = ref(userGamesStore.games.slice(0, visibleGamesOnPage))

const page = ref(1)
const totalPages = ref(Math.ceil(userGamesStore.games.length / visibleGamesOnPage))

function getStartingPage() {
  totalPages.value = Math.ceil(userGamesStore.games.length / visibleGamesOnPage)
  page.value = 1
  visibleGames.value = userGamesStore.games.slice(0, visibleGamesOnPage)
}

function getEndingPage() {
  totalPages.value = Math.ceil(userGamesStore.games.length / visibleGamesOnPage)
  page.value = totalPages.value
  visibleGames.value = userGamesStore.games.slice((totalPages.value - 1) * visibleGamesOnPage)
}

function getPreviousPage() {
  totalPages.value = Math.ceil(userGamesStore.games.length / visibleGamesOnPage)
  if (page.value == 1) page.value = totalPages.value
  else page.value--

  updateGamesView()
}

function getNextPage() {
  totalPages.value = Math.ceil(userGamesStore.games.length / visibleGamesOnPage)
  if (page.value == totalPages.value) page.value = 1
  else page.value++

  updateGamesView()
}

function updateGamesView(): void {
  totalPages.value = Math.ceil(userGamesStore.games.length / visibleGamesOnPage)
  const startingGameIndex = (page.value - 1) * visibleGamesOnPage
  visibleGames.value = userGamesStore.games.slice(
    startingGameIndex,
    startingGameIndex + visibleGamesOnPage,
  )

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
  userGamesStore.resetGames()
  visibleGames.value = userGamesStore.games.slice(0, visibleGamesOnPage)
  totalPages.value = Math.ceil(userGamesStore.games.length / visibleGamesOnPage)
})
</script>

<template>
  <div class="retro-window flex flex-col justify-between items-center">
    <div class="flex flex-row text-3xl pt-13 mr-6">
      <div class="text-center w-30 pr-3 pl-3 overflow-hidden text-ellipsis whitespace-nowrap">
        Name
      </div>
      <div class="text-center w-30 pl-15">Date</div>
      <div class="text-center w-30 pl-11">Rating</div>
      <div class="text-center w-48 pl-6">Main Tag</div>
      <div class="text-center w-48 pr-6">Developer</div>
    </div>
    <div class="flex flex-col gap-5 w-216">
      <div v-for="game in visibleGames" :key="game.id">
        <GameTableItem @deleted-game="updateGamesView" :game="game" class="hover:bg-[#98518E]" />
      </div>
    </div>

    <!-- buttons -->
    <div class="flex flex-row items-center gap-x-10 mb-15">
      <div>
        <button
          class="bg-[#bfedef] border-black border-2 px-2 text-2xl hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
          @click="openAddGameModal"
        >
          + Add boardgame
        </button>
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
        <div>{{ page }} / {{ totalPages }}</div>
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
  </div>

  <AddGameModal @added-game="updateGamesView" />
</template>

<style scoped>
.retro-window {
  width: 60rem;
  height: 35rem;
  position: relative;
  margin: auto;
}
</style>
