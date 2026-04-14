import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { createGame, type Game } from '@/data/model.ts'

export const useUserGamesStore = defineStore('userGames', () => {
  const games = ref<Game[]>([])

  const countOfGames = computed(() => games.value.length)

  function resetGames(): void {
    games.value = []
  }

  function getGame(id: Number): Game | undefined {
    const index = games.value.findIndex((game) => game.id === id)
    if (index === -1) throw new Error(`Get Game: Game with id ${id} does NOT exist`)
    return games.value[index]
  }

  function addGame(game: Game) {
    games.value.push(game)
  }

  function updateGame(updatedGame: Game) {
    const index = games.value.findIndex((game) => game.id === updatedGame.id)
    if (index === -1) throw new Error(`Update Game: Game with id ${updatedGame.id} does NOT exist`)
    games.value[index] = updatedGame // all good
  }

  function deleteGame(deletedGame: Game | undefined) {
    if (typeof deletedGame === 'undefined') throw new Error(`Delete Game: Game is undefined`)
    games.value = games.value.filter((game) => game.id != deletedGame?.id)
  }

  return {
    games,
    countOfGames,
    getGame,
    addGame,
    updateGame,
    resetGames,
    deleteGame,
  }
})
