<script setup lang="ts">
import { ref, reactive, useTemplateRef } from 'vue'
import { useAddGameModal } from '@/composables/useAddGameModal'
import { createGame, type Game } from '@/data/Game'
import { useUserStore } from '@/stores/userStore'
import apiService from '@/api/apiService'

const { isAddGameModalOpen, closeAddGameModal } = useAddGameModal()

const userStore = useUserStore()

const gameFromForm = reactive(createGame({ developer: userStore.user.username }))
const tagList: string[] = reactive([])

const isInvalidFormat = ref(false) // show error when true
const errorText = ref('')

const emit = defineEmits(['addedGame'])
//TODO SEE HOW TO DO EVENT VALIDATION but i dont think its needed here
// since we don't submit the variables or the game..

async function sendInputAndClose(): Promise<void> {
  /*
  const newGame = createGame({
    name: gameFromForm.name,
    description: gameFromForm.description,
    postedDate: gameFromForm.postedDate,
    thumbnailPath: gameFromForm.thumbnailPath,
    rating: 0.0,
    mainTag: gameFromForm.mainTag, // TODO remake this later but god not now
    developer: userRepo.user.username,
  })
  */
  const addGameButton: any = document.getElementById('addGameButton')
  addGameButton.disabled = true

  const response = await apiService.games.postGame(gameFromForm)

  // if no success we got the "errors" JSON, that is basically objects with
  // "name" , "maintag", etc. properties with their given errors, already as a JS object
  const errors: object = response.errors

  if (
    response.success ||
    errors === undefined
  ) // no errors but failure => no connection, act as added because cache
  {
    closeAddGameModal()
    emit('addedGame')

    // reset game when opening add modal again
    gameFromForm.name = ''
    gameFromForm.description = ''
    gameFromForm.mainTag = ''

    addGameButton.disabled = false
    return
  }

  // append all errors to the error text:
  errorText.value = ''
  for (const [key, value] of Object.entries(errors)) {
    errorText.value += key + ': ' + value + '\n'
  }

  isInvalidFormat.value = true
}

function closeOnBackdropClick(e: Event) {
  if (e.target === e.currentTarget) closeAddGameModal()
}
</script>

<template>
  <Teleport to="body">
    <div
      v-show="isAddGameModalOpen"
      @click="closeOnBackdropClick"
      class="fixed inset-0 z-11 flex items-center justify-center backdrop-brightness-30"
    >
      <div class="w-2/3 h-2/3 mt-20 md:w-100 md:mt-0 md:min-h-2/3 relative retro-window">
        <div class="p-8 h-full overflow-scroll flex flex-col gap-y-3 text-lg">
          <label for="name">Name</label>
          <input
            id="addGameNameField"
            type="text"
            name="name"
            v-model="gameFromForm.name"
            class="game-input"
            placeholder="Enter name"
          />

          <label for="description">Description:</label>
          <textarea
            id="addGameDescriptionField"
            v-model="gameFromForm.description"
            name="description"
            class="game-input min-h-10"
            placeholder="Enter description"
          />

          <label for="tags">Main Tag:</label>
          <input
            id="addGameTagField"
            type="text"
            name="tags"
            v-model="gameFromForm.mainTag"
            class="game-input"
            placeholder="Enter tag(s)"
          />

          <label for="thumbnail">Thumbnail URL:</label>
          <input
            id="addGameImageField"
            type="text"
            name="thumbnail"
            v-model="gameFromForm.thumbnailPath"
            class="game-input"
            placeholder="Enter image URL"
          />
          <img v-bind:src="gameFromForm.thumbnailPath" class="w-45" />

          <span class="text-[red]" v-show="isInvalidFormat" id="addGameError">{{ errorText }}</span>
          <button
            id="addGameButton"
            ref="addGameButton"
            @click="sendInputAndClose"
            class="mt-5 md:mt-0 hover:cursor-pointer hover_scale bg-[#bfedef] w-30 self-center border-2"
          >
            Add Game
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>
