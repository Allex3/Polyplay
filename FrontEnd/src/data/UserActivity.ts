import type { User } from './User'

export type ActivityType = {
  id: number
  info: string
}

export type UserActivity = {
  id: number
  user: User
  activityType: ActivityType
  userId: number
  activityTypeId: number
  activityTimestamp: Date
  ipAddress: string
  additionalInfo: string
}
