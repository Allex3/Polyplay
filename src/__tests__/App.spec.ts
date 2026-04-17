import { describe, test, it, expect, beforeAll } from 'vitest'
import { useUserGamesStore } from '../stores/gameRepo'
import { mount } from '@vue/test-utils'
import App from '../App.vue'
import { createGame } from '@/data/model'

import { setActivePinia, createPinia, type Store } from 'pinia'

setActivePinia(createPinia())
var gameStore = useUserGamesStore()

describe('Games CRUD', () => {
  beforeAll(async () => {
    return
  })
  test('Create Empty Game', () => {
    const game = createGame()
    expect(game).toEqual(createGame(game)) // create identical game
  })

  test('Add Game', () => {
    const game = createGame({ id: 2 })
    gameStore.addGame(game)
    const addedGame = gameStore.getGame(game.id)
    expect(game).toEqual(addedGame) // it is added correctly
  })

  test('Delete Existent Game', () => {
    const addedGame = gameStore.getGame(2)
    gameStore.deleteGame(addedGame)
    expect(() => gameStore.getGame(addedGame.id)).toThrow()
  })

  test('Delete Non-existent Game', () => {
    expect(() => gameStore.deleteGame(undefined)).toThrow()
  })

  test('Update Existent Game', () => {
    const game = createGame({ id: 2 })
    gameStore.addGame(game)
    game.description = 'New Description'
    gameStore.updateGame(game) // updated version

    const updatedGame = gameStore.getGame(game.id)
    expect(game.description).toEqual(updatedGame?.description) // it is updated correctly
  })
  test('Update Non-existent Game', () => {
    const game = createGame({ id: 3 })
    game.description = 'New Description'

    // updated version, but game doesn't exist in the list..
    expect(() => gameStore.updateGame(game)).toThrow()
  })

  test('Correct Game Count', () => {
    const gameCount = gameStore.countOfGames
    expect(gameCount).toBe(1)
  })

  test('Reset Games Correctly', () => {
    gameStore.resetGames()
    const gameCount = gameStore.countOfGames
    expect(gameCount).toBe(0)
  })
})
