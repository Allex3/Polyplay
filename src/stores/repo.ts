import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { Game } from '@/data/model.ts'

export const useGames = defineStore('games', () => {
  const games = ref<Game[]>([])

  const countOfGames = computed(() => games.value.length)

  function resetGames(): void {
    games.value = []
  }

  function addGame(game: Game) {
    games.value.push(game)
  }

  function updateGame(updatedGame: Game) {
    const index = games.value.findIndex((game) => game.id === updatedGame.id)
    if (index !== -1) games.value[index] = updatedGame
  }

  function deleteGame(deletedGame: Game) {
    games.value = games.value.filter((game) => game.id !== deletedGame.id)
  }

  return {
    games,
    countOfGames,
    addGame,
    deleteGame,
  }
})
