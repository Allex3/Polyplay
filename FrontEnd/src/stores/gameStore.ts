/*
import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { createGame, type Game } from '@/data/model.ts'
import apiService from '@/api/apiService'

export const useGamesStore = defineStore('games', async () => {
  const games = (await apiService.games.getGames()).data

  const maxID = ref(0)

  const countOfGames = computed(() => games.value.length)

  function resetGames(): void {
    games.value = []
  }

  function getNewID(): number {
    return ++maxID.value
  }

  function getGame(id: number): Game {
    const game = games.value.find((game) => game.id === id)
    if (game === undefined) throw new Error(`Get Game: Game with id ${id} does NOT exist`)
    return game
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
    getNewID,
    getGame,
    addGame,
    updateGame,
    resetGames,
    deleteGame,
  }
})
*/
import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { createGame, type Game } from '@/data/model.ts'
export const useGamesStore = defineStore('games', async () => {})
