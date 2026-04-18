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

function isScrolledIntoView(el: HTMLElement) {
  var rect = el.getBoundingClientRect()
  var elemTop = rect.top
  var elemBottom = rect.bottom

  // Only completely visible elements return true:
  //var isVisible = (elemTop >= 0) && (elemBottom <= document.getElementById('add-window')?.getBoundingClientRect().height);
  // Partially visible elements return true:
  //isVisible = elemTop < window.innerHeight && elemBottom >= 0;
  //return isVisible;
}

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
      class="fixed inset-0 z-11 flex items-center justify-center backdrop-brightness-35"
    >
      <div class="w-100 h-100 relative retro-window">
        <div class="h-full w-full">
          <div class="p-8 overflow-scroll h-full w-full flex flex-col gap-y-3 text-lg">
            <label for="name">Name</label>
            <input
              type="text"
              name="name"
              v-model="gameFromForm.name"
              class="game-input"
              placeholder="Enter name"
            />

            <label for="description">Description:</label>
            <textarea
              v-model="gameFromForm.description"
              name="description"
              class="game-input"
              placeholder="Enter description"
            />

            <label for="tags">Tags:</label>
            <input
              type="text"
              name="tags"
              v-model="mainTag"
              class="game-input"
              placeholder="Enter tag(s)"
            />

            <label for="thumbnail">Thumbnail URL:</label>
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
              class="hover:cursor-pointer hover_scale bg-[#bfedef] w-30 self-center border-2"
            >
              Add Game
            </button>
          </div>
        </div>
      </div>
    </div>
  </Teleport>
</template>
