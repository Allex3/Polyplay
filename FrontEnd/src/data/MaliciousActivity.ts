import type { User } from './User'
import type { ActivityType } from './UserActivity'

export type MaliciousActivity = {
  id: number
  user: User | null
  activityType: ActivityType | null
  userId: number
  activityTypeId: number
  ipAddress: string
  info: string
}

export function createMaliciousActivity(data: Partial<MaliciousActivity>): MaliciousActivity {
  return {
    id: data.id ?? 0,
    user: null,
    activityType: null,
    userId: data.userId ?? 0,
    activityTypeId: data.activityTypeId ?? 0,
    ipAddress: 'no',
    info: data.info ?? '',
  }
}
