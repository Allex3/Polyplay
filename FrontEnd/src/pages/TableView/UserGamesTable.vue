<script setup lang="ts">
import GameTableItem from './GameTableItem.vue'

import AddGameModal from './AddGameModal.vue'
import { useAddGameModal } from '@/composables/useAddGameModal'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { useRouter } from 'vue-router'
import { usePagination } from '@/composables/usePagination'

import { useInfiniteScroll } from '@vueuse/core'
import { ref, useTemplateRef } from 'vue'
import type { Game } from '@/data/model'
import apiService from '@/api/apiService'

const { isUserLoggedIn } = useShowProfileAndHideLogin()

const router = useRouter()

if (!isUserLoggedIn.value) {
  router.push('/login')
}

const { openAddGameModal } = useAddGameModal()

const infiniteScrollContainer = useTemplateRef('infiniteScrollContainer')

const VISIBLE_GAMES_ON_PAGE = 4

var pageNumber = 1

const visibleGames = ref<Game[]>(
  (await apiService.games.getGames(pageNumber, VISIBLE_GAMES_ON_PAGE)).games,
)

const totalGames = (await apiService.games.getGames(pageNumber, VISIBLE_GAMES_ON_PAGE)).totalGames
var loadedGames = 4

// here, I will act like "pages" are the times i reach the end of scroll
// basically, first page with 4 games, then second with 4 gamees, and so on
// putting 4 games in the infinite scroll each time we reach them
const { reset } = useInfiniteScroll(
  infiniteScrollContainer,
  async () => {
    // onLoadMore()
    pageNumber += 1 // next 4 games
    loadedGames += VISIBLE_GAMES_ON_PAGE // should get >= totalGames to stop
    visibleGames.value.push(
      ...(await apiService.games.getGames(pageNumber, VISIBLE_GAMES_ON_PAGE)).games,
    )
    console.log(visibleGames)
  },
  {
    // options
    distance: 10,
    canLoadMore: () => {
      if (loadedGames >= totalGames) return false
      return true
    },
  },
)
const resetGamesList = reset
</script>

<template>
  <div
    class="retro-window max-h-2/3 m-auto w-10/12 relative gap-10 flex flex-col justify-between items-center"
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
    <div
      ref="infiniteScrollContainer"
      class="flex flex-col gap-5 p-4 h-80 m-auto overflow-y-scroll w-11/12 md:w-216"
    >
      <div v-for="game in visibleGames" :key="game.id">
        <GameTableItem @deleted-game="resetGamesList" :game="game" class="hover:bg-[#98518E]" />
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
          + Add Game
        </button>
      </div>
    </div>
  </div>

  <AddGameModal @added-game="resetGamesList" />
</template>
