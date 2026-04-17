<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useAddGameModal } from '@/composables/useAddGameModal'
import { createGame } from '@/data/model'
import { useGamesStore } from '@/stores/gameRepo'

const gamesStore = useGamesStore()
const { isAddGameModalOpen, closeAddGameModal } = useAddGameModal()

const gameFromForm = reactive(createGame())
const tagList: string[] = reactive([])
const mainTag = ref('')

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
    id: gamesStore.getNewID(),
    name: gameFromForm.name,
    description: gameFromForm.description,
    postedDate: gameFromForm.postedDate,
    imagePath: gameFromForm.imagePath,
    rating: 0.0,
    tags: [mainTag.value], // TODO remake this later but god not now
    developer: 'Idkkk',
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
      class="fixed inset-0 z-1000 flex items-center justify-center backdrop-brightness-35"
    >
      <div class="w-100 h-155 relative retro-window">
        <div class="h-full w-full relative">
          <div class="p-8 relative overflow-scroll h-full w-full flex flex-col gap-y-3 text-lg">
            <label for="name" class="-z-5">Name</label>
            <input
              type="text"
              name="name"
              v-model="gameFromForm.name"
              class="game-input"
              placeholder="Enter name"
            />

            <label class="-z-5" for="description">Description:</label>
            <textarea
              v-model="gameFromForm.description"
              name="description"
              class="game-input"
              placeholder="Enter description"
            />

            <label class="-z-5" for="tags">Tags:</label>
            <input
              type="text"
              name="tags"
              v-model="mainTag"
              class="game-input"
              placeholder="Enter tag(s)"
            />

            <label class="-z-5" for="thumbnail">Thumbnail URL:</label>
            <input
              type="text"
              name="thumbnail"
              v-model="gameFromForm.imagePath"
              class="game-input"
              placeholder="Enter image URL"
            />
            <img v-bind:src="gameFromForm.imagePath" class="w-45" />

            <button
              @click="sendInputAndClose"
              class="-z-5 hover:cursor-pointer hover_scale bg-[#bfedef] w-30 self-center border-2"
            >
              Add Game
            </button>
          </div>
        </div>
      </div>
    </div>
  </Teleport>
</template>
