<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useAddGameModal } from '@/composables/useAddGameModal'
import { createGame } from '@/data/model'
import { useGamesStore } from '@/stores/gameStore'
import { useUserRepo } from '@/stores/userStore'

const gamesStore = useGamesStore()
const { isAddGameModalOpen, closeAddGameModal } = useAddGameModal()

const gameFromForm = reactive(createGame())
const tagList: string[] = reactive([])
const mainTag = ref('')

const userRepo = useUserRepo()

const isInvalidFormat = ref(false) // show error when true
const errorText = ref('')

const emit = defineEmits(['addedGame'])
//TODO SEE HOW TO DO EVENT VALIDATION but i dont think its needed here
// since we don't submit the variables or the game..

document.cookie

function gameFormatIsValid(): boolean {
  isInvalidFormat.value = false

  if (gameFromForm.name == '') {
    errorText.value = 'Name field should not be empty.'
    isInvalidFormat.value = true
  } else if (gameFromForm.description == '') {
    errorText.value = 'Description field should not be empty.'
    isInvalidFormat.value = true
  } else if (mainTag.value == '') {
    errorText.value = 'Main Tag field should not be empty.'
    isInvalidFormat.value = true
  } else if (gameFromForm.imagePath == '') {
    errorText.value = 'Image field should not be empty.'
    isInvalidFormat.value = true
  }
  if (isInvalidFormat.value) return false

  return true
}

function sendInputAndClose(): void {
  if (!gameFormatIsValid()) {
    return
  }
  const newGame = createGame({
    id: gamesStore.getNewID(),
    name: gameFromForm.name,
    description: gameFromForm.description,
    postedDate: gameFromForm.postedDate,
    imagePath: gameFromForm.imagePath,
    rating: 0.0,
    tags: [mainTag.value], // TODO remake this later but god not now
    developer: userRepo.user.username,
  })

  gamesStore.addGame(newGame)
  emit('addedGame')
  closeAddGameModal()
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
      <div class="w-2/3 mt-20 md:w-100 md:mt-0 md:min-h-2/3 relative retro-window">
        <div class="h-full w-full">
          <div class="p-8 overflow-scroll h-full w-full flex flex-col gap-y-3 text-lg">
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

            <label for="tags">Tags:</label>
            <input
              id="addGameTagField"
              type="text"
              name="tags"
              v-model="mainTag"
              class="game-input"
              placeholder="Enter tag(s)"
            />

            <label for="thumbnail">Thumbnail URL:</label>
            <input
              id="addGameImageField"
              type="text"
              name="thumbnail"
              v-model="gameFromForm.imagePath"
              class="game-input"
              placeholder="Enter image URL"
            />
            <img v-bind:src="gameFromForm.imagePath" class="w-45" />

            <span class="text-[red]" v-show="isInvalidFormat" id="addGameError">{{
              errorText
            }}</span>
            <button
              id="addGameButton"
              @click="sendInputAndClose"
              class="mt-5 md:mt-0 hover:cursor-pointer hover_scale bg-[#bfedef] w-30 self-center border-2"
            >
              Add Game
            </button>
          </div>
        </div>
      </div>
    </div>
  </Teleport>
</template>
