import { type GamesApi } from './gamesApi'
import gamesApi from './gamesApi'
import { gameCommentsApi, type GameCommentsApi } from './gameCommentsApi.ts'
import gamesComments from './gameCommentsApi.ts'

class APIService {
  games: GamesApi
  gamesComments: GameCommentsApi
  constructor() {
    this.games = gamesApi
    this.gamesComments = gameCommentsApi
  }
}

const apiService = new APIService()
export default apiService
