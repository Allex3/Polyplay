<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useAddGameModal } from '@/composables/useAddGameModal'
import { createGame } from '@/data/model'
import { useUserGamesStore } from '@/stores/repo'

const userGamesStore = useUserGamesStore()
const { isAddGameModalOpen, closeAddGameModal } = useAddGameModal()

const gameFromForm = reactive(createGame())
const tagList: string[] = []

const emit = defineEmits(['addedGame'])
//TODO SEE HOW TO DO EVENT VALIDATION but i dont think its needed here
// since we don't submit the variables or the game..

function gameFormatIsValid(): boolean {
  return true
}

function sendInputAndClose(): void {
  if (!gameFormatIsValid()) {
    // idk show error smth text help
    return
  }
  const newGame = createGame({
    id: 0,
    name: gameFromForm.name,
    description: gameFromForm.description,
    postedDate: gameFromForm.postedDate,
    imagePath: gameFromForm.imagePath,
    rating: 0.0,
    tags: tagList,
    developer: 'Idkkk',
  })

  userGamesStore.addGame(newGame)
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
      class="fixed inset-0 z-1000 flex items-center justify-center backdrop-brightness-50"
    >
      <div class="p-8 retro-window relative flex h-3/4 w-2/5 flex-col gap-y-3 text-lg">
        <span>Name</span>
        <input
          type="text"
          v-model="gameFromForm.name"
          class="retro-input"
          placeholder="Enter name"
        />

        <span>Description:</span>
        <textarea
          v-model="gameFromForm.description"
          class="retro-input"
          placeholder="Enter description"
        />

        <span>Thumbnail URL:</span>
        <input
          type="text"
          v-model="gameFromForm.imagePath"
          class="retro-input"
          placeholder="Enter image URL"
        />
        <img v-bind:src="gameFromForm.imagePath" />

        <button
          @click="sendInputAndClose"
          class="hover:cursor-pointer hover_scale bg-[#bfedef] w-30 self-center border-2"
        >
          Add Game
        </button>
      </div>
    </div>
  </Teleport>
</template>
