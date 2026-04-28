<script setup lang="ts">
import GameTableItem from './GameTableItem.vue'
import { onBeforeMount, onMounted, ref, Suspense } from 'vue'

import AddGameModal from './AddGameModal.vue'
import { useAddGameModal } from '@/composables/useAddGameModal'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { useRouter } from 'vue-router'
import apiService from '@/api/apiService'

const { isUserLoggedIn } = useShowProfileAndHideLogin()

const router = useRouter()

if (!isUserLoggedIn.value) {
  router.push('/login')
}

var games = (await apiService.games.getGames()).data

const { openAddGameModal } = useAddGameModal()

const visibleGamesOnPage = 4

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

async function updateGamesView(): Promise<void> {
  games = (await apiService.games.getGames()).data

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
    class="retro-window m-auto w-10/12 relative gap-10 flex flex-col justify-between items-center"
  >
    <div class="flex flex-row text-sm mr-12 md:text-3xl pt-13 md:mr-6">
      <div
        class="text-center w-10 md:w-30 md:pr-3 md:pl-3 overflow-hidden text-ellipsis whitespace-nowrap"
      >
        Name
      </div>
      <div class="text-center w-10 md:w-30 md:pl-15">Date</div>
      <div class="text-center w-10 md:w-30 md:pl-11">Rating</div>
      <div class="text-center w-16 md:w-48 md:pl-6">Main Tag</div>
      <div class="text-center w-16 md:w-48 md:pr-6">Developer</div>
    </div>
    <div class="flex flex-col gap-5 w-11/12 md:w-216 h-auto">
      <div v-for="game in visibleGames" :key="game.id">
        <GameTableItem @deleted-game="updateGamesView" :game="game" class="hover:bg-[#98518E]" />
      </div>
    </div>

    <!-- buttons -->
    <div class="flex flex-row items-center gap-x-1 md:gap-x-10 mb-10">
      <div>
        <button
          class="text-sm bg-[#bfedef] border-black border-2 px-1 md:px-2 md:text-2xl hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
          @click="openAddGameModal"
          id="openAddGameModal"
        >
          + Add boardgame
        </button>
      </div>
      <div class="flex flex-row gap-x-3 text-base">
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
