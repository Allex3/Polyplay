import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { createGame, type Game } from '@/data/model.ts'

export const useUserGamesStore = defineStore('userGames', () => {
  const games = ref<Game[]>([])

  const countOfGames = computed(() => games.value.length)

  function resetGames(): void {
    games.value = []
    // for testing
    addGame(createGame({ name: 'A', id: 1 }))
    addGame(createGame({ name: 'B', id: 2 }))
    addGame(createGame({ name: 'C', id: 3 }))
    addGame(createGame({ name: 'D', id: 4 }))
    addGame(createGame({ name: 'E', id: 5 }))
    addGame(createGame({ name: 'F', id: 6 }))
    addGame(createGame({ name: 'G', id: 7 }))
    addGame(createGame({ name: 'H', id: 8 }))
    addGame(createGame({ name: 'I', id: 9 }))
    addGame(createGame({ name: 'J', id: 10 }))
  }

  function addGame(game: Game) {
    games.value.push(game)
  }

  function updateGame(updatedGame: Game) {
    const index = games.value.findIndex((game) => game.id === updatedGame.id)
    if (index !== -1) games.value[index] = updatedGame
  }

  function deleteGame(deletedGame: Game | undefined) {
    if (typeof deletedGame === 'undefined') return
    games.value = games.value.filter((game) => game.id != deletedGame?.id)
  }

  return {
    games,
    countOfGames,
    addGame,
    updateGame,
    resetGames,
    deleteGame,
  }
})
