import { type GamesApi } from './gamesApi'
import gamesApi from './gamesApi'
import { type GameCommentsApi } from './gameCommentsApi.ts'
import gameCommentsApi from './gameCommentsApi.ts'
import type { UsersApi } from './usersApi.ts'
import usersApi from './usersApi.ts'

class APIService {
  games: GamesApi
  gamesComments: GameCommentsApi
  users: UsersApi
  constructor() {
    this.games = gamesApi
    this.gamesComments = gameCommentsApi
    this.users = usersApi
  }
}

const apiService = new APIService()
export default apiService
