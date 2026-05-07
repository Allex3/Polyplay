import type { GameComment } from '@/data/GameComment'
import { cacheRequest } from './offlineApiSupport'
import type { UserActivity } from '@/data/UserActivity'
import type { MaliciousActivity } from '@/data/MaliciousActivity'

class MaliciousActivityApi {
  constructor() {}

  private async callApi(method: string, endpoint: string, requestParams = {}) {
    const fetchData: { URL: string; options: any } = {
      URL: endpoint,
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

      return { success: true, maliciousActivitiesData: result }
    } catch (error) {
      // actual connection down or user offline
      cacheRequest(fetchData)
      return {
        success: false,
        error: error instanceof Error ? error.message : error,
      }
    }
  }

  public async getMaliciousActivities() {
    const response = await this.callApi('GET', `/api/MaliciousActivities`)
    return response.maliciousActivitiesData // list, already parsed from JSON
  }

  public postMaliciousActivity(maliciousActivity: MaliciousActivity) {
    const maliciousActivityWithoutId: object = (({ id, ...restOfMaliciousActivity }) =>
      restOfMaliciousActivity)(maliciousActivity)
    return this.callApi('POST', `/api/MaliciousActivities`, {
      body: JSON.stringify(maliciousActivityWithoutId),
    })
  }
}

const maliciousActivityApi = new MaliciousActivityApi()
export { maliciousActivityApi as default, type MaliciousActivityApi }
// NOTE: default export means that you can export that using any name, like import {games} from ..
