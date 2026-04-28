import type { Game } from '@/data/model'

class GamesApiInterface {
  baseURL: string

  constructor() {
    this.baseURL = 'https://localhost:7114'
  }

  private async callApi(method: string, endpoint: string, requestParams = {}) {
    try {
      const response = await fetch(this.baseURL + endpoint, {
        method: method,
        mode: 'cors',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        ...requestParams, // put the requestParams object: body, etc.
      })

      if (!response.ok) {
        return { success: false, errors: await response.json() } //if NOT ok, we get the POST/PUT data validation errors
      }

      const result = await response.json()

      return { success: true, gamesData: result }
    } catch (error) {
      return { success: false, error: error instanceof Error ? error.message : error }
    }
  }

  public async getGames(pageNumber: number, pageSize: number = 12) {
    // 12 by default
    const response = await this.callApi(
      'GET',
      `/api/games?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    )
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
}

const gamesApi = new GamesApiInterface()
export { gamesApi as default, type GamesApiInterface }
// NOTE: default export means that you can export that using any name, like import {games} from ..
