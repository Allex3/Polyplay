import type { GameComment } from '@/data/GameComment'
import { cacheRequest } from './offlineApiSupport'
import type { UserActivity } from '@/data/UserActivity'

class UserActivityApi {
  baseURL: string

  constructor() {
    this.baseURL = 'https://localhost:7114'
  }

  private async callApi(method: string, endpoint: string, requestParams = {}) {
    const fetchData: { URL: string; options: any } = {
      URL: this.baseURL + endpoint,
      options: {
        method: method,
        mode: 'cors',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        ...requestParams, // put the requestParams object: body, etc.
      }, //TODO after POST return websocket
    }

    try {
      const response = await fetch(fetchData.URL, fetchData.options)

      if (!response.ok) {
        return { success: false, errors: await response.json() } //if NOT ok, we get the POST/PUT data validation errors
      }

      const result = await response.json()

      return { success: true, userActivitiesData: result }
    } catch (error) {
      // actual connection down or user offline
      cacheRequest(fetchData)
      return {
        success: false,
        error: error instanceof Error ? error.message : error,
      }
    }
  }

  public async getUserActivities() {
    const response = await this.callApi('GET', `/api/UserActivities`)
    return response.userActivitiesData // list, already parsed from JSON
  }

  public postUserActivity(userActivity: UserActivity) {
    const userActivityWithoutId: object = (({ id, ...restOfUserActivity }) => restOfUserActivity)(
      userActivity,
    )
    return this.callApi('POST', `/api/UserActivities`, {
      body: JSON.stringify(userActivityWithoutId),
    })
  }
}

const userActivityApi = new UserActivityApi()
export { userActivityApi as default, type UserActivityApi }
// NOTE: default export means that you can export that using any name, like import {games} from ..
