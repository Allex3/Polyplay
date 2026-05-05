import type { Game } from '@/data/Game'
import { cacheRequest } from './offlineApiSupport'
import type { GeneralChatMessage } from '@/data/GeneralChatMessage'

class GamesApi {
  baseURL: string
  generalChatWebSocket: undefined | WebSocket

  constructor() {
    this.baseURL = 'https://localhost:7114'
    this.generalChatWebSocket = undefined
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

  public startChatConnection(updateView: Function) {
    this.generalChatWebSocket = new WebSocket(this.baseURL + '/ws/startTestGames')

    this.generalChatWebSocket.onopen = function () {
      console.log('Connected to general chat')
    }
    this.generalChatWebSocket.onmessage = async function (event) {
      // received a message (broadcasted to all)
      const message: GeneralChatMessage = JSON.parse(event.data)
      await updateView(message) // update view with new message
    }
    this.generalChatWebSocket.onclose = function () {
      console.log('Disconnected from general chat')
    }
  }

  public stopChatConnection() {
    this.generalChatWebSocket?.close() // close connection
  }
}

const gamesApi = new GamesApi()
export { gamesApi as default, type GamesApi }
// NOTE: default export means that you can export that using any name, like import {games} from ..
