import type { User } from './User'

export type ActivityType = {
  id: number
  info: string
}

export type UserActivity = {
  id: number
  user: User | null
  activityType: ActivityType | null
  userId: number
  activityTypeId: number
  activityTimestamp: Date
  ipAddress: string
  additionalInfo: string
}

export function createUserActivity(data: Partial<UserActivity>): UserActivity {
  return {
    id: data.id ?? 0,
    user: null,
    activityType: null,
    userId: data.userId ?? 0,
    activityTypeId: data.activityTypeId ?? 0,
    activityTimestamp: new Date(),
    ipAddress: 'no',
    additionalInfo: data.additionalInfo ?? '',
  }
}

export enum USER_ACTIVITIES {
  LOGIN_FAILED = 1,
  POST_GAME = 2,
  PUT_GAME = 3,
  DELETE_GAME = 4,
  POST_GENERAL_CHAT = 5,
  POST_GAME_COMMENT = 6,
  PUT_GAME_COMMENT = 7,
  DELETE_GAME_COMMENT = 8,
}
