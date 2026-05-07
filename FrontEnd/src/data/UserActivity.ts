export type ActivityType = {
  id: number
  name: string
}

export type UserActivity = {
  id: number
  userId: number
  activityTypeId: number
  activityTimestamp: Date
  ipAddress: string
  additionalInfo: string
}
