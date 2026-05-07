import type { Game } from '@/data/Game'
import { cacheRequest } from './offlineApiSupport'

class GamesApi {
  generateGamesWebSocket: undefined | WebSocket

  constructor() {
    this.generateGamesWebSocket = undefined
  }

  private async callApi(method: string, endpoint: string, requestParams = {}) {
    const fetchData: { URL: string; options: any } = {
      URL: endpoint,
      options: {
        method: method,
        mode: 'cors',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        ...requestParams, // put the requestParams object: body, etc.
      }, //TODO after POST return websocket
    }

    try {
      const response = await fetch(fetchData.URL, fetchData.options)

      if (!response.ok) {
        return { success: false, errors: await response.json() } //if NOT ok, we get the POST/PUT data validation errors
      }

      const result = await response.json()

      return { success: true, gamesData: result }
    } catch (error) {
      // actual connection down or user offline
      cacheRequest(fetchData)
      return {
        success: false,
        error: error instanceof Error ? error.message : error,
      }
    }
  }

  public async getGames(pageNumber: number, pageSize: number = 12) {
    // 12 by default
    const response = await this.callApi(
      'GET',
      `/api/games?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    )
    if (response.gamesData == undefined) return { games: [], totalGames: 0 } // request didn't work
    return { games: response.gamesData.data, totalGames: response.gamesData.totalRecords }
  }

  public async getGame(id: number) {
    return this.callApi('GET', `/api/games/${id}`)
  }

  public postGame(game: Game) {
    const gameWithoutID: object = (({ id, ...restOfGame }) => restOfGame)(game)
    return this.callApi('POST', '/api/games', { body: JSON.stringify(gameWithoutID) })
  }

  public putGame(game: Game) {
    return this.callApi('PUT', `/api/games/${game.id}`, { body: JSON.stringify(game) })
  }
  public deleteGame(id: number) {
    return this.callApi('DELETE', `/api/games/${id}`)
  }

  public generateTestGames(updateView: Function) {
    this.generateGamesWebSocket = new WebSocket('/ws/startTestGames')

    this.generateGamesWebSocket.onopen = function (event) {
      this.send('connection started') // announce it can start the loop
    }
    this.generateGamesWebSocket.onmessage = async function (event) {
      this.send('received') // the server is awaiting "handshake" to know to continue after generating and sleeping for 3 seconds
      await updateView() // update view with new games
    }
  }

  public stopGeneratingTestGames() {
    this.generateGamesWebSocket?.close() // close connection
  }

  public getTagStats() {
    return this.callApi('GET', '/api/games/stats')
  }
}

const gamesApi = new GamesApi()
export { gamesApi as default, type GamesApi }
// NOTE: default export means that you can export that using any name, like import {games} from ..
