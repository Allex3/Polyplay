import { type GamesApi } from './gamesApi'
import gamesApi from './gamesApi'
import { type GameCommentsApi } from './gameCommentsApi.ts'
import gameCommentsApi from './gameCommentsApi.ts'
import type { UsersApi } from './usersApi.ts'
import usersApi from './usersApi.ts'
import type { GeneralChatApi } from './generalChatApi.ts'
import generalChatApi from './generalChatApi.ts'

class APIService {
  games: GamesApi
  gamesComments: GameCommentsApi
  users: UsersApi
  generalChat: GeneralChatApi
  constructor() {
    this.games = gamesApi
    this.gamesComments = gameCommentsApi
    this.users = usersApi
    this.generalChat = generalChatApi
  }
}

const apiService = new APIService()
export default apiService
