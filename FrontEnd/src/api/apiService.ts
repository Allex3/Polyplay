import { type GamesApi } from './gamesApi'
import gamesApi from './gamesApi'
import { type GameCommentsApi } from './gameCommentsApi.ts'
import gameCommentsApi from './gameCommentsApi.ts'
import type { UsersApi } from './usersApi.ts'
import usersApi from './usersApi.ts'
import type { GeneralChatApi } from './generalChatApi.ts'
import generalChatApi from './generalChatApi.ts'
import type { UserActivityApi } from './userActivityApi.ts'
import userActivityApi from './userActivityApi.ts'
import type { MaliciousActivityApi } from './maliciousActivityApi.ts'
import maliciousActivityApi from './maliciousActivityApi.ts'

class APIService {
  games: GamesApi
  gamesComments: GameCommentsApi
  users: UsersApi
  generalChat: GeneralChatApi
  userActivity: UserActivityApi
  maliciousActivity: MaliciousActivityApi
  constructor() {
    this.games = gamesApi
    this.gamesComments = gameCommentsApi
    this.users = usersApi
    this.generalChat = generalChatApi
    this.userActivity = userActivityApi
    this.maliciousActivity = maliciousActivityApi
  }
}

const apiService = new APIService()
export default apiService
