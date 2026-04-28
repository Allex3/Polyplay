<script setup lang="ts">
import GameTableItem from './GameTableItem.vue'

import AddGameModal from './AddGameModal.vue'
import { useAddGameModal } from '@/composables/useAddGameModal'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { useRouter } from 'vue-router'
import { usePagination } from '@/composables/usePagination'

const { isUserLoggedIn } = useShowProfileAndHideLogin()

const router = useRouter()

if (!isUserLoggedIn.value) {
  router.push('/login')
}

const { openAddGameModal } = useAddGameModal()

const VISIBLE_GAMES_ON_PAGE = 4

const { visibleGames, totalPages, page, goToPage } = usePagination(VISIBLE_GAMES_ON_PAGE)

function updateGamesView() {
  goToPage(page.value) // reset the current page basically
}
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
          + Add Game
        </button>
      </div>
      <div class="flex flex-row gap-x-3 text-base">
        <button
          class="bg-[#bfedef] border-black border-2 px-2 hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
          @click="goToPage(1)"
        >
          &lt;&lt;
        </button>
        <button
          class="bg-[#bfedef] border-black border-2 px-2 hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
          @click="goToPage(--page)"
        >
          &lt;
        </button>
        <div>{{ page }} / {{ totalPages }}</div>
        <button
          class="bg-[#bfedef] border-black border-2 px-2 hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
          @click="goToPage(++page)"
        >
          &gt;
        </button>
        <button
          class="bg-[#bfedef] border-black border-2 px-2 hover:bg-[#a6ebee] hover:cursor-pointer active:bg-[#88d8dc]"
          @click="goToPage(totalPages)"
        >
          &gt;&gt;
        </button>
      </div>
    </div>
  </div>

  <AddGameModal @added-game="updateGamesView" />
</template>
