<script setup lang="ts">
import { ref } from 'vue'
import { onBeforeRouteUpdate, useRoute, useRouter } from 'vue-router'
import { useUserRepo } from '@/stores/userStore'
import { useGamesStore } from '@/stores/gameStore'
import { type Game, type User, createGame } from '@/data/model'

const userRepo = useUserRepo()
const gameStore = useGamesStore()

const router = useRouter()
const route = useRoute()

const tag = ref('')

const currentGame = ref<Game>(createGame())

if (userRepo.user.username != gameStore.getGame(Number(route.params.gameid)).developer) {
  //TODO change tihs, for testing
  router.push('/PermissionDenied')
} else {
  // correct
  currentGame.value = gameStore.getGame(Number(route.params.gameid))
}

tag.value = currentGame.value.tags[0] ?? '' //TODO get rid of this

function sendInputAndClose() {
  currentGame.value.tags = [tag.value] //TODO later make actual tags
  gameStore.updateGame(currentGame.value)
}
</script>
<template>
  <div class="w-4/5 md:w-250 md:h-140 relative retro-window m-auto z-1000">
    <div class="h-full w-full relative">
      <div class="p-8 relative overflow-scroll h-full w-full flex flex-col gap-y-3 text-lg">
        <label for="name">Name</label>
        <input
          type="text"
          id="nameInput"
          name="name"
          v-model="currentGame.name"
          class="game-input"
          placeholder="Enter name"
        />

        <label for="description">Description:</label>
        <textarea
          v-model="currentGame.description"
          id="descriptionInput"
          name="description"
          class="game-input"
          placeholder="Enter description"
        />

        <label for="tags">Tags:</label>
        <input
          type="text"
          name="tags"
          v-model="tag"
          id="tagInput"
          class="game-input"
          placeholder="Enter tag(s)"
        />

        <label for="thumbnail">Thumbnail URL:</label>
        <input
          type="text"
          name="thumbnail"
          v-model="currentGame.imagePath"
          id="imageInput"
          class="game-input"
          placeholder="Enter image URL"
        />
        <img v-bind:src="currentGame.imagePath" class="w-45" />

        <button
          @click="sendInputAndClose"
          id="saveButton"
          class="mt-5 md:mt-0 hover:cursor-pointer hover_scale bg-[#bfedef] w-30 self-center border-2"
        >
          Save Game
        </button>
      </div>
    </div>
  </div>
</template>
