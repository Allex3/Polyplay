<script setup lang="ts">
import { ref } from 'vue'
import { onBeforeRouteUpdate, useRoute, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/userStore'
import { type Game, createGame } from '@/data/Game'
import { type User } from '@/data/User'
import apiService from '@/api/apiService'
import { usePostPutApiCallWithErrors } from '@/composables/usePostPutApiCallWithErrors'

const userStore = useUserStore()

const router = useRouter()
const route = useRoute()

const tags = ref([])

const { validateInput } = usePostPutApiCallWithErrors()

const isInvalidFormat = ref(false)
const errorText = ref('')
const isSavedSuccessfully = ref(false)
const successText = ref('')

if (!(await apiService.games.getGame(Number(route.params.gameid))).success)
  router.push('/PermissionDenied') // game doesn't exist, 404 probably

const currentGame = ref<Game>(
  (await apiService.games.getGame(Number(route.params.gameid))).gamesData,
)

if (userStore.user.username != currentGame.value.developer)
  //TODO change tihs, for testing
  router.push('/PermissionDenied')

async function sendInputAndClose() {
  //TODO later make actual tags

  const response = await apiService.games.putGame(currentGame.value)
  validateInput(
    response,
    isInvalidFormat,
    isSavedSuccessfully,
    errorText,
    successText,
    'Saved Successfully',
  )
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
          v-model="currentGame.mainTag"
          id="tagInput"
          class="game-input"
          placeholder="Enter tag(s)"
        />

        <label for="thumbnail">Thumbnail URL:</label>
        <input
          type="text"
          name="thumbnail"
          v-model="currentGame.thumbnailPath"
          id="imageInput"
          class="game-input"
          placeholder="Enter image URL"
        />
        <img v-bind:src="currentGame.thumbnailPath" class="w-45" />
        <span class="text-[red] text-center" v-show="isInvalidFormat" id="addGameError">{{
          errorText
        }}</span>
        <span
          class="text-[green] text-center"
          v-show="isSavedSuccessfully"
          id="addGameSuccessfully"
          >{{ successText }}</span
        >
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
