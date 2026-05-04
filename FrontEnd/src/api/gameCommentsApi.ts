import type { GameComment } from '@/data/GameComment'
import { cacheRequest } from './offlineApiSupport'

class GameCommentsApi {
  baseURL: string
  generateGamesWebSocket: undefined | WebSocket

  constructor() {
    this.baseURL = 'https://localhost:7114'
    this.generateGamesWebSocket = undefined
  }

  private async callApi(method: string, endpoint: string, requestParams = {}) {
    const fetchData: { URL: string; options: any } = {
      URL: this.baseURL + endpoint,
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

      return { success: true, gameCommentsData: result }
    } catch (error) {
      // actual connection down or user offline
      cacheRequest(fetchData)
      return {
        success: false,
        error: error instanceof Error ? error.message : error,
      }
    }
  }

  public async getGameComments(gameId: number) {
    // 12 by default
    const response = await this.callApi('GET', `/api/GameComments?gameId=${gameId}`)
    return response.gameCommentsData // list, already parsed from JSON
  }

  public postGameComment(gameComment: GameComment) {
    const gameCommentWithoutID: object = (({ id, ...restOfGameComment }) => restOfGameComment)(
      gameComment,
    )
    return this.callApi('POST', '/api/gameComments', { body: JSON.stringify(gameCommentWithoutID) })
  }

  public putGameComment(gameComment: GameComment) {
    return this.callApi('PUT', `/api/gameComments/${gameComment.id}`, {
      body: JSON.stringify(gameComment),
    })
  }
  public deleteGameComment(id: number) {
    return this.callApi('DELETE', `/api/gameComments/${id}`)
  }
}

const gameCommentsApi = new GameCommentsApi()
export { gameCommentsApi as default, type GameCommentsApi }
// NOTE: default export means that you can export that using any name, like import {games} from ..
