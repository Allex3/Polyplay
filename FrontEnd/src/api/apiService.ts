import { type GamesApiInterface } from './gamesApiInterface'
import gamesApi from './gamesApiInterface'

class APIService {
  games: GamesApiInterface
  constructor() {
    this.games = gamesApi
  }
}

const apiService = new APIService()
export default apiService
