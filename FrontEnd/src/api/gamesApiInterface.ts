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
        throw new Error(response.statusText)
      }

      const result = await response.json()

      return { success: true, data: result }
    } catch (error) {
      return { success: false, error: error instanceof Error ? error.message : error }
    }
  }

  public getGames() {
    return this.callApi('GET', '/api/games')
  }
}

const gamesApi = new GamesApiInterface()
export { gamesApi as default, type GamesApiInterface }
// NOTE: default export means that you can export that using any name, like import {games} from ..
