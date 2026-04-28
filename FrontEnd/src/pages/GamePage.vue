<script setup lang="ts">
import { computed, onBeforeMount, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useUserRepo } from '@/stores/userRepo'
import { type Game, type User, createGame } from '@/data/model'

import { useCookieManager } from '@/composables/useCookieManager'
import apiService from '@/api/apiService'

const userRepo = useUserRepo()

const { readCookie, writeCookie } = useCookieManager()

const router = useRouter()
const route = useRoute()

const currentGame = ref<Game>((await apiService.games.getGame(Number(route.params.gameid))).data)

onBeforeMount(() => {
  // for some reason it  was STILL IN ISO 8601 FORMAT, NOT A DATE
  currentGame.value.postedDate = new Date(currentGame.value.postedDate)
})

var visitedGames: object = readCookie('VisitedGames') ?? {}

if (visitedGames.hasOwnProperty(currentGame.value.id)) visitedGames[currentGame.value.id] += 1
else visitedGames[currentGame.value.id] = 1

writeCookie('VisitedGames', visitedGames, 99)

console.log(readCookie('VisitedGames'))
</script>
<template>
  <!-- Two items: For the game details, and for the chat  -->
  <div class="w-5/6 retro-window md:w-220 m-auto mt-1 flex flex-row pl-4 pr-4 pt-5 pb-5">
    <!-- Game details -->
    <div class="flex flex-col gap-2.5">
      <!-- Game image -> (name, dev, tags col) -> (follow, favorite col)-->
      <div class="flex flex-row gap-4">
        <img
          class="w-25 h-16 md:w-45 md:h-30"
          :src="currentGame.thumbnailPath"
          alt="Game Thumbnail"
        />

        <!-- name, dev, tags -->
        <div class="flex flex-col gap-2 mt-1">
          <span class="text-2xl">{{ currentGame.name }}</span>
          <span class="w-25 text-sm comic-neue"
            >developed by
            <router-link class="comic-neue-bold" to="">{{
              currentGame.developer
            }}</router-link></span
          >
          <div class="comic-neue">
            <span class="bg-[#bfedef] border text-center p-0.5 text-xs">{{
              currentGame.mainTag
            }}</span>
          </div>
        </div>

        <!-- follow, favorite buttons-->
        <div class="flex flex-col gap-2 mt-2.5 ml-5">
          <button
            class="bg-[#bfedef] font-bold comic-neue border text-xs pl-2 pr-2 hover:scale-105 hover:cursor-pointer hover:bg-[#b0ddde] active:bg-[#a5d2d2]"
          >
            Follow Dev
          </button>
          <button
            class="bg-[#bfedef] font-bold comic-neue border text-xs pl-2 pr-2 hover:scale-105 hover:cursor-pointer hover:bg-[#b0ddde] active:bg-[#a5d2d2]"
          >
            + Favorite Game
          </button>
        </div>
      </div>

      <!-- Second row: Game date, ratings, updated-->
      <div class="flex flex-row gap-10 pl-4 text-xs">
        <div class="flex flex-col">
          <span class="comic-neue text-center">posted</span>
          <span class="color-[#9E4889] text-[#9E4889] text-base">{{
            currentGame?.postedDate.getFullYear() +
            '/' +
            currentGame?.postedDate.getMonth() +
            '/' +
            currentGame?.postedDate.getDay()
          }}</span>
        </div>
        <div>
          <!-- TODO HAVE TO DO RATING SYSTEM-->
          242 ratings
        </div>
        <div class="flex flex-col">
          <span class="comic-neue text-center">updated</span>
          <span class="color-[#9E4889] text-[#9E4889] text-base">{{
            currentGame?.postedDate.getFullYear() +
            '/' +
            currentGame?.postedDate.getMonth() +
            '/' +
            currentGame?.postedDate.getDay()
          }}</span>
        </div>
      </div>

      <!-- Now 4 rows: Description title, TODO devlog sometime not now lmao to ohard-->
      <!-- so two rows-->
      <span class="text-[#6C4366] text-2xl pl-4">description</span>
      <div class="pl-4 w-1/2 h-auto text-sm comic-neue">
        {{ currentGame.description }}
      </div>
    </div>

    <!-- Chat -->
    <div class="hidden flex flex-col h-5/6 w-1/3 self-center justify-center retro-window">
      <span class="w-30">COMING SOON!!</span>
    </div>
  </div>
</template>
