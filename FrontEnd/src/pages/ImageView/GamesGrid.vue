<script setup lang="ts">
import { useRouter } from 'vue-router'
import GameImageItem from './GameImageItem.vue'
import { onBeforeMount, ref } from 'vue'
import { usePagination } from '@/composables/usePagination'

const VISIBLE_GAMES_ON_PAGE = 12 // 12 to be seen in the grid

const { visibleGames, totalPages, page, goToPage } = usePagination(VISIBLE_GAMES_ON_PAGE)
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
      <div class="text-sm text-center pt-2">{{ page }} / {{ totalPages }}</div>
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
</template>
